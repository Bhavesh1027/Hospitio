using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;
public record UpdateCustomersGuestJourneysRequest(UpdateCustomersGuestJourneysIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomersGuestJourneysHandler : IRequestHandler<UpdateCustomersGuestJourneysRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageSettings;
    public UpdateCustomersGuestJourneysHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService, IOptions<VonageSettingsOptions> vonageSettings)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageSettings = vonageSettings.Value;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomersGuestJourneysRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.CustomerGuestJournies.Where(e => e.JourneyStep == request.In.JourneyStep && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The guest journey Step {request.In.JourneyStep} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}
        string stepName = ((JourneyStepTempleteTypeEnums)request.In.JourneyStep).ToString();
        var updateCustomersGuestJourneys = await _db.CustomerGuestJournies.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (updateCustomersGuestJourneys == null)
        {
            return _response.Error($"Customers guest journey with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        int CustomerId = updateCustomersGuestJourneys.CutomerId;
        var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId ==  CustomerId).FirstOrDefaultAsync(cancellationToken);

        //if customer have not whatsapp-bussiness account or not connected with vonage , so that time whatsapp template store in DB but not send for approval
        if(vonageCred.AppId != null && vonageCred.AppPrivatKey != null && vonageCred.WABAId != null)
        {
            //if customer have bussienss account but not before template go for aproval so this time create and send for approval
            if(updateCustomersGuestJourneys.VonageTemplateId == null)
            {
                var templateResponse = await _vonageService.CreateTemplate(vonageCred.AppId, updateCustomersGuestJourneys.Id, vonageCred.AppPrivatKey, vonageCred.WABAId, request.In, cancellationToken, MessageSenderEnum.Customer.ToString(), stepName, CustomerId);

                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);
                updateCustomersGuestJourneys.WhatsappTemplateName = templateResponse.TemplateName;
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
                    updateCustomersGuestJourneys.VonageTemplateId = jsonResponse.id;
                    updateCustomersGuestJourneys.VonageTemplateStatus = jsonResponse.status;
                }

            }
            else
            {
                if (updateCustomersGuestJourneys.WhatsappTemplateName != null || updateCustomersGuestJourneys.VonageTemplateId != null)
                {
                    var templateResponse = await _vonageService.UpdateTemplate(vonageCred.AppId, vonageCred.AppPrivatKey, vonageCred.WABAId, updateCustomersGuestJourneys.VonageTemplateId, request.In, cancellationToken, MessageSenderEnum.Customer.ToString(), updateCustomersGuestJourneys.WhatsappTemplateName, CustomerId);

                    updateCustomersGuestJourneys.VonageTemplateStatus = templateResponse.TemplateStatus;
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);

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
                        updateCustomersGuestJourneys.VonageTemplateStatus = templateResponse.TemplateStatus;
                        updateCustomersGuestJourneys.JourneyStep = request.In.JourneyStep;
                        updateCustomersGuestJourneys.Name = templateResponse.TemplateName;
                        updateCustomersGuestJourneys.SendType = request.In.SendType;
                        updateCustomersGuestJourneys.TimingOption1 = request.In.TimingOption1;
                        updateCustomersGuestJourneys.TimingOption2 = request.In.TimingOption2;
                        updateCustomersGuestJourneys.TimingOption3 = request.In.TimingOption3;
                        updateCustomersGuestJourneys.Timing = request.In.Timing;
                        updateCustomersGuestJourneys.NotificationDays = request.In.NotificationDays;
                        updateCustomersGuestJourneys.NotificationTime = request.In.NotificationTime;
                        updateCustomersGuestJourneys.GuestJourneyMessagesTemplateId = request.In.GuestJourneyMessagesTemplateId > 0 ? request.In.GuestJourneyMessagesTemplateId : null;
                        updateCustomersGuestJourneys.TempletMessage = request.In.TempletMessage;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    return _response.Error($"This Whatsapp Template is Not Exist , so choose right Template", AppStatusCodeError.UnprocessableEntity422);
                }
            }
        }
        else
        {
            var jsonButtons = JsonConvert.SerializeObject(request.In.Buttons);
            updateCustomersGuestJourneys.JourneyStep = request.In.JourneyStep;
            updateCustomersGuestJourneys.Name = request.In.Name;
            updateCustomersGuestJourneys.SendType = request.In.SendType;
            updateCustomersGuestJourneys.TimingOption1 = request.In.TimingOption1;
            updateCustomersGuestJourneys.TimingOption2 = request.In.TimingOption2;
            updateCustomersGuestJourneys.TimingOption3 = request.In.TimingOption3;
            updateCustomersGuestJourneys.Timing = request.In.Timing;
            updateCustomersGuestJourneys.NotificationDays = request.In.NotificationDays;
            updateCustomersGuestJourneys.NotificationTime = request.In.NotificationTime;
            updateCustomersGuestJourneys.GuestJourneyMessagesTemplateId = request.In.GuestJourneyMessagesTemplateId > 0 ? request.In.GuestJourneyMessagesTemplateId : null;
            updateCustomersGuestJourneys.TempletMessage = request.In.TempletMessage;
            updateCustomersGuestJourneys.Buttons = jsonButtons == "[]" ? null : jsonButtons;
            await _db.SaveChangesAsync(cancellationToken);
        }


        return _response.Success(new UpdateCustomersGuestJourneysOut("Update customer guest journey successful", new()
        {
            Id = request.In.Id,
            JourneyStep = request.In.JourneyStep,
            Name = request.In.Name,
            WhatsappTemplateName = updateCustomersGuestJourneys.WhatsappTemplateName,
            SendType = request.In.SendType,
            TimingOption1 = request.In.TimingOption1,
            TimingOption2 = request.In.TimingOption2,
            TimingOption3 = request.In.TimingOption3,
            Timing = request.In.Timing,
            NotificationDays = request.In.NotificationDays,
            NotificationTime = request.In.NotificationTime,
            GuestJourneyMessagesTemplateId = request.In.GuestJourneyMessagesTemplateId,
            TempletMessage = request.In.TempletMessage,
            VonageTemplateId = updateCustomersGuestJourneys.VonageTemplateId,
            VonageTemplateStatus = updateCustomersGuestJourneys.VonageTemplateStatus,
            Buttons = JsonConvert.SerializeObject(request.In.Buttons)
        }));
    }
}
