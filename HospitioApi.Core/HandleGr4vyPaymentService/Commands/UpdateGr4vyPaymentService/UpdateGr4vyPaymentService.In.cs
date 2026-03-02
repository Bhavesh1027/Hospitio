using HospitioApi.Core.HandleGr4vyPaymentService.Commands.CreateGr4vyPaymentService;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.UpdateGr4vyPaymentService;

public class UpdateGr4vyPaymentServiceIn
{
    public int? HospitioPaymentProcessorId { get; set; }
    public bool IsDigitalWallet { get; set; }
    public bool IsActive { get; set; }
    public PaymentService? paymentService { get; set; }
    public DigitalWallet? digitalWallet { get; set; }
}
public class PaymentService
{
    public string? payment_service_definition_id { get; set; }
    public List<string>? accepted_currencies { get; set; }
    public List<string>? accepted_countries { get; set; }
    public string? display_name { get; set; }
    public List<Field>? fields { get; set; }
}
public class DigitalWallet
{
    public bool? accept_terms_and_conditions { get; set; }
    public List<string>? domain_names { get; set; }
    public string? merchant_name { get; set; }
    public string? merchant_url { get; set; }
    public string? provider { get; set; }
}