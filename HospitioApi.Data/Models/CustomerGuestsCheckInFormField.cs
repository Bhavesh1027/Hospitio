using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestsCheckInFormField : Auditable
{
    public int? CustomerId { get; set; }
    public int? CustomerGuestsCheckInFormBuilderId { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? FieldType { get; set; }
    public bool? RequiredFields { get; set; }
    public int? DisplayOrder { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestsCheckInFormBuilder? CustomerGuestsCheckInFormBuilder { get; set; }
}
