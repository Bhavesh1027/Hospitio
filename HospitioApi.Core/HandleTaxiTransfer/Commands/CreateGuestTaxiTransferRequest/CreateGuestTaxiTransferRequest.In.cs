namespace HospitioApi.Core.HandleTaxiTransfer.Commands.CreateGuestTaxiTransferRequest;

public class CreateGuestTaxiTransferRequestIn
{
    public int CustomerId { get; set; }
    public int GuestId { get; set; }
    public string? TransferId { get; set; }
    public string? TransferStatus {  get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentDetails { get; set; }
    public string? TransferJson { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? HospitioFareAmount { get; set; }
    public DateTime? PickUpDate { get; set; }
    public ExtraDetails? ExtraDetails { get; set; }
}

public class ExtraDetails
{
    public string? Luggage {  get; set; }
    public string? passenger { get; set; }
    public string? FromLocation { get; set;}
    public string? ToLocation { get; set; }
    public DateTime? RefundDate { get; set; }
}
