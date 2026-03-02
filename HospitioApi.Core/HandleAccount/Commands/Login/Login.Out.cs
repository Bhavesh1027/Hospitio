using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.Login;

public class LoginOut : BaseResponseOut
{
    public LoginOut(
        string message, int userId,
        AccessToken accessToken,
      Shared.RefreshToken refreshToken,
      ChatHubConnectionToken chathubconnectionaccessToken
        ) : base(message)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserId = userId;
        ChatHubConnectionAccessToken = chathubconnectionaccessToken;
    }
    public int UserId { get; set; }
    public AccessToken AccessToken { get; set; }
    public Shared.RefreshToken RefreshToken { get; set; }
    public Shared.ChatHubConnectionToken ChatHubConnectionAccessToken { get; set; }
}
