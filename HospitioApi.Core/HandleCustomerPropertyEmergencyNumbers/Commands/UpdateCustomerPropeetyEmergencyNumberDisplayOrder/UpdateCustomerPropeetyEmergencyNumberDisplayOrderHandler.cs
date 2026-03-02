using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleProduct.Commands.CreateProduct;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropeetyEmergencyNumberDisplayOrder;

public record UpdateCustomerPropeetyEmergencyNumberDisplayOrderRequest(UpdatedCustomerPropeetyEmergencyNumberDisplayOrderIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerPropeetyEmergencyNumberDisplayOrderHandler : IRequestHandler<UpdateCustomerPropeetyEmergencyNumberDisplayOrderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerPropeetyEmergencyNumberDisplayOrderHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerPropeetyEmergencyNumberDisplayOrderRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UpdateCustomerPropeetyEmergencyNumberDisplayorderIn != null)
        {

            foreach (var item in request.In.UpdateCustomerPropeetyEmergencyNumberDisplayorderIn)
            {
                var customerPropertyEmergencyNumberDisplayOrder = await _db.CustomerPropertyEmergencyNumbers.Where(e => e.Id == item.Id).FirstOrDefaultAsync();
                if (customerPropertyEmergencyNumberDisplayOrder != null)
                {
                    if (customerPropertyEmergencyNumberDisplayOrder.JsonData != null)
                    {
                        var jsonData = JsonConvert.DeserializeObject<UpdateCustomerPropertyEmergencyNumberIn>(customerPropertyEmergencyNumberDisplayOrder.JsonData);
                        jsonData.DisplayOrder = item.DisplayOrder;
                        customerPropertyEmergencyNumberDisplayOrder.JsonData = JsonConvert.SerializeObject(jsonData);
                        //customerPropertyEmergencyNumberDisplayOrder.DisplayOrder = item.DisplayOrder;

                    }
                    else
                    {
                        customerPropertyEmergencyNumberDisplayOrder.DisplayOrder = item.DisplayOrder;
                    }
                }
                

            }
            await _db.SaveChangesAsync(cancellationToken);
            return _response.Success(new UpdateCustomerPropertyEmergencyNumberDisplayOrderOuts($"DisplayOrderUpdatedSuccessfully"));
        }
        return _response.Error($"Customer property emergency number  could not be found.", AppStatusCodeError.Gone410);
    }
}
