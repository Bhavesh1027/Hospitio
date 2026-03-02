using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;
public record UpdateCustomerGuestAlertsRequest(UpdateCustomerGuestAlertsIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerGuestAlertsHandler : IRequestHandler<UpdateCustomerGuestAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomerGuestAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerGuestAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerGuestAlerts.Where(e => e.OfficeHoursMsg == request.In.OfficeHoursMsg && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer guest alerts already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customerGuestAlerts = await _db.CustomerGuestAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuestAlerts == null)
        {
            return _response.Error($"Ticket customer guest alert could not be found.", AppStatusCodeError.Gone410);
        }

        customerGuestAlerts.CustomerId = Convert.ToInt32(request.CustomerId);
        customerGuestAlerts.OfficeHoursMsg = request.In.OfficeHoursMsg;
        customerGuestAlerts.OfflineHourMsg = request.In.OfflineHourMsg;
        customerGuestAlerts.OfflineHoursMsgWaitTimeInMinutes = request.In.OfflineHoursMsgWaitTimeInMinutes;
        customerGuestAlerts.OfficeHoursMsgWaitTimeInMinutes = request.In.OfficeHoursMsgWaitTimeInMinutes;
        customerGuestAlerts.ReplyAtDiffPeriod = request.In.ReplyAtDiffPeriod;
        customerGuestAlerts.IsActive = request.In.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        var updateCustomerGuestAlertsOut = new UpdatedCustomerGuestAlertsOut()
        {
            Id = customerGuestAlerts.Id,
            CustomerId = customerGuestAlerts.CustomerId,
            OfficeHoursMsg = customerGuestAlerts.OfficeHoursMsg,
            OfflineHourMsg = customerGuestAlerts.OfflineHourMsg,
            OfficeHoursMsgWaitTimeInMinutes = customerGuestAlerts.OfficeHoursMsgWaitTimeInMinutes,
            OfflineHoursMsgWaitTimeInMinutes = customerGuestAlerts.OfflineHoursMsgWaitTimeInMinutes,
            ReplyAtDiffPeriod = customerGuestAlerts.ReplyAtDiffPeriod,
            IsActive = customerGuestAlerts.IsActive
        };

        return _response.Success(new UpdateCustomerGuestAlertsOut("Update customer guest alerts successful.", updateCustomerGuestAlertsOut));
    }
}
