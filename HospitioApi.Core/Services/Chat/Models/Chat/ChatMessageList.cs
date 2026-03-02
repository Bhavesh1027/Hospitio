using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Chat.Models.Chat
{
    public class ChatMessageList
    {
        public int? ChannelId { get; set; }
        public string MessageType { get; set; }
        public byte? MessageSender { get; set; }
        public byte? Source { get; set; }
        public byte? MsgReqType { get; set; }
        public string Attachment { get; set; }
        public int? RequestId { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public string TranslateMessage { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsRead { get; set; }
        public int? Id { get; set; }
        public int? MessageSenderId { get; set; }
        public string UserType { get; set; }
        public byte? RequestType { get; set; }
        public byte? RequestStatus { get; set; }
    }
}
