namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;


public class UpdateUserStatusIn
{
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}
