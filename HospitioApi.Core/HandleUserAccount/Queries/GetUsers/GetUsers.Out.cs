using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsers;

public class GetUsersOut : BaseResponseOut
{
    public GetUsersOut(string message) : base(message)
    {
    }
    public GetUsersOut(string message, List<UserOut> userOut) : base(message)
    {
        UserOut = userOut;
    }
    public List<UserOut> UserOut { get; set; } = new();
}

public class DeptWiseUserOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? ManagerId { get; set; }
    public ManagerOut? Manager { get; set; }
    public List<GroupOut> Groups { get; set; }
    public List<UserOut> Users { get; set; }
}

public class ManagerOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}

public class GroupOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public GroupLeaderOut? GroupLeader { get; set; }
}
public class GroupLeaderOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}

public class UserOut
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DepartmentId { get; set; }
    public int? UserLevelId { get; set; }
    public bool IsActive { get; set; } = true;
    //No need for now per UI
    public List<UsersPermissionOut> UserModulePermissions { get; set; } = new();

}
public class UsersPermissionOut
{
    public int Id { get; set; }
    public int? PermissionId { get; set; }
    public int? UserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsSend { get; set; }
}
public class UserLevelOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}
