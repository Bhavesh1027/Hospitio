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

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplates;
public record UpdateGuestJourneyMessagesTemplatesRequest(UpdateGuestJourneyMessagesTemplatesIn In) : IRequest<AppHandlerResponse>;
public class UpdateGuestJourneyMessagesTemplatesHandler : IRequestHandler<UpdateGuestJourneyMessagesTemplatesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageSettings;
    public UpdateGuestJourneyMessagesTemplatesHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService, IOptions<VonageSettingsOptions> vonageSettings)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageSettings = vonageSettings.Value;
    }
    public async Task<AppHandlerResponse> Handle(UpdateGuestJourneyMessagesTemplatesRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.GuestJourneyMessagesTemplates.Where(e => e.Name == request.In.Name && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The GuestJourneyMessagesTemplates {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}
        var guestJourneyMessagesTemplates = await _db.GuestJourneyMessagesTemplates.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (guestJourneyMessagesTemplates == null)
        {
            return _response.Error($"GuestJourneyMessagesTemplates with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        guestJourneyMessagesTemplates.Buttons = JsonConvert.SerializeObject(request.In.Buttons) == "[]" ? null : JsonConvert.SerializeObject(request.In.Buttons);

        if(guestJourneyMessagesTemplates.VonageTemplateId != null && guestJourneyMessagesTemplates.WhatsappTemplateName != null)
        {
            var templateResponse = await _vonageService.UpdateTemplate(_vonageSettings.AppId, _vonageSettings.AppPrivatKey, _vonageSettings.WABAId, guestJourneyMessagesTemplates.VonageTemplateId, request.In, cancellationToken, MessageSenderEnum.Hospitio.ToString(), guestJourneyMessagesTemplates.WhatsappTemplateName, request.In.UserId);

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);
            guestJourneyMessagesTemplates.VonageTemplateStatus = templateResponse.TemplateStatus;
            
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
                guestJourneyMessagesTemplates.VonageTemplateStatus = templateResponse.TemplateStatus;
                guestJourneyMessagesTemplates.Name = request.In.Name;
                guestJourneyMessagesTemplates.TempleteType = request.In.TempleteType;
                guestJourneyMessagesTemplates.TempletMessage = request.In.TempletMessage;
                guestJourneyMessagesTemplates.IsActive = request.In.IsActive;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        else
        {
            var jsonButtons = JsonConvert.SerializeObject(request.In.Buttons);
            guestJourneyMessagesTemplates.Name = request.In.Name;
            guestJourneyMessagesTemplates.TempleteType = request.In.TempleteType;
            guestJourneyMessagesTemplates.TempletMessage = request.In.TempletMessage;
            guestJourneyMessagesTemplates.IsActive = request.In.IsActive;
            guestJourneyMessagesTemplates.Buttons = jsonButtons == "[]" ? null : jsonButtons;
            await _db.SaveChangesAsync(cancellationToken);
        }
        var guestJourneyMessagesTemplatesOut = new UpdatedGuestJourneyMessagesTemplatesOut()
        {
            Id = guestJourneyMessagesTemplates.Id,
            Name = guestJourneyMessagesTemplates.Name,
            WhatsappTemplateName = guestJourneyMessagesTemplates?.WhatsappTemplateName,
            TempleteType = guestJourneyMessagesTemplates.TempleteType,
            TempletMessage = guestJourneyMessagesTemplates.TempletMessage,
            VonageTemplateId = guestJourneyMessagesTemplates.VonageTemplateId,
            VonageTemplateStatus = guestJourneyMessagesTemplates.VonageTemplateStatus,
            IsActive = guestJourneyMessagesTemplates.IsActive,
            Buttons = JsonConvert.SerializeObject(request.In.Buttons)
        };

        return _response.Success(new UpdateGuestJourneyMessagesTemplatesOut("Update GuestJourneyMessagesTemplates successful.", guestJourneyMessagesTemplatesOut));
    }
}
