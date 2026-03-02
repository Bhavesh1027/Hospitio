namespace HospitioApi.Shared;

public class AccessToken
{
    public string Jwt { get; }
    public AccessToken(string token)
    {
        Jwt = token;
    }
}
