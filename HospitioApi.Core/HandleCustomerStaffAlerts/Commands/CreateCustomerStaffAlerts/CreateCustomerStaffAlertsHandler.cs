using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlerts;
public record CreateCustomerStaffAlertsRequest(CreateCustomerStaffAlertsIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class CreateCustomerStaffAlertsHandler : IRequestHandler<CreateCustomerStaffAlertsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerStaffAlertsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerStaffAlerts.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer staff alert {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var checkTotalAlert = await _db.CustomerStaffAlerts.Select(e => new CustomerStaffAlert()
        {
            Id = e.Id
        }).CountAsync();
        if (checkTotalAlert > 4)
        {
            return _response.Error("Not created,only 5 staff alert add.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customerStaffAlert = new CustomerStaffAlert
        {
            CustomerId = Convert.ToInt32(request.CustomerId),
            Name = request.In.Name,
            Platfrom = request.In.Platfrom,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            WaitTimeInMintes = request.In.WaitTimeInMintes,
            IsActive = request.In.IsActive,
            Msg = request.In.Msg,
            CustomerUserId = request.In.CustomerUserId,
        };

        await _db.CustomerStaffAlerts.AddAsync(customerStaffAlert, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateCustomerStaffAlertsOut("Create customer staff alert successful.", new()
        {
            Id = customerStaffAlert.Id,
            Name = customerStaffAlert.Name,
            Platfrom = customerStaffAlert.Platfrom,
            PhoneCountry = customerStaffAlert.PhoneCountry,
            PhoneNumber = customerStaffAlert.PhoneNumber,
            WaitTimeInMintes = customerStaffAlert.WaitTimeInMintes,
            IsActive = customerStaffAlert.IsActive,
            Msg = customerStaffAlert.Msg,
            CustomerUserId = customerStaffAlert.CustomerUserId,
        }));

    }
}
