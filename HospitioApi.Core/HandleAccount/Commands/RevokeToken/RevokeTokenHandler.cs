using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.RevokeToken;

public record RevokeTokenHandlerRequest(RevokeTokenIn In)
    : IRequest<AppHandlerResponse>;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RevokeTokenHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IHttpContextAccessor httpContextAccessor)
    {
        _response = response;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppHandlerResponse> Handle(RevokeTokenHandlerRequest request, CancellationToken cancellationToken)
    {
        var oldDbToken = await _db.RefreshTokens.SingleOrDefaultAsync(t => t.Id == request.In.TokenId, cancellationToken);

        var utcNow = DateTime.UtcNow;

        if (oldDbToken == null ||
            oldDbToken.Token != request.In.TokenValue ||
            oldDbToken.ExpiresUtc < utcNow ||
            oldDbToken.Revoked != null)
        {
            return _response.Error("Invalid token.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
        }

        var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

        oldDbToken.Revoked = utcNow;
        oldDbToken.RevokedByIp = ipAddress;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new RevokeTokenOut("Token revoked successfully."));
    }
}
