using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;
public record CreateAdminCustomerAlertsRequest(CreateAdminCustomerAlertsIn In) : IRequest<AppHandlerResponse>;
public class CreateAdminCustomerAlertsHandler : IRequestHandler<CreateAdminCustomerAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateAdminCustomerAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateAdminCustomerAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.AdminCustomerAlerts.Where(e => e.Msg == request.In.Msg).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The admin customer alerts already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var adminCustomerAlert = new AdminCustomerAlert
        {
            Msg = request.In.Msg,
            MsgWaitTimeInMinutes = request.In.MsgWaitTimeInMinutes,
            IsActive = request.In.IsActive
        };

        var checkTotalAlert = await _db.AdminCustomerAlerts.Select(e => new AdminCustomerAlert()
        {
            Id = e.Id
        }).CountAsync();

        if (checkTotalAlert > 0)
        {
            return _response.Error("Not created,only 1 customer alert Add.", AppStatusCodeError.UnprocessableEntity422);
        }
        await _db.AdminCustomerAlerts.AddAsync(adminCustomerAlert, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var createdAdminCustomerAlertsOut = new CreatedAdminCustomerAlertsOut
        {
            Id = adminCustomerAlert.Id,
            Msg = adminCustomerAlert.Msg,
            MsgWaitTimeInMinutes = adminCustomerAlert.MsgWaitTimeInMinutes,
            IsActive = adminCustomerAlert.IsActive
        };

        return _response.Success(new CreateAdminCustomerAlertsOut("Create admin customer alert successful.", createdAdminCustomerAlertsOut));
    }
}
