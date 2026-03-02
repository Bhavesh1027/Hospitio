using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomers;
public class GetAdminUserCustomersOut : BaseResponseOut
{
    public GetAdminUserCustomersOut(string message, List<AdminUserCustomersOut> adminUserCustomersOut) : base(message)
    {
        AdminUserCustomersOut = adminUserCustomersOut;
    }
    public List<AdminUserCustomersOut> AdminUserCustomersOut { get; set; }
}
public class AdminUserCustomersOut
{
    public int? Id { get; set; }
    public string? BusinessName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserType { get; set; }
    public string? UserName { get; set; }
    public string? ProfilePicture { get; set; }
    public List<UserOuts> UserOuts { get; set; }
}
public class UserOuts
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
}