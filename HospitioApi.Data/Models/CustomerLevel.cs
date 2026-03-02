using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

[Index(nameof(LevelName), IsUnique = true)]
public partial class CustomerLevel : Auditable
{
    public CustomerLevel()
    {
        CustomerUsers = new HashSet<CustomerUser>();
    }
    [MaxLength(50)]
    public string? LevelName { get; set; }
    [MaxLength(50)]
    public string? NormalizedLevelName { get; set; }
    public bool? IsCustomerUserType { get; set; }

    public virtual ICollection<CustomerUser> CustomerUsers { get; set; }

}
