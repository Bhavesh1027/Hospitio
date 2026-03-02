using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;
public record CreateCustomersGuestJourneysRequest(CreateCustomersGuestJourneysIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomersGuestJourneysHandler : IRequestHandler<CreateCustomersGuestJourneysRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;

    public CreateCustomersGuestJourneysHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomersGuestJourneysRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.CustomerGuestJournies.Where(e => e.JourneyStep == request.In.JourneyStep).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The journey Step {request.In.JourneyStep} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}
        var customerGuestJournies = new CustomerGuestJourny();
        customerGuestJournies.CutomerId = request.In.CustomerId;
        await _db.CustomerGuestJournies.AddAsync(customerGuestJournies, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        int uniqueId = customerGuestJournies.Id;
        string stepName = ((JourneyStepTempleteTypeEnums)request.In.JourneyStep).ToString();

        var customersGuestJourney = await _db.CustomerGuestJournies.Where(x => x.Id == uniqueId).FirstOrDefaultAsync(cancellationToken);

        var jsonButtons = JsonConvert.SerializeObject(request.In.Buttons);

        customersGuestJourney.CutomerId = request.In.CustomerId;
        customersGuestJourney.JourneyStep = request.In.JourneyStep;
        customersGuestJourney.Name = request.In.Name;
        customersGuestJourney.SendType = request.In.SendType;
        customersGuestJourney.TimingOption1 = request.In.TimingOption1;
        customersGuestJourney.TimingOption2 = request.In.TimingOption2;
        customersGuestJourney.TimingOption3 = request.In.TimingOption3;
        customersGuestJourney.Timing = request.In.Timing;
        customersGuestJourney.NotificationDays = request.In.NotificationDays;
        customersGuestJourney.NotificationTime = request.In.NotificationTime;
        customersGuestJourney.GuestJourneyMessagesTemplateId = request.In.GuestJourneyMessagesTemplateId > 0 ? request.In.GuestJourneyMessagesTemplateId : null;
        customersGuestJourney.TempletMessage = request.In.TempletMessage;
        customersGuestJourney.IsActive = true;
        customersGuestJourney.Buttons = jsonButtons == "[]" ? null : jsonButtons;
       
        var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        
        if(vonageCred != null && vonageCred.WABAId != null && vonageCred.AppId != null && vonageCred.AppPrivatKey != null)
        {
            var templateResponse = await _vonageService.CreateTemplate(vonageCred.AppId,uniqueId ,vonageCred.AppPrivatKey, vonageCred.WABAId, request.In,cancellationToken, MessageSenderEnum.Customer.ToString(), stepName, request.In.CustomerId);

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);
            customersGuestJourney.WhatsappTemplateName = templateResponse.TemplateName;

            if (templateResponse.Status == "failed")
            {
                string errorDetails = jsonResponse.detail;
                if (errorDetails.Contains("not a valid URI"))
                {
                    return _response.Error($"Please Enter a Right URI or Select a Right Parameter For URL", AppStatusCodeError.UnprocessableEntity422);
                }
                else if (errorDetails.Contains("not a valid phone number"))
                {
                    return _response.Error($"Please Enter a Right Phone Number or Select a Right Parameter For Phone Number", AppStatusCodeError.UnprocessableEntity422);
                }
                else
                {
                    return _response.Error($"{jsonResponse.detail}", AppStatusCodeError.UnprocessableEntity422);
                }
            }
            else
            {
                customersGuestJourney.VonageTemplateId = jsonResponse.id;
                customersGuestJourney.VonageTemplateStatus = jsonResponse.status;
            }
        }
        else
        {
            customersGuestJourney.VonageTemplateStatus = "Unavailable";
            customersGuestJourney.VonageTemplateId = null;
        }

        await _db.SaveChangesAsync(cancellationToken);

        var createCustomersGuestJourneysOut = new CreatedCustomersGuestJourneysOut
        {
            Id = customersGuestJourney.Id,
            CustomerId = customersGuestJourney.CutomerId,
            JourneyStep = customersGuestJourney.JourneyStep,
            Name = customersGuestJourney.Name,
            WhatsappTemplateName = customersGuestJourney.WhatsappTemplateName,
            SendType = customersGuestJourney.SendType,
            TimingOption1 = customersGuestJourney.TimingOption1,
            TimingOption2 = customersGuestJourney.TimingOption2,
            TimingOption3 = customersGuestJourney.TimingOption3,
            Timing = customersGuestJourney.Timing,
            NotificationDays = customersGuestJourney.NotificationDays,
            NotificationTime = customersGuestJourney.NotificationTime,
            GuestJourneyMessagesTemplateId = customersGuestJourney.GuestJourneyMessagesTemplateId,
            VonageTemplateStatus = customersGuestJourney.VonageTemplateStatus,
            TempletMessage = customersGuestJourney.TempletMessage,
            VonageTemplateId = customersGuestJourney.VonageTemplateId,
            IsActive = customersGuestJourney.IsActive,
            Buttons = JsonConvert.SerializeObject(request.In.Buttons)
        };

        return _response.Success(new CreateCustomersGuestJourneysOut("Create customer guest journey successful.", createCustomersGuestJourneysOut));
    }
}
