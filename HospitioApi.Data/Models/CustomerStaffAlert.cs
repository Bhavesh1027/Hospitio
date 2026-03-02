using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerStaffAlert : Auditable
{
    public int? CustomerId { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(10)]
    public string? Platfrom { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public int? WaitTimeInMintes { get; set; }
    public string? Msg { get; set; }
    public int? CustomerUserId { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual CustomerUser? CustomerUser { get; set; }
}
