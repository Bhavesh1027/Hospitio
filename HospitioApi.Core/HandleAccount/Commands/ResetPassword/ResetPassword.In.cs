namespace HospitioApi.Core.HandleAccount.Commands.ResetPassword;

public class ResetPasswordIn
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
