using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class Lead : Auditable
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [MaxLength(50)]
    public string? LastName { get; set; }
    [MaxLength(100)]
    public string? Company { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    public string? Comment { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [ForeignKey("Users")]
    public int? ContactFor { get; set; }
    public virtual User? Users { get; set; }

}
