namespace HospitioApi.Core.HandleUserAccount.Commands.EditUserAccount;

public class EditUserAccountIn
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
    public int? GroupId { get; set; }
    public int? UserLevelId { get; set; }
    public int? SupervisorId { get; set; }
    //public int? CustomerId { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;
    public List<EditUsersPermissionIn> UserModulePermissions { get; set; } = new();

}

public class EditUsersPermissionIn
{
    public int? Id { get; set; }
    public int? PermissionId { get; set; }
    public int? UserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsSend { get; set; }
}