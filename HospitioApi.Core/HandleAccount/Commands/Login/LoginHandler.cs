using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.Login;

public record LoginHandlerRequest(LoginIn In)
    : IRequest<AppHandlerResponse>;

public class LoginHandler : IRequestHandler<LoginHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _response = response;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppHandlerResponse> Handle(LoginHandlerRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.Include(x => x.UserLevel).Include(t => t.UserLevel).Include(r => r.UsersPermissions).ThenInclude(x => x.Permission).IgnoreQueryFilters()
            .Where(u => (u.Email == request.In.Email || u.UserName == request.In.Email)
            && u.IsActive == true
            ).SingleOrDefaultAsync(cancellationToken);

        if (user != null)
        {
            var encryptedPassword = CryptoExtension.Encrypt(request.In.Password, user!.Id.ToString());

            if (user.Password != encryptedPassword)
            {
                /** Do not expose to the client the fact that email does not exists */
                return _response.Error("Invalid login attempt.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
            }

            var tokenExpiresUtc = _jwtService.GetRefreshTokenExpirationUtc();
            var hubconnectionExpiresUtc = _jwtService.GetHubConnectionTokenExpirationUtc();
            var refreshTokenValue = _jwtService.GenerateRefreshTokenValue(user.Id);

            var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

            var dbRefreshToken = new Data.Models.RefreshToken(user.Id, refreshTokenValue, tokenExpiresUtc, ipAddress);

            await _db.RefreshTokens.AddAsync(dbRefreshToken, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            var dtoAccessToken = await _jwtService.GenerateJWTokenAsync(user, cancellationToken);
            var dtoChatHubConnectionAccessToken = await _jwtService.GenerateHubConnectionJWTokenAsync(user, cancellationToken);
            var dtoRefreshToken = new Shared.RefreshToken(dbRefreshToken.Token, dbRefreshToken.Id, new(dbRefreshToken.ExpiresUtc));
            var dtoChatHubConnectionToken = new ChatHubConnectionToken(dtoChatHubConnectionAccessToken, new(hubconnectionExpiresUtc));

            return _response.Success(new LoginOut("Login successful.", user.Id, dtoAccessToken, dtoRefreshToken, dtoChatHubConnectionToken));
        }

        return _response.Error("Invalid login attempt.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
    }
}
