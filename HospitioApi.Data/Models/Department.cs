using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class Department : Auditable
{
    public Department()
    {
        Groups = new HashSet<Group>();
        Users = new HashSet<User>();
    }
    [MaxLength(50)]
    public string? Name { get; set; }
    public int? DepartmentMangerId { get; set; }
    public virtual User? DepartmentManger { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<User> Users { get; set; }
}
