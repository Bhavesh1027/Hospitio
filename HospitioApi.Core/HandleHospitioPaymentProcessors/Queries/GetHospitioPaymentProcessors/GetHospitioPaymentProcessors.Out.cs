using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessors;

public class GetHospitioPaymentProcessorsOut : BaseResponseOut
{
    public GetHospitioPaymentProcessorsOut(string message, List<HospitioPaymentProcessorsOut> hospitioPaymentProcessorsOut) : base(message)
    {
        HospitioPaymentProcessorsOut = hospitioPaymentProcessorsOut;
    }
    public List<HospitioPaymentProcessorsOut> HospitioPaymentProcessorsOut { get; set; } = new();
}

public class HospitioPaymentProcessorsOut
{
    public int Id { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
