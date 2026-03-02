namespace HospitioApi.Core.HandleDepartment.Commands.EditDepartment;

public class EditDepartmentIn
{
    public string Name { get; set; } = null!;
    public bool? IsActive { get; set; }
    public int? UserType { get; set; }
}


