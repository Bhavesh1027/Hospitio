using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroupsByDepartmentId;

public class GetGroupsByDepartmentIdOut: BaseResponseOut
{
    public GetGroupsByDepartmentIdOut(string message, List<GroupsByDepartmentIdOut> groupsOut) : base(message)
    {
        GroupsOut = groupsOut;
    }
    public List<GroupsByDepartmentIdOut> GroupsOut { get; set; } = new();
}
public class GroupsByDepartmentIdOut
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
}