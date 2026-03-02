using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class Group : Auditable
{
    public Group()
    {
        Users = new HashSet<User>();
    }

    [MaxLength(50)]
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
    public int? GroupLeaderId { get; set; }

    public virtual User? GroupLeader { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Ticket> Ticket { get; set; }


}
