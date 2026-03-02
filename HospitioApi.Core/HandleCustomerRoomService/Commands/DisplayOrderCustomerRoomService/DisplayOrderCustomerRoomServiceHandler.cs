using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService
{
    public record DisplayOrderCustomerRoomServiceRequest(DisplayOrderCustomerRoomServiceIn In) : IRequest<AppHandlerResponse>;
    public class DisplayOrderCustomerRoomServiceHandler : IRequestHandler<DisplayOrderCustomerRoomServiceRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        public DisplayOrderCustomerRoomServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response)
        {
            _db = db;
            _response = response;
        }

        public async Task<AppHandlerResponse> Handle(DisplayOrderCustomerRoomServiceRequest request, CancellationToken cancellationToken)
        {
            foreach (var customerRoomService in request.In.DisplayOrderCustomerRoomService)
            {
                var displayOrderCustomerRoomService = await _db.CustomerGuestAppRoomServiceCategories.Where(e => e.Id == customerRoomService.Id).FirstOrDefaultAsync(cancellationToken);
               
                if (displayOrderCustomerRoomService == null)
                {
                    return _response.Error($"Cusomers Room Service with Id {customerRoomService.Id} could not be found.", AppStatusCodeError.Gone410);
                }

                if (displayOrderCustomerRoomService.JsonData != null)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<DisplayOrderCustomerRoomServiceJsonOut>(displayOrderCustomerRoomService.JsonData);

                    result.DisplayOrder = customerRoomService.DisplayOrder;
                    displayOrderCustomerRoomService.JsonData = JsonConvert.SerializeObject(result);
                }
                else
                {
                    displayOrderCustomerRoomService.DisplayOrder = customerRoomService.DisplayOrder;
                }

                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new DisplayOrderCustomerRoomServiceOut("Room-Service display order updated."));
        }
    }

}
