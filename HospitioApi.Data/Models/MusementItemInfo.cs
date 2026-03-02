namespace HospitioApi.Data.Models;

public partial class MusementItemInfo : Auditable
{
    //public MusementItemInfo()
    //{
    //    MusementPaymentInfos = new HashSet<MusementPaymentInfo>();
    //}


    public int? MusementOrderInfoId { get; set; }
    public string? ProductActivityId { get; set; }
    public int? Quantity { get; set; }
    // 1. CANCELLED ,2. CONFIRMED
    public decimal? ProductOriginalRetailPrice { get; set; }
    public string? ItemUUID { get; set; }
    public string? ItemMusementProductId { get; set; }
    public string? CartItemUUID { get; set; }
    public string? TransactionCode { get; set; }
    public string? Title { get; set; }
    public string? PriceFeature { get; set; }
    public string? TicketHolder { get; set; }
    public string? Currency { get; set; }
    public decimal? ProductDiscountAmount { get; set; }
    public decimal? ProductServiceFee { get; set; }
    public decimal? TotalPrice { get; set; }
    public bool IsCancel { get;set; }
    public virtual MusementOrderInfo? MusementOrderInfo { get; set; }
    //public virtual ICollection<MusementPaymentInfo> MusementPaymentInfos { get; set; }
}
