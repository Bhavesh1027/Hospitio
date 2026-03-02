namespace HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;

public class CustomerRefreshTokenIn
{
    public CustomerRefreshTokenIn() { }
    public CustomerRefreshTokenIn(int tokenId, string tokenValue)
    {
        TokenId = tokenId;
        TokenValue = tokenValue;
    }

    public string TokenValue { get; set; } = string.Empty;
    public int TokenId { get; set; } = default;
}
