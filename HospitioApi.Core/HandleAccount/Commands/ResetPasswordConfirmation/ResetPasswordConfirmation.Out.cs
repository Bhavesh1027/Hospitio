using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;

public class ResetPasswordConfirmationOut : BaseResponseOut
{
    public ResetPasswordConfirmationOut(string message) : base(message) { }
}
