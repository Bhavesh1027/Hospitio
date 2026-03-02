using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.Services.Jwt;

public interface IJwtService
{
    Task<AccessToken> GenerateJWTokenAsync(User user, CancellationToken cancellationToken);
    Task<AccessToken> GenerateJWTokenForCustomerAsync(CustomerUser customerUser, CancellationToken cancellationToken);
    string GenerateRefreshTokenValue(int userId);
    DateTime GetRefreshTokenExpirationUtc();
    DateTime GetHubConnectionTokenExpirationUtc();
    string GenerateJWTokenForGr4vy();
    string GenerateJWTResetPasswordTokenAsync(User user);
    string GenerateJWTResetPasswordTokenForCustomerAsync(CustomerUser customerUser);
    Task<AccessToken> GenerateHubConnectionJWTokenAsync(User user, CancellationToken cancellationToken);
    Task<AccessToken> GenerateJWTokenForChatHubConnectionCustomerAsync(CustomerUser customerUser, CancellationToken cancellationToken);
}
