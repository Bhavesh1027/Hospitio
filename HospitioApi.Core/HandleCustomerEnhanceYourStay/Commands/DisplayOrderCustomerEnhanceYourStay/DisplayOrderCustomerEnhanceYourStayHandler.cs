using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DisplayOrderCustomerEnhanceYourStay
{
    public record DisplayOrderCustomerEnhanceYourStayRequest(DisplayOrderCustomerEnhanceYourStayIn In) : IRequest<AppHandlerResponse>;
    public class DisplayOrderCustomerEnhanceYourStayHandler : IRequestHandler<DisplayOrderCustomerEnhanceYourStayRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;

        public DisplayOrderCustomerEnhanceYourStayHandler(ApplicationDbContext db, IHandlerResponseFactory response)
        {
            _db = db;
            _response = response;
        }

        public async Task<AppHandlerResponse> Handle(DisplayOrderCustomerEnhanceYourStayRequest request, CancellationToken cancellationToken)
        {
            foreach (var CustomerEnhanceYourStay in request.In.displayOrderCustomerEnhanceYourStay)
            {
                var displayOrderCustomerEnhanceYourStay = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(e => e.Id == CustomerEnhanceYourStay.Id).FirstOrDefaultAsync(cancellationToken);

                if (displayOrderCustomerEnhanceYourStay == null)
                {
                    return _response.Error($"Cusomers Enhance Your Stay with Id {CustomerEnhanceYourStay.Id} could not be found.", AppStatusCodeError.Gone410);
                }
                if (displayOrderCustomerEnhanceYourStay.JsonData != null)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<CategoryName>(displayOrderCustomerEnhanceYourStay.JsonData);

                    result.CategoryDisplayOrder = CustomerEnhanceYourStay.DisplayOrder;
                    displayOrderCustomerEnhanceYourStay.JsonData = JsonConvert.SerializeObject(result);
                }
                else
                {
                    displayOrderCustomerEnhanceYourStay.DisplayOrder = CustomerEnhanceYourStay.DisplayOrder;
                }

                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new DisplayOrderCustomerEnhanceYourStayOut("Customer Enhance Your Stay display order updated."));
        }
    }
}
