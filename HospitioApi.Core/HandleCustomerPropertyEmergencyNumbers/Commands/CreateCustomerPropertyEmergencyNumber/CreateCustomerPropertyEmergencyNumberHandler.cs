using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
public record CreateCustomerPropertyEmergencyNumberRequest(CreateCustomerPropertyEmergencyNumberIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerPropertyEmergencyNumberHandler : IRequestHandler<CreateCustomerPropertyEmergencyNumberRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerPropertyEmergencyNumberHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerPropertyEmergencyNumberRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerPropertyEmergencyNumbers.Where(e => e.PhoneNumber == request.In.PhoneNumber && e.CustomerPropertyInformationId == request.In.CustomerPropertyInformationId).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The property emergency number  {request.In.PhoneNumber} already exists.", AppStatusCodeError.Conflict409);
        }
        var customerPropertyEmergencyNumber = new CustomerPropertyEmergencyNumber()
        {
            CustomerPropertyInformationId = request.In.CustomerPropertyInformationId,
            Name = request.In.Name,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            IsActive = request.In.IsActive
        };

        await _db.CustomerPropertyEmergencyNumbers.AddAsync(customerPropertyEmergencyNumber, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateCustomerPropertyEmergencyNumberOut("Create customer property emergency number successful.", new()
        {
            Id = customerPropertyEmergencyNumber.Id,
            CustomerPropertyInformationId = customerPropertyEmergencyNumber.CustomerPropertyInformationId,
            Name = customerPropertyEmergencyNumber.Name,
            PhoneCountry = customerPropertyEmergencyNumber.PhoneCountry,
            PhoneNumber = customerPropertyEmergencyNumber.PhoneNumber,
            IsActive = customerPropertyEmergencyNumber.IsActive
        }));
    }
}
