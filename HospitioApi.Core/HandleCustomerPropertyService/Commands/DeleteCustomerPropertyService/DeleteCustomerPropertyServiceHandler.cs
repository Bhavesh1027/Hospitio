using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;
public record DeleteCustomerPropertyServiceRequest(DeleteCustomerPropertyServiceIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerPropertyServiceHandler : IRequestHandler<DeleteCustomerPropertyServiceRequest,AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteCustomerPropertyServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteCustomerPropertyServiceRequest request, CancellationToken cancellationToken)
    {
        var propertyService = await _db.CustomerPropertyServices.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (propertyService == null)
        {
            return _response.Error($"The customer property service id not found.", AppStatusCodeError.UnprocessableEntity422);
        }
        if(propertyService.JsonData != null)
        {
            var propertServiceJsonData = JsonConvert.DeserializeObject<CustomerPropertyServiceJsonOut>(propertyService.JsonData);
            propertServiceJsonData.IsDeleted = true;
            propertyService.JsonData = JsonConvert.SerializeObject(propertServiceJsonData); 
        }
        else
        {
            var serviceImage = await _db.CustomerPropertyServiceImages.Where(e => e.CustomerPropertyServiceId == request.In.Id).ToListAsync(cancellationToken);
            _db.CustomerPropertyServiceImages.RemoveRange(serviceImage);
            _db.CustomerPropertyServices.Remove(propertyService);
        }
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerPropertyServiceOut("Delete property service successful"));
    }
}
