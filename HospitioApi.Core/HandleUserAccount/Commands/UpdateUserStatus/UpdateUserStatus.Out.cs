using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;

public class UpdateUserStatusOut : BaseResponseOut
{
    public UpdateUserStatusOut(string message) : base(message)
    {
    }
}
