namespace HospitioApi.Data.Models;

public partial class MusementOrderInfo : Auditable
{
    public MusementOrderInfo()
    {
        MusementItemInfos = new HashSet<MusementItemInfo>();
    }

    public int MusementGuestInfoId { get; set; }
    public string? OrderUUID { get; set; }
    public string? Identifier { get; set; }

    // 1. PENDING , 2. COMPLETED, 3. CANCELLED
    public string? Currency { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? PaymentJson { get; set; }
    public string? CartUUID { get; set; }

    public virtual MusementGuestInfo? MusementGuestInfo { get; set; }
    public virtual ICollection<MusementItemInfo> MusementItemInfos { get; set; }
}
