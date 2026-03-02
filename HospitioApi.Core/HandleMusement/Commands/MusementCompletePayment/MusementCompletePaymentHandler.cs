using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;
using static Vonage.ProactiveConnect.Lists.SyncStatus;

namespace HospitioApi.Core.HandleMusement.Commands.MusementCompletePayment;
public record MusementCompletePaymentRequest(MusementCompletePaymentIn In) : IRequest<AppHandlerResponse>;
public class MusementCompletePaymentHandler : IRequestHandler<MusementCompletePaymentRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MusementSettingsOptions _musementSettings;
    private readonly ApplicationDbContext _db;
    public MusementCompletePaymentHandler(
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

    public async Task<AppHandlerResponse> Handle(MusementCompletePaymentRequest request, CancellationToken cancellationToken)
    {
        var paymentInfo = await _db.MusementPaymentInfos.Where(s => s.OrderUUID == request.In.order_uuid).FirstOrDefaultAsync(cancellationToken);
        if ( paymentInfo != null )
        {
            return _response.Error("Payment Alredy Exits", AppStatusCodeError.Gone410);
        }
        var OrderInfo = await _db.MusementOrderInfos.Where(s => s.OrderUUID == request.In.order_uuid && s.DeletedAt == null).FirstOrDefaultAsync();

        var MusementGuestInfo = await _db.MusementGuestInfos.Where(s => s.CartUUID == OrderInfo.CartUUID && s.Id == OrderInfo.MusementGuestInfoId && s.DeletedAt == null).FirstOrDefaultAsync();

        if (MusementGuestInfo != null)
        {
            _db.MusementGuestInfos.Remove(MusementGuestInfo);
        }

        if (OrderInfo != null)
        {
            _db.MusementOrderInfos.Remove(OrderInfo);
        }

        MusementPaymentInfo paymentyInfo = new MusementPaymentInfo();
        paymentyInfo.CustomerGuestId = int.Parse(request.In.GuestId);
        paymentyInfo.CustomerId = int.Parse(request.In.CustomerId);
        paymentyInfo.OrderUUID = request.In.order_uuid;
        paymentyInfo.PlatForm = (byte)(MusementPaymentPlatFormEnum.Musement);
        paymentyInfo.PaymentStatus = (byte)(MusementPaymentStatusEnum.COMPLETED);
        paymentyInfo.PaymentMetod = (byte)((int)Enum.Parse(typeof(Shared.Enums.MusementPaymentMethodEnum), request.In.PaymentMethod, true));

        if (OrderInfo != null)
        {
            paymentyInfo.Amount = OrderInfo.TotalPrice;
            paymentyInfo.Currency = OrderInfo.Currency;
            paymentyInfo.OrderInfoId = OrderInfo.Id;
        }
        await _db.MusementPaymentInfos.AddAsync(paymentyInfo);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new MusementCompletePaymentOut("Create MusementPayment successful."));
    }
}
