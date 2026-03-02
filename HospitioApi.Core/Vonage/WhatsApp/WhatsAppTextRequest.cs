//using System.Text.Json.Serialization;
//using Vonage.Common.Serialization;
//using Vonage.Messages.WhatsApp;
//using Vonage.Messages;

//namespace HospitioApi.Core.Vonage.WhatsApp;

//public struct WhatsAppTextRequest : IWhatsAppMessage
//{
//    /// <inheritdoc />
//    [JsonPropertyOrder(0)]
//    [JsonConverter(typeof(EnumDescriptionJsonConverter<MessagesChannel>))]
//    public MessagesChannel Channel => MessagesChannel.WhatsApp;

//    /// <inheritdoc />
//    [JsonPropertyOrder(5)]
//    public string ClientRef { get; set; }

//    /// <inheritdoc />
//    [JsonPropertyOrder(4)]
//    public string From { get; set; }

//    /// <inheritdoc />
//    [JsonPropertyOrder(1)]
//    [JsonConverter(typeof(EnumDescriptionJsonConverter<MessagesMessageType>))]
//    public MessagesMessageType MessageType => MessagesMessageType.Text;

//    /// <summary>
//    ///     The text of message to send; limited to 4096 characters, including unicode.
//    /// </summary>
//    [JsonPropertyOrder(2)]
//    public string Text { get; set; }

//    /// <inheritdoc />
//    [JsonPropertyOrder(3)]
//    public string To { get; set; }
//}
