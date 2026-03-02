using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;

public class GetDepartmentCustomerUsersOut : BaseResponseOut
{
    public GetDepartmentCustomerUsersOut(string message, List<CustomerDepartmentsOut> customerdeptWiseUserOut) : base(message)
    {
        CustomerDeptWiseUserOut = customerdeptWiseUserOut;
    }
    public List<CustomerDepartmentsOut> CustomerDeptWiseUserOut { get; set; } = new();
}

public class CustomerDepartmentsOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? CeoId { get; set; }
    public string? CeoName { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public int? FilteredCount { get; set; }
    public bool IsActive { get; set; }
    public List<CustomerGroupOut>? CustomerGroups { get; set; } = new();

}


public class CustomerGroupOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? GroupLeaderId { get; set; }
    public string? GroupLeader { get; set; }
    public bool IsActive { get; set; }
    public List<CustomerUserOut>? CustomerUsersOut { get; set; } = new();
}

public class CustomerUserOut
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;


}