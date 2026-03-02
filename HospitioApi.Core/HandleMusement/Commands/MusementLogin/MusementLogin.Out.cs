using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Commands.MusementLogin;

public class MusementLoginOut : BaseResponseOut
{
    public MusementLoginOut(string message , MusementLoginResponseOut musementLoginResponse) : base(message)
    {
        musementLoginResponseOut = musementLoginResponse;
    }
    public MusementLoginResponseOut musementLoginResponseOut { get; set; }
}

public class MusementLoginResponseOut
{
    public string? access_token { get; set;}
    public int? expires_in { get;set;}
    public string? token_type { get;set;}
    public string? scope { get;set;}

}

