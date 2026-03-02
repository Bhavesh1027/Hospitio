namespace HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;

public class ResetPasswordConfirmationIn
{
    public string Email { get; set; } = string.Empty;
    public bool IsUser { get; set; }
}
