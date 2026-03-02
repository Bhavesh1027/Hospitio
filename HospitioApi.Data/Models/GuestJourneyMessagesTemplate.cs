using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class GuestJourneyMessagesTemplate : Auditable
{

    public GuestJourneyMessagesTemplate()
    {
        CustomerGuestJournies = new HashSet<CustomerGuestJourny>();
    }

    /// <summary>
    /// 1: Automatic, 2: Post Booking, 3: Online Check in,  4: Pre-Arrival, 5: In-House,  6: Post-Arrival
    /// </summary>
    [DefaultValue(1)]
    public byte? TempleteType { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    public string? WhatsappTemplateName { get; set; }
    public string? TempletMessage { get; set; }
    public string? Buttons { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public virtual ICollection<CustomerGuestJourny> CustomerGuestJournies { get; set; }

}
