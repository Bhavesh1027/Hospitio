using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentServiceById;

public class GetGr4vyPaymentServiceByIdOut : BaseResponseOut
{
    public GetGr4vyPaymentServiceByIdOut(string message, PaymentServiceByIdOut payment) : base(message)
    {
        paymentService = payment;
    }
    public PaymentServiceByIdOut paymentService { get; set; }
}
public class PaymentServiceByIdOut
{
    public int? Id { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string? GRPaymentServiceId { get; set; }
    public string? GRWebhookURL { get; set; }
    public bool? IsActive { get; set; }
    public bool? GR3DSecureEnabled { get; set; }
    public string? GRAcceptedCountries { get; set; }
    public string? GRAcceptedCurrencies { get; set; }
    public string? GRFields { get; set; }
    public bool? GRIsDeleted { get; set; }
    public string? GRMerchantProfile { get; set; }
    public PaymentProcessorsOuts PaymentProcessorsOuts { get; set; } = new();
    public PaymentProcessorsDefinationsOuts PaymentProcessorsDefinationsOuts { get; set; } = new();
}
public class PaymentProcessorsOuts
{
    public int? Id { get; set; }
    public bool? IsActive { get; set; }
    public string? GRCategory { get; set; }
    public string? GRGroup { get; set; }
    public string? GRID { get; set; }
    public string? GRIcon { get; set; }
    public string? GRName { get; set; }
}
public class PaymentProcessorsDefinationsOuts
{
    public int? Id { get; set; }
    public string? GRFields { get; set; }
    public string? GRSupportedCountries { get; set; }
    public string? GRSupportedCurrencies { get; set; }
    public string? GRSupportedFeatures { get; set; }
    public int? PaymentProcessorId { get; set; }
    public bool? IsActive { get; set; }
}