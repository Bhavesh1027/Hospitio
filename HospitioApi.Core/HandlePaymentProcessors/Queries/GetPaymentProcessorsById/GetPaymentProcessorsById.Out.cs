using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;

public class GetPaymentProcessorsByIdOut : BaseResponseOut
{
    public GetPaymentProcessorsByIdOut(string message, PaymentProcessorsByIdOut paymentProcessorsByIdOut) : base(message)
    {
        PaymentProcessorsByIdOut = paymentProcessorsByIdOut;
    }
    public PaymentProcessorsByIdOut PaymentProcessorsByIdOut { get; set; }
}
public class PaymentProcessorsByIdOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
