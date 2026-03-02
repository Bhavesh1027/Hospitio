using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;

public class GetCustomersPaymentProcessorsOut : BaseResponseOut
{
    public GetCustomersPaymentProcessorsOut(string message, List<CustomersPaymentProcessorsOut> customersPaymentProcessorsOut) : base(message)
    {
        CustomersPaymentProcessorsOut = customersPaymentProcessorsOut;
    }
    public List<CustomersPaymentProcessorsOut> CustomersPaymentProcessorsOut { get; set; } = new List<CustomersPaymentProcessorsOut>();
}
public class CustomersPaymentProcessorsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Currency { get; set; }
}
