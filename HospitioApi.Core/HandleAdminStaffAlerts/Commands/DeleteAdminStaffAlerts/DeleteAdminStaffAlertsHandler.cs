using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlerts;
public record DeleteAdminStaffAlertsRequest(DeleteAdminStaffAlertsIn In) : IRequest<AppHandlerResponse>;
public class DeleteAdminStaffAlertsHandler : IRequestHandler<DeleteAdminStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageOptions;

    public DeleteAdminStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService, IOptions<VonageSettingsOptions> vonageOptions)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageOptions = vonageOptions.Value;
    }

    public async Task<AppHandlerResponse> Handle(DeleteAdminStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var adminStaffAlert = await _db.AdminStaffAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (adminStaffAlert == null)
        {
            return _response.Error($"Admin staff alert with {request.In.Id} not found.", AppStatusCodeError.Gone410);
        }
        _db.AdminStaffAlerts.Remove(adminStaffAlert);
        await _db.SaveChangesAsync(cancellationToken);

        string appId = _vonageOptions.AppId;
        string appPrivateKey = _vonageOptions.AppPrivatKey;
        string wabaId = _vonageOptions.WABAId;

        if (appId != null && appPrivateKey != null && wabaId != null && adminStaffAlert.WhatsappTemplateName != null && adminStaffAlert.VonageTemplateId != null)
        {
            await _vonageService.RemoveWhatsappTemplate(appId, appPrivateKey, wabaId, adminStaffAlert.WhatsappTemplateName, adminStaffAlert.VonageTemplateId);
            //adminStaffAlert.VonageTemplateId = null;
            //adminStaffAlert.WhatsappTemplateName = null;
            //adminStaffAlert.VonageTemplateStatus = null;
        }

        return _response.Success(new DeleteAdminStaffAlertsOut("Delete admin staff alert successful.", new() { Id = adminStaffAlert.Id }));
    }
}
