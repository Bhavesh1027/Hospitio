using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using Vonage.Common.Monads;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;

public record DisplayOrderCustomerReceptionRequest(DisplayOrderCustomerReceptionIn In) : IRequest<AppHandlerResponse>;

public class DisplayOrderCustomerReceptionHandler : IRequestHandler<DisplayOrderCustomerReceptionRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DisplayOrderCustomerReceptionHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;   
    }

    public async Task<AppHandlerResponse> Handle(DisplayOrderCustomerReceptionRequest request, CancellationToken cancellationToken)
    {
        foreach (var customerReception in request.In.DisplayOrderCustomerReception)
        {
            var displayOrderCustomerReception = await _db.CustomerGuestAppReceptionCategories.Where(e => e.Id == customerReception.Id && e.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

            if (displayOrderCustomerReception == null)
            {
                return _response.Error($"Cusomers reception with Id {customerReception.Id} could not be found.", AppStatusCodeError.Gone410);
            }

            if (displayOrderCustomerReception.JsonData != null)
            {
                var result = System.Text.Json.JsonSerializer.Deserialize<DisplayOrderCustomerReceptionJsonOut>(displayOrderCustomerReception.JsonData);

                result.DisplayOrder = customerReception.DisplayOrder;
                displayOrderCustomerReception.JsonData = JsonConvert.SerializeObject(result);
            }
            else
            {
                displayOrderCustomerReception.DisplayOrder = customerReception.DisplayOrder;
            }

            await _db.SaveChangesAsync(cancellationToken);

        }
        return _response.Success(new DisplayOrderCustomerReceptionOut("CustomerReception display order updated."));
    }
}
     //return _response.Error($"Customers reception with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);