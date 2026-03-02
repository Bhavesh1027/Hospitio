using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;
using System.Text.Json;

namespace HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;
public record MusementCreateOrderRequest(MusementCreateOrderIn In) : IRequest<AppHandlerResponse>;
public class MusementCreateOrderHandler : IRequestHandler<MusementCreateOrderRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MusementSettingsOptions _musementSettings;
    private readonly ApplicationDbContext _db;
    public MusementCreateOrderHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response,
        IHttpClientFactory httpClientFactory,
        IOptions<MusementSettingsOptions> musementSettings,
        ApplicationDbContext db
        )
    {
        _dapper = dapper;
        _response = response;
        _httpClientFactory = httpClientFactory;
        _musementSettings = musementSettings.Value;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(MusementCreateOrderRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
        client.DefaultRequestHeaders.Add("X-Musement-Application", "string");
        client.DefaultRequestHeaders.Add("X-Musement-Currency", "USD");
        client.DefaultRequestHeaders.Add("X-Musement-Version", "3.4.0");

        var mesumentRequest =  new HttpResponseMessage();

        string extraData = System.Text.Json.JsonSerializer.Serialize(request.In.Extra_data);

        if (request.In.Order_uuid != null)
        {

            JObject json = JObject.Parse(@"{
            extra_data : ''
            }");

            json["extra_data"] = extraData.ToString();

            var postData = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            mesumentRequest = await client.PatchAsync(_musementSettings.musement_url + request.In.Url, postData);
        }
        else
        {
            JObject json = JObject.Parse(@"{
            cart_uuid : '' ,
            email_notification : '',
            extra_data : ''
            }");

            json["cart_uuid"] = request.In.Cart_uuid;
            json["email_notification"] = request.In.Email_notification;
            json["extra_data"] = extraData.ToString();

            var postData = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            mesumentRequest = await client.PostAsync(_musementSettings.musement_url + request.In.Url, postData);
        }

        if (mesumentRequest.IsSuccessStatusCode)
        {
            var response = await mesumentRequest.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<order>(response);

            if (tokenResponse != null)
            {
                // Insert/Update MusementCustomer Info
                var exitsMusementCustomerData = await _db.MusementGuestInfos.Where(s => s.CartUUID == request.In.Cart_uuid && s.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

                if (exitsMusementCustomerData == null)
                {
                    var musementGuestInfos = new MusementGuestInfo
                    {
                        FirstName = tokenResponse.customer.firstname,
                        LastName = tokenResponse.customer.lastname,
                        Email = tokenResponse.customer.email,
                        MusementCustomerId = tokenResponse.customer.id,
                        CustomerGuestId = int.Parse(request.In.Guest_Id),
                        CartUUID = request.In.Cart_uuid
                    };
                    await _db.MusementGuestInfos.AddAsync(musementGuestInfos);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    exitsMusementCustomerData.FirstName = tokenResponse.customer.firstname;
                    exitsMusementCustomerData.LastName = tokenResponse.customer.lastname;
                    exitsMusementCustomerData.Email = tokenResponse.customer.email;
                    exitsMusementCustomerData.MusementCustomerId = tokenResponse.customer.id;
                    exitsMusementCustomerData.CustomerGuestId = int.Parse(request.In.Guest_Id);
                }

                // Insert / Update Musement Order Info

                if(request.In.Order_uuid == null)
                {
                    var GetOrderData = await _db.MusementOrderInfos.Where(s => s.CartUUID == request.In.Cart_uuid).ToListAsync();

                    if(GetOrderData.Count !=0 ||GetOrderData != null)
                    {
                        _db.RemoveRange(GetOrderData);
                    }
                }

                var MusementCustomerId = await _db.MusementGuestInfos.Where(s => s.CartUUID == request.In.Cart_uuid && s.DeletedAt == null).Select(s => s.Id).FirstOrDefaultAsync(cancellationToken);
                var exitsMusementOrderData = await _db.MusementOrderInfos.Where(s => s.OrderUUID == tokenResponse.uuid && s.DeletedAt == null).Include(s => s.MusementGuestInfo).FirstOrDefaultAsync(cancellationToken);
                if (exitsMusementOrderData == null)
                {
                    var musementOrderInfo = new MusementOrderInfo
                    {
                        Identifier = tokenResponse.identifier,
                        OrderUUID = tokenResponse.uuid,
                        MusementGuestInfoId = MusementCustomerId,
                        Currency = tokenResponse.total_price.currency,
                        TotalPrice = tokenResponse.total_price.value,
                        DiscountAmount = tokenResponse.discount_amount.value,
                        PaymentJson = JsonConvert.SerializeObject(tokenResponse),
                        CartUUID = request.In.Cart_uuid
                    };
 
                    await _db.MusementOrderInfos.AddAsync(musementOrderInfo);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    exitsMusementOrderData.Identifier = tokenResponse.identifier;
                    exitsMusementOrderData.OrderUUID = tokenResponse.uuid;
                    exitsMusementOrderData.Currency = tokenResponse.total_price.currency;
                    exitsMusementOrderData.TotalPrice = tokenResponse.total_price.value;
                    exitsMusementOrderData.DiscountAmount = tokenResponse.discount_amount.value;
                    exitsMusementOrderData.PaymentJson = JsonConvert.SerializeObject(tokenResponse);
                }


                // Insert / Update Musement Item Info
                var musementOrderId = await _db.MusementOrderInfos.Where(s => s.OrderUUID == tokenResponse.uuid && s.DeletedAt == null).Select(s => s.Id).FirstOrDefaultAsync(cancellationToken);

                var AllOrderItemData = await _db.MusementItemInfos.Where(s => s.MusementOrderInfoId == musementOrderId && s.DeletedAt == null).ToListAsync(cancellationToken);

                if (AllOrderItemData.Count != 0 || AllOrderItemData != null)
                {
                    _db.MusementItemInfos.RemoveRange(AllOrderItemData);
                }

                foreach (var OrderItem in tokenResponse.items)
                {
                    var musementItemInfos = new MusementItemInfo
                    {
                        ItemUUID = OrderItem.uuid,
                        MusementOrderInfoId = musementOrderId,
                        CartItemUUID = OrderItem.cart_item_uuid,
                        TransactionCode = OrderItem.transaction_code,
                        ItemMusementProductId = OrderItem.product.id,
                        Title = OrderItem.product.title,
                        ProductActivityId = OrderItem.product.activity_uuid,
                        PriceFeature = OrderItem.product.price_tag.price_feature,
                        TicketHolder = OrderItem.product.price_tag.ticket_holder,
                        ProductOriginalRetailPrice = OrderItem.product.original_retail_price.value,
                        ProductServiceFee = OrderItem.product.service_fee.value,
                        ProductDiscountAmount = OrderItem.product.discount_amount.value,
                        TotalPrice = OrderItem.total_retail_price_in_order_currency.value,
                        Quantity = OrderItem.quantity,
                        Currency = OrderItem.total_retail_price_in_order_currency.currency
                    };

                    await _db.MusementItemInfos.AddAsync(musementItemInfos);
                }

                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new MusementCreateOrderOut("Create Order successful.", response));
        }
        else
        {
            return _response.Error("Create order does not exits", AppStatusCodeError.Gone410);
        }

    }
}
