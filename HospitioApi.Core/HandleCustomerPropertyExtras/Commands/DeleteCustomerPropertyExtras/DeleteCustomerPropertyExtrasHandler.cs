using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtras;
public record DeleteCustomerPropertyExtrasRequest(DeleteCustomerPropertyExtrasIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerPropertyExtrasHandler : IRequestHandler<DeleteCustomerPropertyExtrasRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerPropertyExtrasHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteCustomerPropertyExtrasRequest request, CancellationToken cancellationToken)
    {
        var customerPropertyExtra = await _db.CustomerPropertyExtras.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerPropertyExtra == null)
        {
            return _response.Error($"Customer property extra could not be found.", AppStatusCodeError.Gone410);
        }
        if(customerPropertyExtra.JsonData != null)
        {
            var CustomerPropertyExtrasJsonOut = JsonConvert.DeserializeObject<CustomerPropertyExtrasJsonOut>(customerPropertyExtra.JsonData);
            CustomerPropertyExtrasJsonOut.IsDeleted = true;
            customerPropertyExtra.JsonData = JsonConvert.SerializeObject(CustomerPropertyExtrasJsonOut);
        }
        else
        {
            var secondaryEntities = await _db.CustomerPropertyExtraDetails.Where(s => s.CustomerPropertyExtraId == request.In.Id).ToListAsync(cancellationToken);
            if (secondaryEntities != null)
            {
                foreach (var secondaryEntity in secondaryEntities)
                {
                    _db.CustomerPropertyExtraDetails.Remove(secondaryEntity);
                }

            }

            _db.CustomerPropertyExtras.Remove(customerPropertyExtra);
        }
       
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerPropertyExtrasOut("Delete customer property extra successful.", new()
        {
            Id = request.In.Id
        }));
    }
}
