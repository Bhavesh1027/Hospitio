using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestJourny : Auditable
{
    public int CutomerId { get; set; }
    /// <summary>
    ///  1: Automatic, 2: Post Booking, 3: Online Check in, 4: Pre-Arrival, 5: In-House, 6: Post-Arrival
    /// </summary>
    [DefaultValue(1)]
    public byte? JourneyStep { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    public string? WhatsappTemplateName { get; set; }
    /// <summary>
    /// 1. Reservation Events,  2. Time and day of week (In-House)
    /// </summary>
    [DefaultValue(1)]
    public byte? SendType { get; set; }
    /// <summary>
    /// 1. Days,    2. Hours
    /// </summary>
    [DefaultValue(1)]
    public byte? TimingOption1 { get; set; }
    /// <summary>
    /// 1. After,   2. Before
    /// </summary>
    [DefaultValue(1)]
    public byte? TimingOption2 { get; set; }
    /// <summary>
    ///  1. Booking, 2. Arrival,  3. Departure
    /// </summary>
    [DefaultValue(1)]
    public byte? TimingOption3 { get; set; }
    public int? Timing { get; set; }
    [MaxLength(50)]
    public string? NotificationDays { get; set; }
    [Column(TypeName = "time(0)")]
    public TimeSpan? NotificationTime { get; set; }
    public int? GuestJourneyMessagesTemplateId { get; set; }
    public string? TempletMessage { get; set; }
    public string? Buttons { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public virtual Customer Cutomer { get; set; } = null!;
    public virtual GuestJourneyMessagesTemplate? GuestJourneyMessagesTemplate { get; set; }
}
