using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.CreateCustomerGuestAlerts;
public record CreateCustomerGuestAlertsRequest(CreateCustomerGuestAlertsIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class CreateCustomerGuestAlertsHandler : IRequestHandler<CreateCustomerGuestAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateCustomerGuestAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerGuestAlerts.Where(e => e.OfficeHoursMsg == request.In.OfficeHoursMsg && e.OfflineHourMsg == request.In.OfflineHourMsg).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer guest alerts already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var customerGuestAlert = new CustomerGuestAlert
        {
            CustomerId = Convert.ToInt32(request.CustomerId),
            OfficeHoursMsg = request.In.OfficeHoursMsg,
            OfficeHoursMsgWaitTimeInMinutes = request.In.OfficeHoursMsgWaitTimeInMinutes,
            OfflineHourMsg = request.In.OfficeHoursMsg,
            OfflineHoursMsgWaitTimeInMinutes = request.In.OfflineHoursMsgWaitTimeInMinutes,
            ReplyAtDiffPeriod = request.In.ReplyAtDiffPeriod,
            IsActive = request.In.IsActive
        };

        var checkTotalAlert = await _db.CustomerGuestAlerts.Where(c => c.CustomerId == Convert.ToInt32(request.CustomerId)).Select(e => new CustomerGuestAlert()
        {
            Id = e.Id
        }).CountAsync();

        if (checkTotalAlert > 0)
        {
            return _response.Error("Not created,only 1 guest alert Add.", AppStatusCodeError.UnprocessableEntity422);
        }
        await _db.CustomerGuestAlerts.AddAsync(customerGuestAlert, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var createdCustomerGuestAlertsOut = new CreatedCustomerGuestAlertsOut
        {
            Id = customerGuestAlert.Id,
            OfficeHoursMsg = customerGuestAlert.OfficeHoursMsg,
            OfficeHoursMsgWaitTimeInMinutes = customerGuestAlert.OfficeHoursMsgWaitTimeInMinutes,
            OfflineHourMsg = customerGuestAlert.OfficeHoursMsg,
            OfflineHoursMsgWaitTimeInMinutes = customerGuestAlert.OfflineHoursMsgWaitTimeInMinutes,
            ReplyAtDiffPeriod = customerGuestAlert.ReplyAtDiffPeriod,
            IsActive = customerGuestAlert.IsActive
        };

        return _response.Success(new CreateCustomerGuestAlertsOut("Create customer guest alert successful.", createdCustomerGuestAlertsOut));
    }
}
