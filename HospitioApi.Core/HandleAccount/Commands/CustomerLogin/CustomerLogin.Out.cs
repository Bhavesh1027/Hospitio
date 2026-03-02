using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerLogin;

public class CustomerLoginOut : BaseResponseOut
{
	public CustomerLoginOut(
		string message,
		AccessToken accessToken,
	  CustomerUserRefreshToken refreshToken,
	   ChatHubConnectionToken chathubconnectionaccessToken,
	   bool ismuted
		) : base(message)
	{
		AccessToken = accessToken;
		RefreshToken = refreshToken;
		ChatHubConnectionAccessToken = chathubconnectionaccessToken;
		IsMuted = ismuted;
	}
	public AccessToken AccessToken { get; set; }
	public CustomerUserRefreshToken RefreshToken { get; set; }
	public ChatHubConnectionToken ChatHubConnectionAccessToken { get; set; }
	public bool IsMuted { get; set; }
}
