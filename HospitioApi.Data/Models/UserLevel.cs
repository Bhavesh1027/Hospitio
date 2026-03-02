using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

[Index(nameof(LevelName), IsUnique = true)]
public partial class UserLevel : Auditable
{
    public UserLevel()
    {
        Users = new HashSet<User>();
    }
    [MaxLength(50)]
    public string? LevelName { get; set; }
    [MaxLength(50)]
    public string? NormalizedLevelName { get; set; }
    public bool? IsHospitioUserType { get; set; }

    public virtual ICollection<User> Users { get; set; }
}
