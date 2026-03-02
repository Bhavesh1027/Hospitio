using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleAccount.Commands.RevokeToken;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerRevokeToken;
public record CustomerRevokeTokenHandlerRequest(CustomerRevokeTokenIn In)
    : IRequest<AppHandlerResponse>;
public class CustomerRevokeTokenHandler : IRequestHandler<CustomerRevokeTokenHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerRevokeTokenHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IHttpContextAccessor httpContextAccessor)
    {
        _response = response;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppHandlerResponse> Handle(CustomerRevokeTokenHandlerRequest request, CancellationToken cancellationToken)
    {
        var oldDbToken = await _db.CustomerUserRefreshTokens.SingleOrDefaultAsync(t => t.Id == request.In.TokenId, cancellationToken);

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
