using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class AnonymousUsers : Auditable
{
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// 1=Hospitio, 2=Customer , 3=CustomerGuest
    /// </summary>
    public byte? UserType { get; set; }
}
