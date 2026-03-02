using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetAdminPaymentDetail;
public record GetAdminPaymentDetailRequest(GetAdminPaymentDetailIn In) : IRequest<AppHandlerResponse>;
public class GetAdminPaymentDetailHandler : IRequestHandler<GetAdminPaymentDetailRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly ICommonDataBaseOprationService _common;
    private readonly ApplicationDbContext _db;
    public GetAdminPaymentDetailHandler(IDapperRepository dapper, IHandlerResponseFactory response, IJwtService jwtService, ICommonDataBaseOprationService common, ApplicationDbContext db)
    {
        _dapper = dapper;
        _response = response;
        _jwtService = jwtService;
        _common = common;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(GetAdminPaymentDetailRequest request, CancellationToken cancellationToken)
    {
        AdminPaymentDetailOut payment = new AdminPaymentDetailOut();

        string token = _jwtService.GenerateJWTokenForGr4vy();
        payment.Token = token;
        payment.Merchant_Account_Id = "default";

        CustomerGuest customerGuest = await _common.GetAdminGuestBuyer(request.In.GuestId, payment.Merchant_Account_Id, token, _db, cancellationToken);
        payment.Buyer_Id = customerGuest.GRAdminBuyerId;
        payment.Country = customerGuest.Country;

        return _response.Success(new GetAdminPaymentDetailOut("Get payment detail successful.", payment));
    }
}
