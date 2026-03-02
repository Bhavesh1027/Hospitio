namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessors;

public class CreateHospitioPaymentProcessorsIn
{
    public int PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
