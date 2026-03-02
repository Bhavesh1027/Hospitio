using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;
public record DeleteAdminCustomerAlertsRequest(DeleteAdminCustomerAlertsIn In) : IRequest<AppHandlerResponse>;
public class DeleteAdminCustomerAlertsHandler : IRequestHandler<DeleteAdminCustomerAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteAdminCustomerAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteAdminCustomerAlertsRequest request, CancellationToken cancellationToken)
    {
        var adminCustomerAlert = await _db.AdminCustomerAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (adminCustomerAlert == null)
        {
            return _response.Error($"Admin customer alerts could not be found.", AppStatusCodeError.Gone410);
        }

        _db.AdminCustomerAlerts.Remove(adminCustomerAlert);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteAdminCustomerAlertsOut("Delete admin customer alerts successful.", new() { Id = request.In.Id }));
    }
}
