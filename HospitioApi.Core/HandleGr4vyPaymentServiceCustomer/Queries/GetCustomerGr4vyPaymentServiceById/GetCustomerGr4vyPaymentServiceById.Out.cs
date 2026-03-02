using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentServiceById;

public class GetCustomerGr4vyPaymentServiceByIdOut : BaseResponseOut
{
    public GetCustomerGr4vyPaymentServiceByIdOut(string message, CustomerPaymentServiceByIdOut payment) : base(message)
    {
        paymentService = payment;
    }
    public CustomerPaymentServiceByIdOut paymentService { get; set; }
}
public class CustomerPaymentServiceByIdOut
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
    public CustomerPaymentProcessorsOuts PaymentProcessorsOuts { get; set; } = new();
    public CustomerPaymentProcessorsDefinationsOuts PaymentProcessorsDefinationsOuts { get; set; } = new();
}
public class CustomerPaymentProcessorsOuts
{
    public int? Id { get; set; }
    public bool? IsActive { get; set; }
    public string? GRCategory { get; set; }
    public string? GRGroup { get; set; }
    public string? GRID { get; set; }
    public string? GRIcon { get; set; }
    public string? GRName { get; set; }
}
public class CustomerPaymentProcessorsDefinationsOuts
{
    public int? Id { get; set; }
    public string? GRFields { get; set; }
    public string? GRSupportedCountries { get; set; }
    public string? GRSupportedCurrencies { get; set; }
    public string? GRSupportedFeatures { get; set; }
    public int? PaymentProcessorId { get; set; }
    public bool? IsActive { get; set; }
}