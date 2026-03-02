using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.EncryptPasswords;

public class EncryptPasswordsOut : BaseResponseOut
{
    public EncryptPasswordsOut(
        string message
        ) : base(message)
    {
       
    }
}
