using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyService;
public record UpdateCustomerPropertyServiceRequest(UpdateCustomerPropertyServiceIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerPropertyServiceHandler : IRequestHandler<UpdateCustomerPropertyServiceRequest,AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomerPropertyServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerPropertyServiceRequest request,CancellationToken cancellationToken)
    {
        var customerPropertyService = await _db.CustomerPropertyServices.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerPropertyService == null)
        {
            return _response.Error($"The customer property service image not found.", AppStatusCodeError.UnprocessableEntity422);
        }
        customerPropertyService.CustomerPropertyInformationId = request.In.CustomerPropertyInformationId;
        customerPropertyService.Name = request.In.Name;
        customerPropertyService.Icon = request.In.Icon;
        customerPropertyService.Description = request.In.Description;
        customerPropertyService.IsActive = request.In.IsActive;

        if (request.In.UPCustomerPropertyServiceImageIns.Any())
        {
            foreach(var item in request.In.UPCustomerPropertyServiceImageIns)
            {
                var serviceImages = await _db.CustomerPropertyServiceImages.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
                if(serviceImages == null)
                {
                    return _response.Error($"The customer property service image not found.", AppStatusCodeError.UnprocessableEntity422);
                }
                serviceImages.ServiceImages = item.ServiceImage;
            }            
        }
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new UpdateCustomerPropertyServiceOut("Customer property service updated successful."));
    }
}
