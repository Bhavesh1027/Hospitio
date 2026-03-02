using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using TokenDTO = HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;
public record CustomerRefreshTokenHandlerRequest(CustomerRefreshTokenIn In)
    : IRequest<AppHandlerResponse>;
public class CustomerRefreshTokenHandler : IRequestHandler<CustomerRefreshTokenHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerRefreshTokenHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _response = response;
        _db = db;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppHandlerResponse> Handle(CustomerRefreshTokenHandlerRequest request, CancellationToken cancellationToken)
    {
        var oldDToken = await _db.CustomerUserRefreshTokens.IgnoreQueryFilters()
            .Include(t => t.CustomerUser).ThenInclude(x => x.Customer.Product).Include(x => x.CustomerUser.CustomerUsersPermissions).Include(x => x.CustomerUser.CustomerLevel)
            .Where(t => t.Id == request.In.TokenId && t.CustomerUser.DeletedAt == null && t.CustomerUser.IsActive == true).SingleOrDefaultAsync(cancellationToken);

        var utcNow = DateTime.UtcNow;

        if (oldDToken == null ||
            oldDToken.Token != request.In.TokenValue ||
            oldDToken.ExpiresUtc < utcNow ||
            oldDToken.Revoked != null)
        {
            return _response.Error("Invalid refresh token.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
        }

        var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

        /** Revoke old Db Refresh Token for this user since we are  */
        oldDToken.Revoked = utcNow;
        oldDToken.RevokedByIp = ipAddress;

        var customerUserId = oldDToken.CustomerUserId;
        var tokenExpiresUtc = _jwtService.GetRefreshTokenExpirationUtc();
        var refreshTokenValue = _jwtService.GenerateRefreshTokenValue(customerUserId);
        var newDbRefreshToken = new Data.Models.CustomerUserRefreshToken(customerUserId, refreshTokenValue, tokenExpiresUtc, ipAddress);

        await _db.CustomerUserRefreshTokens.AddAsync(newDbRefreshToken, cancellationToken);

        oldDToken.ReplacedByToken = refreshTokenValue;

        await _db.SaveChangesAsync(cancellationToken);


        var dtoAccessToken = await _jwtService.GenerateJWTokenForCustomerAsync(oldDToken.CustomerUser, cancellationToken);
        var dtoRefreshToken = new TokenDTO.RefreshToken(newDbRefreshToken.Token, newDbRefreshToken.Id, new(newDbRefreshToken.ExpiresUtc));

        return _response.Success(new CustomerRefreshTokenOut("Token refreshed successfully.", dtoAccessToken, dtoRefreshToken));
    }
}
