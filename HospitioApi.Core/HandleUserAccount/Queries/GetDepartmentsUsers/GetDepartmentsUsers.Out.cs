using HospitioApi.Shared;


namespace HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;


public class GetDepartmentsUsersOut : BaseResponseOut
{
    public GetDepartmentsUsersOut(string message, List<DepartmentsOut> deptWiseUserOut) : base(message)
    {
        DeptWiseUserOut = deptWiseUserOut;
    }
    public List<DepartmentsOut> DeptWiseUserOut { get; set; } = new();
}

public class DepartmentsOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? CeoId { get; set; }
    public string? CeoName { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public int? FilteredCount { get; set; }
    public bool IsActive { get; set; }
    public List<GroupOut>? Groups { get; set; } = new();

}


public class GroupOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? GroupLeaderId { get; set; }
    public string? GroupLeader { get; set; }
    public bool IsActive { get; set; }
    public List<UserOut>? UsersOut { get; set; } = new();
}

public class UserOut
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;


}
