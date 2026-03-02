using HospitioApi.Shared;


namespace HospitioApi.Core.HandleUserAccount.Commands.DeleteUserAccount;

public class DeleteUserAccountOut : BaseResponseOut
{
    public DeleteUserAccountOut(string message, DeleteUser deleteUser) : base(message)
    {
        DeleteUser = deleteUser;
    }
    public DeleteUser DeleteUser { get; set; }
}
public class DeleteUser
{
    public int DeleteUserId { get; set; }
}