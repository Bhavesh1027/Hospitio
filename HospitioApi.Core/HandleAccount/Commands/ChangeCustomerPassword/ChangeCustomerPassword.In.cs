namespace HospitioApi.Core.HandleAccount.Commands.ChangeCustomerPassword;

public class ChangeCustomerPasswordIn
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
