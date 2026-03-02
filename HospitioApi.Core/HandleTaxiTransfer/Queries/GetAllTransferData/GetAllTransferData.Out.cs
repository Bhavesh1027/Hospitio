using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetAllTransferData;

public class GetAllTransferDataOut : BaseResponseOut
{
    public GetAllTransferDataOut(string message , List<TaxiTransferResponse> response) : base(message)
    {
        TaxiTransferResponse = response;
    }
    public List<TaxiTransferResponse> TaxiTransferResponse { get; set; }
}

public class TaxiTransferResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? GuestId { get; set; }
    public string? GuestFirstName { get; set; }
    public string? GuestLastName { get; set; }
    public string? ReservationId { get; set; }
    public string? TransferStatus { get; set; }
    public string? TransferId { get; set; }
    public string? GRPaymentId { get; set; }
    public decimal? HospitioFareAmount { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? MarkUpAmount { get; set; }
    public string? RefundStatus { get; set; }
    public int? RefundAmount { get; set; }
    public string? RefundId { get; set; }
    public string? Luggage { get; set; }
    public string? Passenger { get; set; }
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public string? FormLocationDescription { get; set; }
    public string? ToLocationDescription { get; set; }
    public string? PickUpDateTime { get; set; }
    public string? BookedDateTime { get; set; }
    public string? CancelledAt { get; set; }
    public string? PassengerCount { get; set; }
    public string? PassengerName { get; set; }
    public string? PassengerEmail { get; set; }
    public string? PassengerMobile { get; set; }
    public string? Currency { get; set; }
    public string? GRCreateAt { get; set; }
    public string? PaymentServiceTransactionId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentServiceRefeunfId { get; set; }
    public int? FilterCount { get; set; }
}

