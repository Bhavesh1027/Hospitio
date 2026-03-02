
namespace HospitioApi.Core.HandleAccount.Commands.RevokeToken;

public class RevokeTokenIn
{
    public RevokeTokenIn() { }
    public RevokeTokenIn(int tokenId, string tokenValue)
    {
        TokenId = tokenId;
        TokenValue = tokenValue;
    }

    public string TokenValue { get; set; } = string.Empty;
    public int TokenId { get; set; } = default;
}

