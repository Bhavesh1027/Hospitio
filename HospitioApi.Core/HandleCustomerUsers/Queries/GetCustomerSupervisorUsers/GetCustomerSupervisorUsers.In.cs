namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerSupervisorUsers;

public class GetCustomerSupervisorUsersIn
{
    public int? DepartmentId { get; set; }
    public int? GroupId { get; set; }
    public int? CustomerUserLevelId { get; set; }
    public int? CustomerUserId { get; set; }
    public int? CustomerId { get; set; }
}
