using MediatR;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropeetyEmergencyNumberDisplayOrder;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.NewFolder;

public record UpdateCustomerPropertyExtraDisplayOrderRequest(UpdatedCustomerPropertyExtraDisplayOrderIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerPropertyExtraDisplayOrderHandler : IRequestHandler<UpdateCustomerPropertyExtraDisplayOrderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerPropertyExtraDisplayOrderHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerPropertyExtraDisplayOrderRequest request, CancellationToken cancellationToken)
    {
        if (request.In.updateCustomerPropertyExtraDisplayOrderIns != null)
        {

            foreach (var item in request.In.updateCustomerPropertyExtraDisplayOrderIns)
            {
                var customerPropertyExtraDisplayOrder = await _db.CustomerPropertyExtras.Where(e => e.Id == item.Id && e.ExtraType == item.ExtraType).FirstOrDefaultAsync();
                if (customerPropertyExtraDisplayOrder != null)
                {


                    if (customerPropertyExtraDisplayOrder.JsonData != null)
                    {
                        var jsonData = JsonConvert.DeserializeObject<CustomerPropertyExtrasIn>(customerPropertyExtraDisplayOrder.JsonData);
                        customerPropertyExtraDisplayOrder.DisplayOrder = item.DisplayOrder;
                        jsonData.DisplayOrder = item.DisplayOrder;
                        customerPropertyExtraDisplayOrder.JsonData = JsonConvert.SerializeObject(jsonData);
                        //customerPropertyExtraDisplayOrder.DisplayOrder = item.DisplayOrder;
                    }
                    else
                    {
                        customerPropertyExtraDisplayOrder.DisplayOrder = item.DisplayOrder;
                    }
                }
            }
            await _db.SaveChangesAsync(cancellationToken);
            return _response.Success(new UpdateCustomerPropertyExtraDisplayOrderOut($"DisplayOrderUpdatedSuccessfully"));

        }
        return _response.Error($"Customer property Display Order  could not be found.", AppStatusCodeError.Gone410);
    }
}

