using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class TaxiTransferGuestRequest : Auditable
{
    public int? CustomerId { get; set; }
    public int? GuestId { get; set;}
    [MaxLength(50)]
    public string? TransferId {  get; set; }
    [MaxLength(50)]
    public string? TransferStatus { get; set;}
    public decimal? RefundAmount { get; set; }
    [MaxLength(100)]
    public string? GRPaymentId { get; set; }
    public string? GRPaymentDetails { get;  set; }
    public string? TransferJson {  get; set; }
    public string? RefundId { get; set; }
    public string? RefundStatus { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? HospitioFareAmount { get; set; }
    public string? ExtraDetailsJson { get; set; }
    public DateTime? PickUpDate { get; set; }
    public bool? IsMonthlyReport { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuest? Guest { get; set; }
}
