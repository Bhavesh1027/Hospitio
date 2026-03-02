using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;
public record DeleteCustomerPropertyEmergencyNumberRequest(DeleteCustomerPropertyEmergencyNumberIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerPropertyEmergencyNumberHandler : IRequestHandler<DeleteCustomerPropertyEmergencyNumberRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerPropertyEmergencyNumberHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerPropertyEmergencyNumberRequest request, CancellationToken cancellationToken)
    {
        var customerPropertyEmergencyNumber = await _db.CustomerPropertyEmergencyNumbers.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (customerPropertyEmergencyNumber == null)
        {
            return _response.Error($"Customer property emergency number with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        if (customerPropertyEmergencyNumber.JsonData != null)
        {
           var PropertyEmergencyNumberJsonOut =  JsonConvert.DeserializeObject<CustomerPropertyEmergencyNumberJsonOut>(customerPropertyEmergencyNumber.JsonData);
            PropertyEmergencyNumberJsonOut.IsDeleted = true;
            customerPropertyEmergencyNumber.JsonData = JsonConvert.SerializeObject(PropertyEmergencyNumberJsonOut);
        }
        else
        {
            _db.CustomerPropertyEmergencyNumbers.Remove(customerPropertyEmergencyNumber);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerPropertyEmergencyNumberOut("Delete customer property emergency number successful.", new()
        {
            Id = request.In.Id
        }));

    }
}
