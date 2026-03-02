using static HospitioApi.Shared.GetPagedExtension;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;

public class GetDepartmentCustomerUsersIn : BaseSearchFilterOptions
{
    public int? CustomerId { get; set; }
}
