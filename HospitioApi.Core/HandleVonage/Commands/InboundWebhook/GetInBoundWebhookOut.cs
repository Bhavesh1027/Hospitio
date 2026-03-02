using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonage.Commands.InboundWebhook;

public class GetInBoundWebhookOut : BaseResponseOut
{
    public GetInBoundWebhookOut(string message) : base(message) { }
}
