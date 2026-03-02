using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.RevokeToken;

public class RevokeTokenOut : BaseResponseOut
{
    public RevokeTokenOut(string message) : base(message) { }
}
