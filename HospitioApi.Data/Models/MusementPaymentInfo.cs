namespace HospitioApi.Data.Models;

public partial class MusementPaymentInfo : Auditable
{

    public int OrderInfoId { get; set; }
    // 1. ADYEN, 2. Stripe
    public byte? PaymentMetod { get; set; }
    // 1. PENDING ,2. SUCCESS, 3. FAILED
    public byte? PaymentStatus { get; set; }
    public string? PaymentTransactionId { get; set; }
    public byte? PlatForm { get; set; }
    public int? CustomerGuestId { get; set; }
    public int? CustomerId { get; set; }
    public string? OrderUUID { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? Description { get; set; }
}
