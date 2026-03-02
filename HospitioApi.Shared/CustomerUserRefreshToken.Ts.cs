namespace HospitioApi.Shared;

public class CustomerUserRefreshToken
{
    public CustomerUserRefreshToken(string token, int tokenId, DateTimeUtcUnixEpoch expires)
    {
        Token = token;
        TokenId = tokenId;
        Expires = expires;
    }

    public string Token { get; }
    public int TokenId { get; }
    public DateTimeUtcUnixEpoch Expires { get; }
}
