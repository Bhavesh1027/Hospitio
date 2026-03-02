using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessors;

public class GetPaymentProcessorsOut : BaseResponseOut
{
    public GetPaymentProcessorsOut(string message, List<PaymentProcessorsOut> paymentProcessorsOut) : base(message)
    {
        PaymentProcessorsOut = paymentProcessorsOut;
    }
    public List<PaymentProcessorsOut> PaymentProcessorsOut { get; set; } = new();
}

public class PaymentProcessorsOut
{
    public int Id { get; set; }
    public string GRCategory { get; set; } = string.Empty;

    public string GRGroup { get; set; } = string.Empty;

    public string GRID { get; set; } = string.Empty;

    public string GRIcon { get; set; } = string.Empty;

    public string GRName { get; set; } = string.Empty;
}
