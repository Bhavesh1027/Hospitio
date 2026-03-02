namespace HospitioApi.Core.HandleTaxiTransfer.Commands.CancelGuestTaxiTransferRequest;

public class CancelGuestTaxiTransferRequestIn
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int GuestId { get; set; }
    public string? TransferId { get; set; }
    public string? TransferStatus { get; set; }
    public int? RefundAmount { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentDetails { get; set; }
    public string? TransferJson { get; set; }
    public string? RefundId { get; set; }
    public string? RefundStatus { get; set; }
}
