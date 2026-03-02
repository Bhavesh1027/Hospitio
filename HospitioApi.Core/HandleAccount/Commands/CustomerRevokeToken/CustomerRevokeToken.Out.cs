using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerRevokeToken;

public class CustomerRevokeTokenOut : BaseResponseOut
{
    public CustomerRevokeTokenOut(string message) : base(message) { }
}
