using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlerts;
public record DeleteCustomerStaffAlertsRequest(DeleteCustomerStaffAlertsIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerStaffAlertsHandler : IRequestHandler<DeleteCustomerStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var customerStaffAlert = await _db.CustomerStaffAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerStaffAlert == null)
        {
            return _response.Error($"Customer staff alert with {request.In.Id} not found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerStaffAlerts.Remove(customerStaffAlert);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerStaffAlertsOut("Delete customer staff alert successful.", new() { Id = customerStaffAlert.Id }));
    }
}
