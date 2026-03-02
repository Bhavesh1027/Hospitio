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

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.CreateAdminStaffAlerts;
public record CreateAdminStaffAlertsRequest(CreateAdminStaffAlertsIn In) : IRequest<AppHandlerResponse>;
public class CreateAdminStaffAlertsHandler : IRequestHandler<CreateAdminStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageOptions;

    public CreateAdminStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService, IOptions<VonageSettingsOptions> vonageOptions)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageOptions = vonageOptions.Value;
    }

    public async Task<AppHandlerResponse> Handle(CreateAdminStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.AdminStaffAlerts.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The admin staff alert {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var checkTotalAlert = await _db.AdminStaffAlerts.Select(e => new AdminStaffAlert()
        {
            Id = e.Id
        }).CountAsync();
        if (checkTotalAlert > 4)
        {
            return _response.Error("Not created,only 5 staff alert add.", AppStatusCodeError.UnprocessableEntity422);
        }

        var adminStaffAlert = new AdminStaffAlert
        {
            Name = request.In.Name,
            Platfrom = request.In.Platfrom,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            WaitTimeInMintes = request.In.WaitTimeInMintes,
            IsActive = request.In.IsActive,
            Msg = request.In.Msg,
            UserId = request.In.UserId,
        };

        await _db.AdminStaffAlerts.AddAsync(adminStaffAlert, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        if (request.In.Platfrom == ((int)CommunicationPlatFromEnum.Whatsapp).ToString())
        {
            var newObj = new
            {
                TempletMessage = adminStaffAlert.Msg,
                Buttons = new List<Button>()
            };
            string stepName = "AdminStaffAlert";
            var templateResponse = await _vonageService.CreateTemplate(_vonageOptions.AppId, adminStaffAlert.Id, _vonageOptions.AppPrivatKey, _vonageOptions.WABAId, newObj, cancellationToken, MessageSenderEnum.Hospitio.ToString(), stepName, 0);

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);
            adminStaffAlert.WhatsappTemplateName = templateResponse.TemplateName;
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
            _db.Update(adminStaffAlert);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return _response.Success(new CreateAdminStaffAlertsOut("Create admin staff alert successful.", new()
        {
            Id = adminStaffAlert.Id,
            Name = adminStaffAlert.Name,
            Platfrom = adminStaffAlert.Platfrom,
            PhoneNumber = adminStaffAlert.PhoneNumber,
            PhoneCountry = adminStaffAlert.PhoneCountry,
            WaitTimeInMintes = adminStaffAlert.WaitTimeInMintes,
            IsActive = adminStaffAlert.IsActive,
            Msg = adminStaffAlert.Msg,
            UserId = adminStaffAlert.UserId,
            WhatsappTemplateName = adminStaffAlert.WhatsappTemplateName??null,
            VonageTemplateId = adminStaffAlert.VonageTemplateId??null,
            VonageTemplateStatus = adminStaffAlert.VonageTemplateStatus??null,
        }));

    }
}
