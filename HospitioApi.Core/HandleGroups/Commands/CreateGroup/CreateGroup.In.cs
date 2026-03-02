namespace HospitioApi.Core.HandleGroups.Commands.CreateGroup;

public class CreateGroupIn
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public int? UserType { get; set; }
    public int? UserId { get; set; }
}

