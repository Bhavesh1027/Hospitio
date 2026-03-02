using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsersForDropdown;

public class GetUsersForDropdownOut : BaseResponseOut
{
    public GetUsersForDropdownOut(string message, List<AdminUsersOut> adminUsers) : base(message)
    {
        adminUsersOuts = adminUsers;
    }
    public List<AdminUsersOut> adminUsersOuts { get; set; } = new List<AdminUsersOut>();
}
public class AdminUsersOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
}