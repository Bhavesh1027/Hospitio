using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonage.Commands.ReceiveSMSWebhook
{
    public class ReceiveSMSWebhookOut : BaseResponseOut
    {
        public ReceiveSMSWebhookOut(string message) : base(message) { }
    }
}
