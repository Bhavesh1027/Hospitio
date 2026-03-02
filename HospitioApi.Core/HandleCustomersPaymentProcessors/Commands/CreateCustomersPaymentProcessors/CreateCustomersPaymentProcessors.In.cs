namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;

public class CreateCustomersPaymentProcessorsIn
{
    public int? CustomerId { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Currency { get; set; }
}
