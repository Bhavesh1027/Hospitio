using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using TokenDTO = HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.RefreshToken;

public record RefreshTokenHandlerRequest(RefreshTokenIn In)
    : IRequest<AppHandlerResponse>;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenHandler(
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

    public async Task<AppHandlerResponse> Handle(RefreshTokenHandlerRequest request, CancellationToken cancellationToken)
    {
        var oldDToken = await _db.RefreshTokens.IgnoreQueryFilters()
            .Include(t => t.User.UserLevel).Include(t => t.User.UsersPermissions).ThenInclude(x => x.Permission).IgnoreQueryFilters()
            .Where(t => t.Id == request.In.TokenId && t.User.IsActive == true).SingleOrDefaultAsync(cancellationToken);

        var utcNow = DateTime.UtcNow;

        if (oldDToken == null ||
            oldDToken.Token != request.In.TokenValue ||
            oldDToken.ExpiresUtc < utcNow ||
            oldDToken.Revoked != null || oldDToken.User == null)
        {
            return _response.Error("Invalid refresh token.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
        }

        var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

        /** Revoke old Db Refresh Token for this user since we are  */
        oldDToken.Revoked = utcNow;
        oldDToken.RevokedByIp = ipAddress;

        var userId = oldDToken.UserId;
        var tokenExpiresUtc = _jwtService.GetRefreshTokenExpirationUtc();
        var refreshTokenValue = _jwtService.GenerateRefreshTokenValue(userId);
        var newDbRefreshToken = new Data.Models.RefreshToken(userId, refreshTokenValue, tokenExpiresUtc, ipAddress);

        await _db.RefreshTokens.AddAsync(newDbRefreshToken, cancellationToken);

        oldDToken.ReplacedByToken = refreshTokenValue;

        await _db.SaveChangesAsync(cancellationToken);


        var dtoAccessToken = await _jwtService.GenerateJWTokenAsync(oldDToken.User, cancellationToken);
        var dtoRefreshToken = new TokenDTO.RefreshToken(newDbRefreshToken.Token, newDbRefreshToken.Id, new(newDbRefreshToken.ExpiresUtc));

        return _response.Success(new RefreshTokenOut("Token refreshed successfully.", dtoAccessToken, dtoRefreshToken));
    }
}
