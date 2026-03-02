using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
public record DisplayOrderCustomerConcierageRequest(DisplayOrderCustomerConcierageIn In) : IRequest<AppHandlerResponse>;
public class DisplayOrderCustomerConcierageHandler : IRequestHandler<DisplayOrderCustomerConcierageRequest,AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DisplayOrderCustomerConcierageHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DisplayOrderCustomerConcierageRequest request, CancellationToken cancellationToken)
    {
        foreach (var customerConcierage in request.In.DisplayOrderCustomerConcierage)
        {
            var displayOrderCustomerConcierage = await _db.CustomerGuestAppConciergeCategories.Where(e => e.Id == customerConcierage.Id).FirstOrDefaultAsync(cancellationToken);

            if (displayOrderCustomerConcierage == null)
            {
                return _response.Error($"Cusomers Concierage with Id {customerConcierage.Id} could not be found.", AppStatusCodeError.Gone410);
            }

            if (displayOrderCustomerConcierage.JsonData != null)
            {
                var result = System.Text.Json.JsonSerializer.Deserialize<DisplayOrderCustomerConcierageJsonOut>(displayOrderCustomerConcierage.JsonData);

                result.DisplayOrder = customerConcierage.DisplayOrder;
                displayOrderCustomerConcierage.JsonData = JsonConvert.SerializeObject(result);
            }
            else
            {
                displayOrderCustomerConcierage.DisplayOrder = customerConcierage.DisplayOrder;
            }

            await _db.SaveChangesAsync(cancellationToken);
        }
        return _response.Success(new DisplayOrderCustomerConcierageOut("Concierage display order updated."));
    }
}
