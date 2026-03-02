using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyServiceImage : Auditable
{
    public int? CustomerPropertyServiceId { get; set; }
    [MaxLength(500)]
    public string? ServiceImages { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual CustomerPropertyService? CustomerPropertyService { get; set; }
}
