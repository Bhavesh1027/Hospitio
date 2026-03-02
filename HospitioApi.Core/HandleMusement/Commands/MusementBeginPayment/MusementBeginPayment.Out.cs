using HospitioApi.Core.HandleTransactions.Commands.CaptureTransaction;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Commands.MusementBeginPaymentStripe;

public class MusementBeginPaymentOut : BaseResponseOut
{
    public MusementBeginPaymentOut(string message, string musementBeginPaymentResponseOut) : base(message)
    {
        musementBeginPaymentResponse = musementBeginPaymentResponseOut;
    }
    public string musementBeginPaymentResponse { get; set; }
}

public class musementBeginPaymentClass
{
    public string? gateway { get; set; }
    public ThreeDSecure? ThreeDSecure { get; set; }
    public string? reason { get; set; }
}
public class ThreeDSecure
{
    public object? payload { get; set; }
    public string? payment_intent_client_secret { get; set; }
    public string? type { get; set; }
    public string? url { get; set; }
}