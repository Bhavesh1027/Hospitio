namespace HospitioApi.Data.Models;

public partial class NotificationHistory : Auditable
{
    public int? NotificationId { get; set; }
    public int? UserId { get; set; }

    public bool? IsRead { get; set; }
    public byte? UserType { get; set; }
   // public virtual Customer? Customer { get; set; }
    public virtual Notification? Notification { get; set; }
}
