using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;

public class GetCustomersPaymentProcessorsByIdOut : BaseResponseOut
{
    public GetCustomersPaymentProcessorsByIdOut(string message, CustomersPaymentProcessorsByIdOut customersPaymentProcessorsByIdOut) : base(message)
    {
        CustomersPaymentProcessorsByIdOut = customersPaymentProcessorsByIdOut;
    }
    public CustomersPaymentProcessorsByIdOut CustomersPaymentProcessorsByIdOut { get; set; }
}

public class CustomersPaymentProcessorsByIdOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
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


    public List<PaymentProcessors>? PaymentProcessors { get; set; }

}
public class PaymentProcessors
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public string? GRCategory { get; set; }

    public string? GRGroup { get; set; }

    public string? GRID { get; set; }

    public string? GRIcon { get; set; }

    public string? GRName { get; set; }

    public List<PaymentProcessorsDefinations>? PaymentProcessorsDefinations { get; set; }

}

public class PaymentProcessorsDefinations
{
    public int Id { get; set; }

    public string? GRFields { get; set; }

    public string? GRSupportedCountries { get; set; }

    public string? GRSupportedCurrencies { get; set; }

    public string? GRSupportedFeatures { get; set; }

    public int PaymentProcessorId { get; set; }

    public bool? IsActive { get; set; }


}
