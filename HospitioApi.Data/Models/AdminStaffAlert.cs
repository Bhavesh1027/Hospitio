using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class AdminStaffAlert: Auditable
{
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
    public string? VonageTemplateId { get;set; }
    public string? VonageTemplateStatus { get;set; }
    public string? WhatsappTemplateName { get; set; }    
    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
