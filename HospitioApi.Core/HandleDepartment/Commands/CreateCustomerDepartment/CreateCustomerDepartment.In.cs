namespace HospitioApi.Core.HandleDepartment.Commands.CreateCustomerDepartment;

public class CreateCustomerDepartmentIn
{
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public int? CustomerId { get; set; }
}
