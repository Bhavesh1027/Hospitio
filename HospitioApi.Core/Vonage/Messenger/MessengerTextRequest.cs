using Vonage.Messages.Messenger;
using Vonage.Messages;

namespace HospitioApi.Core.Vonage.Messenger;

public class MessengerTextRequest : MessageRequestBase
{
    public override MessagesChannel Channel => MessagesChannel.Messenger;

    public MessengerRequestData Data { get; set; } = new();
    public override MessagesMessageType MessageType => MessagesMessageType.Text;

    /// <summary>
    ///     The text of message to send; limited to 640 characters, including unicode.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}
