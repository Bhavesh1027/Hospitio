namespace HospitioApi.Core.HandleMusement.Commands.MusementCompletePayment;

public class MusementCompletePaymentIn
{
    public string? order_uuid { get; set; }
    public string? GuestId { get; set; }
    public string? CustomerId { get; set; }
    public string? PaymentMethod { get; set; }

}
