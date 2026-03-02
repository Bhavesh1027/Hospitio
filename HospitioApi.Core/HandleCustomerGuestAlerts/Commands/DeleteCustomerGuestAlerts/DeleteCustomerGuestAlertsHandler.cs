using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlerts;
public record DeleteCustomerGuestAlertsRequest(DeleteCustomerGuestAlertsIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerGuestAlertsHandler : IRequestHandler<DeleteCustomerGuestAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteCustomerGuestAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerGuestAlertsRequest request, CancellationToken cancellationToken)
    {
        var customerGuestAlert = await _db.CustomerGuestAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuestAlert == null)
        {
            return _response.Error($"Customer guest alerts could not be found.", AppStatusCodeError.Gone410);
        }

        _db.CustomerGuestAlerts.Remove(customerGuestAlert);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerGuestAlertsOut("Delete customer guest alerts successful.", new() { Id = request.In.Id }));
    }
}
