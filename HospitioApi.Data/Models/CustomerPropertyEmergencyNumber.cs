using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyEmergencyNumber : Auditable
{
    public int? CustomerPropertyInformationId { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }

    public int? DisplayOrder { get; set; }
    public virtual CustomerPropertyInformation? CustomerPropertyInformation { get; set; }
}
