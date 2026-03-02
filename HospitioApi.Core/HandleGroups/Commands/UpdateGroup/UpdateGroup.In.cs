namespace HospitioApi.Core.HandleGroups.Commands.UpdateGroup;

public class UpdateGroupIn
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public int? UserType { get; set; }
}
