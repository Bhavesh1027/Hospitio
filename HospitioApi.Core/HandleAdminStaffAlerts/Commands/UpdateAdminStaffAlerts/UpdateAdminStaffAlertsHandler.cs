using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;
public record UpdateAdminStaffAlertsRequest(UpdateAdminStaffAlertsIn In) : IRequest<AppHandlerResponse>;
public class UpdateAdminStaffAlertsHandler : IRequestHandler<UpdateAdminStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageOptions;

    public UpdateAdminStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService, IOptions<VonageSettingsOptions> vonageOptions)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageOptions = vonageOptions.Value;
    }

    public async Task<AppHandlerResponse> Handle(UpdateAdminStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.AdminStaffAlerts.Where(e => e.Name == request.In.Name && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (checkExist != null)
        {
            return _response.Error($"The admin staff alert {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var adminStaffAlert = await _db.AdminStaffAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (adminStaffAlert == null)
        {
            return _response.Error($"Admin staff alert with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        string whatsappPlatform = ((int)CommunicationPlatFromEnum.Whatsapp).ToString();
        string oldPlatform = adminStaffAlert.Platfrom;
        string newPlatform = request.In.Platfrom;
        string appId = _vonageOptions.AppId;
        string appPrivateKey = _vonageOptions.AppPrivatKey;
        string wabaId = _vonageOptions.WABAId;

        if (oldPlatform == whatsappPlatform && newPlatform != whatsappPlatform)
        {
            if (appId != null && appPrivateKey != null && wabaId != null && adminStaffAlert.WhatsappTemplateName != null && adminStaffAlert.VonageTemplateId != null)
            {
                await _vonageService.RemoveWhatsappTemplate(appId, appPrivateKey, wabaId, adminStaffAlert.WhatsappTemplateName, adminStaffAlert.VonageTemplateId);
                adminStaffAlert.VonageTemplateId = null;
                adminStaffAlert.WhatsappTemplateName = null;
                adminStaffAlert.VonageTemplateStatus = null;
            }
        }
        else if (oldPlatform != whatsappPlatform && newPlatform == whatsappPlatform)
        {
            string stepName = "AdminStaffAlert";
            if (appId != null && appPrivateKey != null && wabaId != null && adminStaffAlert.Id != 0 && adminStaffAlert.Msg != null)
            {
                var newObj = new
                {
                    TempletMessage = adminStaffAlert.Msg,
                    Buttons = new List<Button>()
                };
                var templateResponse = await _vonageService.CreateTemplate(appId, adminStaffAlert.Id, appPrivateKey, wabaId, newObj, cancellationToken, MessageSenderEnum.Hospitio.ToString(), stepName, 0);
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);

                adminStaffAlert.WhatsappTemplateName =  templateResponse.TemplateName;
                if (templateResponse.Status == "failed")
                {
                    string errorDetails = jsonResponse.detail;
                    return _response.Error($"{jsonResponse.detail}", AppStatusCodeError.UnprocessableEntity422);
                }
                else
                {
                    adminStaffAlert.VonageTemplateId = jsonResponse.id;
                    adminStaffAlert.VonageTemplateStatus = jsonResponse.status;
                }
            }

        }
        else if (oldPlatform == whatsappPlatform && newPlatform == whatsappPlatform)
        {
            if (appId != null && appPrivateKey != null && wabaId != null && adminStaffAlert.Id != 0 && adminStaffAlert.VonageTemplateId != null && adminStaffAlert.WhatsappTemplateName != null && adminStaffAlert.Msg != null)
            {
                var newObj = new
                {
                    TempletMessage = adminStaffAlert.Msg,
                    Buttons = new List<Button>()
                };
                var templateResponse = await _vonageService.UpdateTemplate(appId, appPrivateKey, wabaId, adminStaffAlert.VonageTemplateId, newObj, cancellationToken, MessageSenderEnum.Hospitio.ToString(), adminStaffAlert.WhatsappTemplateName, 0);
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);
                if (templateResponse.Status == "failed")
                {
                    string errorDetails = jsonResponse.detail;
                    return _response.Error($"{jsonResponse.detail}", AppStatusCodeError.UnprocessableEntity422);
                }
                else
                {
                    adminStaffAlert.VonageTemplateStatus = templateResponse.TemplateStatus;
                }
            }
        }

        adminStaffAlert.Name = request.In.Name;
        adminStaffAlert.Platfrom = request.In.Platfrom;
        adminStaffAlert.PhoneCountry = request.In.PhoneCountry;
        adminStaffAlert.PhoneNumber = request.In.PhoneNumber;
        adminStaffAlert.WaitTimeInMintes = request.In.WaitTimeInMintes;
        adminStaffAlert.IsActive = request.In.IsActive;
        adminStaffAlert.Msg = request.In.Msg;
        adminStaffAlert.UserId = request.In.UserId;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateAdminStaffAlertsOut("Update admin staff alert successful.", new()
        {
            Id = request.In.Id,
            Name = request.In.Name,
            Platfrom = request.In.Platfrom,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            WaitTimeInMintes = request.In.WaitTimeInMintes,
            IsActive = request.In.IsActive,
            Msg = request.In.Msg,
            UserId = request.In.UserId,
            VonageTemplateStatus = adminStaffAlert.VonageTemplateStatus??null,
            VonageTemplateId = adminStaffAlert.VonageTemplateId??null,
            WhatsappTemplateName = adminStaffAlert.WhatsappTemplateName??null
        }));
    }
}
