using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroup;

public class GetGroupOut : BaseResponseOut
{
    public GetGroupOut(string message, GroupOut groupOut) : base(message)
    {
        GroupOut = groupOut;
    }
    public GroupOut GroupOut { get; set; }
}
public class GroupOut
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}
