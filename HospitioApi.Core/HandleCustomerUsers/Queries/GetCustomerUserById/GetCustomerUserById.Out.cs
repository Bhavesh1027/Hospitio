using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserById;

public class GetCustomerUserByIdOut : BaseResponseOut
{
    public GetCustomerUserByIdOut(string message) : base(message)
    {
    }
    public GetCustomerUserByIdOut(string message, CustomerUserByIdOut customerUserByIdOut) : base(message)
    {
        CustomerUserByIdOut = customerUserByIdOut;
    }
    public CustomerUserByIdOut CustomerUserByIdOut { get; set; } = new();
}


public class CustomerUserByIdOut
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
    public int? CustomerUserLevelId { get; set; }
    public int? SupervisorId { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;

    public List<CustomerUserPermissionsOut> CustomerUserModulePermissions { get; set; } = new();

}

public class CustomerUserPermissionsOut
{
    public int? Id { get; set; }
    public int? PermissionId { get; set; }
    public int? CustomerUserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsDownload { get; set; }
}
