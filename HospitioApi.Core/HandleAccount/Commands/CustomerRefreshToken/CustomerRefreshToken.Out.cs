using TokenDTO = HospitioApi.Shared;
namespace HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;

public class CustomerRefreshTokenOut : TokenDTO.BaseResponseOut
{
    public CustomerRefreshTokenOut(
        string message,
        TokenDTO.AccessToken accessToken,
        TokenDTO.RefreshToken refreshToken
        ) : base(message)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    public TokenDTO.AccessToken AccessToken { get; set; }
    public TokenDTO.RefreshToken RefreshToken { get; set; }
}
