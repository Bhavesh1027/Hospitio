using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;
public record GetPaymentDetailRequest(GetPaymentDetailIn In) : IRequest<AppHandlerResponse>;
public class GetPaymentDetailHandler : IRequestHandler<GetPaymentDetailRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly ICommonDataBaseOprationService _common;
    private readonly ApplicationDbContext _db;
    public GetPaymentDetailHandler(IDapperRepository dapper, IHandlerResponseFactory response, IJwtService jwtService, ICommonDataBaseOprationService common, ApplicationDbContext db)
    {
        _dapper = dapper;
        _response = response;
        _jwtService = jwtService;
        _common = common;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(GetPaymentDetailRequest request, CancellationToken cancellationToken)
    {
        PaymentDetailOut payment = new PaymentDetailOut();

        string token = _jwtService.GenerateJWTokenForGr4vy();
        payment.Token = token;

        CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _common.GetPaymentProcessorCredential(request.In.CustomerId, _db, cancellationToken);
        payment.Merchant_Account_Id = customerPaymentProcessorCredentials.MerchantId;

        CustomerGuest customerGuest = await _common.GetGuestBuyer(request.In.GuestId, payment.Merchant_Account_Id, token, _db, cancellationToken);
        payment.Buyer_Id = customerGuest.GRBuyerId;
        payment.Country = customerGuest.Country;

        return _response.Success(new GetPaymentDetailOut("Get payment detail successful.", payment));
    }
}
