namespace HospitioApi.Shared;

public class ChatHubConnectionToken
{
    public ChatHubConnectionToken(AccessToken token, DateTimeUtcUnixEpoch expires)
    {
        Token = token;
        Expires = expires;
    }

    public AccessToken Token { get; }
    public DateTimeUtcUnixEpoch Expires { get; }
}
