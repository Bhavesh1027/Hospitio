using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessorById;

public class GetHospitioPaymentProcessorByIdOut : BaseResponseOut
{
    public GetHospitioPaymentProcessorByIdOut(string message, HospitioPaymentProcessorByIdOut hospitioPaymentProcessorByIdOut) : base(message)
    {
        HospitioPaymentProcessorByIdOut = hospitioPaymentProcessorByIdOut;
    }
    public HospitioPaymentProcessorByIdOut HospitioPaymentProcessorByIdOut { get; set; }
}

public class HospitioPaymentProcessorByIdOut
{
    public int Id { get; set; }
    public int? PaymentProcessorId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
