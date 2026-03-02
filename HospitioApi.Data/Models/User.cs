using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class User : Auditable
{
    public User()
    {
        Departments = new HashSet<Department>();
        Groups = new HashSet<Group>();
        InverseSupervisor = new HashSet<User>();
        Leads = new HashSet<Lead>();
        RefreshTokens = new HashSet<RefreshToken>();
        Tickets = new HashSet<Ticket>();
        UsersPermissions = new HashSet<UsersPermission>();
    }

    [MaxLength(50)]
    public string? FirstName { get; set; }
    [MaxLength(50)]
    public string? LastName { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    [MaxLength(5)]
    public string? Title { get; set; }
    [MaxLength(500)]
    public string? ProfilePicture { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public int? DepartmentId { get; set; }
    public int? GroupId { get; set; }

    public int? UserLevelId { get; set; }
    public int? SupervisorId { get; set; }
    [MaxLength(100)]
    public string? UserName { get; set; }
    [MaxLength(255)]
    public string? Password { get; set; }
    public DateTime? DeActivated { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Group? Group { get; set; }

    public virtual User? Supervisor { get; set; }
    public virtual UserLevel? UserLevel { get; set; }
    public virtual ICollection<Department> Departments { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<User> InverseSupervisor { get; set; }
    public virtual ICollection<Lead> Leads { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
    public virtual ICollection<UsersPermission> UsersPermissions { get; set; }

}
