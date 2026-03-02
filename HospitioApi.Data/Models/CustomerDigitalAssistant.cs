using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerDigitalAssistant : Auditable
{

    public int? CustomerId { get; set; }
    [MaxLength(500)]
    public string? Name { get; set; }
    public string? Details { get; set; }
    [MaxLength(100)]
    public string? Icon { get; set; }

    public virtual Customer? Customer { get; set; }
}
