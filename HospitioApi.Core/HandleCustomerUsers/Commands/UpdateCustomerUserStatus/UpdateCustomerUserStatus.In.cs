namespace HospitioApi.Core.HandleCustomerUsers.Commands.UpdateCustomerUserStatus;

public class UpdateCustomerUserStatusIn
{
    public int CustomerUserId { get; set; }
    public bool IsActive { get; set; }
}
