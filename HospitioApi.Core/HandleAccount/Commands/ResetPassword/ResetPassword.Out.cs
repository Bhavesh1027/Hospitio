using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPassword;

public class ResetPasswordOut : BaseResponseOut
{
    public ResetPasswordOut(string message, int id, AccessToken? accessToken, ResetPasswordRefreshTokenOut? refreshToken) : base(message)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Id = id;
    }
    public int Id { get; set; }
    public AccessToken? AccessToken { get; set; }
    public ResetPasswordRefreshTokenOut? RefreshToken { get; set; }
}

public class ResetPasswordRefreshTokenOut
{
    public ResetPasswordRefreshTokenOut(string token, int tokenId, DateTimeUtcUnixEpoch expires)
    {
        Token = token;
        TokenId = tokenId;
        Expires = expires;
    }

    public string Token { get; }
    public int TokenId { get; }
    public DateTimeUtcUnixEpoch Expires { get; }
}

public class ResetPasswordTokenDetailOut
{
    public ResetPasswordTokenDetailOut(int id, string token, DateTime expires, string? remoteIpAddress)
    {
        Id = id;
        Token = token;
        ExpiresUtc = expires;
        CreatedByIp = remoteIpAddress;
    }
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresUtc { get; set; }
    public string? CreatedByIp { get; set; }
}

public class ResetPasswordTokenOut
{
    public AccessToken? AccessToken { get; set; }
    public Shared.ChatHubConnectionToken ChatHubConnectionAccessToken { get; set; }
    public ResetPasswordRefreshTokenOut? RefreshToken { get; set; }
}
