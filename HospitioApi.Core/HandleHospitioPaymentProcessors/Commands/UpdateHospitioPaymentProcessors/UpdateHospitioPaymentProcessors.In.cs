namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessors;

public class UpdateHospitioPaymentProcessorsIn
{
    public int Id { get; set; }
    public int PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
