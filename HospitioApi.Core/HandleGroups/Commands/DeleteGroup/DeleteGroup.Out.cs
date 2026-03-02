using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Commands.DeleteGroup;

public class DeleteGroupOut : BaseResponseOut
{
    public DeleteGroupOut(string message, DeleteGroup deleteGroup) : base(message)
    {
        DeleteGroup = deleteGroup;
    }
    public DeleteGroup DeleteGroup { get; set; }
}
public class DeleteGroup
{
    public int DeleteGroupId { get; set; }
}