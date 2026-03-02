using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlerts;
public record UpdateCustomerStaffAlertsRequest(UpdateCustomerStaffAlertsIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerStaffAlertsHandler : IRequestHandler<UpdateCustomerStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerStaffAlerts.Where(e => e.Name == request.In.Name && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer staff alert {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var customerStaffAlert = await _db.CustomerStaffAlerts.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (customerStaffAlert == null)
        {
            return _response.Error($"Customer staff alert with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        customerStaffAlert.CustomerId = Convert.ToInt32(request.CustomerId);
        customerStaffAlert.Name = request.In.Name;
        customerStaffAlert.Platfrom = request.In.Platfrom;
        customerStaffAlert.PhoneCountry = request.In.PhoneCountry;
        customerStaffAlert.PhoneNumber = request.In.PhoneNumber;
        customerStaffAlert.WaitTimeInMintes = request.In.WaitTimeInMintes;
        customerStaffAlert.IsActive = request.In.IsActive;
        customerStaffAlert.Msg = request.In.Msg;
        customerStaffAlert.CustomerUserId = request.In.CustomerUserId;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomerStaffAlertsOut("Update customer staff alert successful.", new()
        {
            Id = request.In.Id,
            CustomerId = request.In.Id,
            Name = request.In.Name,
            Platfrom = request.In.Platfrom,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            WaitTimeInMintes = request.In.WaitTimeInMintes,
            IsActive = request.In.IsActive,
            Msg = request.In.Msg,
            CustomerUserId = request.In.CustomerUserId,
        }));
    }
}
