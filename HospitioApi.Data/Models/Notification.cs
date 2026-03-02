using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class Notification : Auditable
{
    public Notification()
    {
        NotificationHistories = new HashSet<NotificationHistory>();
    }

    [MaxLength(20)]
    public string? Country { get; set; }
    [MaxLength(20)]
    public string? City { get; set; }
    [MaxLength(10)]
    public string? Postalcode { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? ProductId { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    public string? Message { get; set; }

    public virtual BusinessType? BusinessType { get; set; }
    public virtual Product? Product { get; set; }
    public virtual ICollection<NotificationHistory> NotificationHistories { get; set; }
}
