using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerSupervisorUsers;

public class GetCustomerSupervisorUsersOut : BaseResponseOut
{
    public GetCustomerSupervisorUsersOut(string message) : base(message)
    {
    }
    public GetCustomerSupervisorUsersOut(string message, List<CustomerUserOut> customerUserOut) : base(message)
    {
        CustomerUserOut = customerUserOut;
    }
    public List<CustomerUserOut> CustomerUserOut { get; set; } = new();
}

public class CustomerUserOut
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
    public int? CustomerUserLevelId { get; set; }
    public bool IsActive { get; set; } = true;
    public List<CustomerUsersPermissionOut> CustomerUserModulePermissions { get; set; } = new();

}
public class CustomerUsersPermissionOut
{
    public int Id { get; set; }
    public int? CustomerPermissionId { get; set; }
    public int? CustomerUserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsDownload { get; set; }
}
