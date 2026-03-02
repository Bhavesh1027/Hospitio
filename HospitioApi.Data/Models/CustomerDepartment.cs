using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerDepartment : Auditable
{
    public CustomerDepartment()
    {
        CustomerUsers = new HashSet<CustomerUser>();
        CustomerGroups = new HashSet<CustomerGroup>();
    }
    [MaxLength(50)]
    public string? Name { get; set; }
    public int? DepartmentMangerId { get; set; }
    public int? CustomerId { get; set; }
    public virtual CustomerUser? DepartmentManger { get; set; }
    public virtual Customer? Customers { get; set; }
    public virtual ICollection<CustomerGroup> CustomerGroups { get; set; }
    public virtual ICollection<CustomerUser> CustomerUsers { get; set; }
}
