using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;
public record UpdateAdminCustomerAlertsRequest(UpdateAdminCustomerAlertsIn In) : IRequest<AppHandlerResponse>;
public class UpdateAdminCustomerAlertsHandler : IRequestHandler<UpdateAdminCustomerAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateAdminCustomerAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateAdminCustomerAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.AdminCustomerAlerts.Where(e => e.Msg == request.In.Msg && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The admin customer alerts already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var adminCustomerAlerts = await _db.AdminCustomerAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (adminCustomerAlerts == null)
        {
            return _response.Error($"The admin customer alert could not be found.", AppStatusCodeError.Gone410);
        }

        adminCustomerAlerts.Msg = request.In.Msg;
        adminCustomerAlerts.MsgWaitTimeInMinutes = request.In.MsgWaitTimeInMinutes;
        adminCustomerAlerts.IsActive = request.In.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        var updateAdminCustomerAlertsOut = new UpdatedAdminCustomerAlertsOut()
        {
            Id = adminCustomerAlerts.Id,
            Msg = adminCustomerAlerts.Msg,
            MsgWaitTimeInMinutes = adminCustomerAlerts.MsgWaitTimeInMinutes,
            IsActive = adminCustomerAlerts.IsActive
        };

        return _response.Success(new UpdateAdminCustomerAlertsOut("Update admin customer alerts successful.", updateAdminCustomerAlertsOut));
    }
}
