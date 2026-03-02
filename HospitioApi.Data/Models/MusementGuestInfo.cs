namespace HospitioApi.Data.Models;

public partial class MusementGuestInfo : Auditable
{
    public MusementGuestInfo()
    {
        MusementOrderInfos = new HashSet<MusementOrderInfo>();
    }
    public int CustomerGuestId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public long? MusementCustomerId { get; set; }
    public string? CartUUID { get; set; }

    public virtual CustomerGuest? CustomerGuest { get; set; }
    public virtual ICollection<MusementOrderInfo> MusementOrderInfos { get; set; }
}
