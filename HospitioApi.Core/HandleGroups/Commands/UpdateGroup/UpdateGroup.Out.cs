using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Commands.UpdateGroup;

public class UpdateGroupOut : BaseResponseOut
{
    public UpdateGroupOut(string message, UpdatedGroupOut updatedGroupOut) : base(message)
    {
        UpdatedGroupOut = updatedGroupOut;
    }
    public UpdatedGroupOut UpdatedGroupOut { get; set; }
}
public class UpdatedGroupOut
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
}
