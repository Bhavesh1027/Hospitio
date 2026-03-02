using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroups;

public class GetGroupsOut : BaseResponseOut
{
    public GetGroupsOut(string message, List<GroupsOut> groupsOut) : base(message)
    {
        GroupsOut = groupsOut;
    }
    public List<GroupsOut> GroupsOut { get; set; } = new();
}
public class GroupsOut
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}
