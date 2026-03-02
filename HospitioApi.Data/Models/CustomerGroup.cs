using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGroup : Auditable
{
    public CustomerGroup()
    {
        CustomerUsers = new HashSet<CustomerUser>();
    }
    [MaxLength(50)]
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
    public int? GroupLeaderId { get; set; }
    public int? CustomerId { get; set; }

    public virtual CustomerDepartment? Department { get; set; }
    public virtual CustomerUser? GroupLeader { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<CustomerUser> CustomerUsers { get; set; }
}
