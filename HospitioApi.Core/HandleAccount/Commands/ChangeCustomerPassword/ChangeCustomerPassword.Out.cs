using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.ChangeCustomerPassword;

public class ChangeCustomerPasswordOut : BaseResponseOut
{
    public ChangeCustomerPasswordOut(
        string message
        ) : base(message)
    {
       
    }
}
