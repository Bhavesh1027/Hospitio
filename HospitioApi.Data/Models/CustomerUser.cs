using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerUser : Auditable
{
	public CustomerUser()
	{
		CustomerGroups = new HashSet<CustomerGroup>();
		InverseSupervisor = new HashSet<CustomerUser>();
		CustomerDepartments = new HashSet<CustomerDepartment>();
		CustomerUsersPermissions = new HashSet<CustomerUsersPermission>();
	}
	public int? CustomerId { get; set; }

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
	[MaxLength(100)]
	public string? UserName { get; set; }
	[MaxLength(255)]
	public string? Password { get; set; }
	public int? CustomerDepartmentId { get; set; }
	public int? CustomerGroupId { get; set; }

	public int? CustomerLevelId { get; set; }
	public int? SupervisorId { get; set; }
	public bool IsMuted { get; set; }

	public virtual CustomerDepartment? CustomerDepartment { get; set; }
	public virtual CustomerGroup? CustomerGroup { get; set; }
	public virtual CustomerUser? Supervisor { get; set; }
	public virtual CustomerLevel? CustomerLevel { get; set; }
	public virtual Customer? Customer { get; set; }

	public virtual ICollection<CustomerGroup> CustomerGroups { get; set; }
	public virtual ICollection<CustomerUser> InverseSupervisor { get; set; }
	public virtual ICollection<CustomerDepartment> CustomerDepartments { get; set; }
	public virtual ICollection<CustomerUsersPermission> CustomerUsersPermissions { get; set; }

}
