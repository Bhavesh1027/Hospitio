using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;
public record DeleteCustomerPropertyExtraDetailRequest(DeleteCustomerPropertyExtraDetailIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerPropertyExtraDetailHandler : IRequestHandler<DeleteCustomerPropertyExtraDetailRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerPropertyExtraDetailHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteCustomerPropertyExtraDetailRequest request, CancellationToken cancellationToken)
    {
        var customerPropertyExtra = await _db.CustomerPropertyExtraDetails.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerPropertyExtra == null)
        {
            return _response.Error($"Customer property extra detail could not be found.", AppStatusCodeError.Gone410);
        }
        if(customerPropertyExtra.JsonData != null) 
        {
            var CustomerPropertyExtraDetailJsonOut =  JsonConvert.DeserializeObject<CustomerPropertyExtraDetailJsonOut>(customerPropertyExtra.JsonData);
            CustomerPropertyExtraDetailJsonOut.IsDeleted = true;
            customerPropertyExtra.JsonData = JsonConvert.SerializeObject(CustomerPropertyExtraDetailJsonOut);
        }
        else
        {
            _db.CustomerPropertyExtraDetails.Remove(customerPropertyExtra);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerPropertyExtraDetailOut("Delete customer property extra detail successful.", new()
        {
            Id = request.In.Id
        }));
    }
}
