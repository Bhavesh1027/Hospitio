using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Commands.CreateGroup;

public class CreateGroupOut : BaseResponseOut
{
    public CreateGroupOut(string message, CreatedGroupOut createdGroupOut) : base(message)
    {
        CreatedGroupOut = createdGroupOut;
    }
    public CreatedGroupOut CreatedGroupOut { get; set; }
}
public class CreatedGroupOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
}
