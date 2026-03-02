namespace HospitioApi.Core.HandleAccount.Commands.ChangeUserPassword;

public class ChangeUserPasswordIn
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
