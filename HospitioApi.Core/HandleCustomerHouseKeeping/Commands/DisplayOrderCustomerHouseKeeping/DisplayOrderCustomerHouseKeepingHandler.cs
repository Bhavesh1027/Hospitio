using MediatR;
using HospitioApi.Shared;
using System;
using HospitioApi.Core.Services.HandlerResponse;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitioApi.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping
{
    public record DisplayOrderCustomerHouseKeepingRequest(DisplayOrderCustomerHouseKeepingIn In) : IRequest<AppHandlerResponse>;
    public class DisplayOrderCustomerHouseKeepingHandler : IRequestHandler<DisplayOrderCustomerHouseKeepingRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;

        public DisplayOrderCustomerHouseKeepingHandler(ApplicationDbContext db, IHandlerResponseFactory response)
        {
            _db = db;
            _response = response;
        }
        public async Task<AppHandlerResponse> Handle(DisplayOrderCustomerHouseKeepingRequest request, CancellationToken cancellationToken)
        {
            foreach (var CustomerHouseKeepings in request.In.displayOrderCustomerHouseKeepings)
            {
                var displayOrderCustomerHouseKeeping = await _db.CustomerGuestAppHousekeepingCategories.Where(e => e.Id == CustomerHouseKeepings.Id).FirstOrDefaultAsync(cancellationToken);

                if (displayOrderCustomerHouseKeeping == null)
                {
                    return _response.Error($"Cusomers HouseKeeping with Id {CustomerHouseKeepings.Id} could not be found.", AppStatusCodeError.Gone410);
                }
                if (displayOrderCustomerHouseKeeping.JsonData != null)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<DisplayOrderCustomerHouseKeepingJsonOut>(displayOrderCustomerHouseKeeping.JsonData);

                    result.DisplayOrder = CustomerHouseKeepings.DisplayOrder;
                    displayOrderCustomerHouseKeeping.JsonData = JsonConvert.SerializeObject(result);
                }
                else
                {
                    displayOrderCustomerHouseKeeping.DisplayOrder = CustomerHouseKeepings.DisplayOrder;
                }

                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new DisplayOrderCustomerHouseKeepingOut("Customer HouseKeeping display order updated."));
        }

        
    }
}
