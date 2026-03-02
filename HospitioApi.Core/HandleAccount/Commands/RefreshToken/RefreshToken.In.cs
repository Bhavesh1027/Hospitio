namespace HospitioApi.Core.HandleAccount.Commands.RefreshToken;

public class RefreshTokenIn
{
    public RefreshTokenIn() { }
    public RefreshTokenIn(int tokenId, string tokenValue)
    {
        TokenId = tokenId;
        TokenValue = tokenValue;
    }

    public string TokenValue { get; set; } = string.Empty;
    public int TokenId { get; set; } = default;
}




