using Vonage.Messages;

namespace HospitioApi.Core.Vonage.Sms;

public class SmsRequest : MessageRequestBase
{
    public override MessagesChannel Channel => MessagesChannel.SMS;

    public override MessagesMessageType MessageType => MessagesMessageType.Text;

    public string Text { get; set; } = string.Empty;
}
