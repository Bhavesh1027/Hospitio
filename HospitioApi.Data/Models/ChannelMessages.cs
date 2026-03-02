using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public partial class ChannelMessages :Auditable
    {
        public int  ChannelId {get;set;}

        //public int ChannelUserId { get; set; }

        public string MessageType { get; set; }

        /// <summary>
        ///   1: Hospitio ,2: Customer, 3: Guest

        /// </summary>


        public byte? MessageSender { get; set; }


        /// <summary>
        ///  1: Web Chat,2: Web Chat Request, 3: whatsapp,  4: sms,   5: viber

        /// </summary>
        public byte? Source { get; set; }

        /// <summary>
        ///   1: message,    2: request , 3: journey message

        ///  </summary>
        public byte? MsgReqType { get; set; }

        [MaxLength(300)]
        public string? Attachment { get; set; }
        public int? RequestId { get; set; }
        // Common = 1, EnhanceYourStay = 2
        public byte? RequestType { get; set; }
        public bool IsRead { get; set; }
        public int? MessageSenderId { get; set; }
        public string? MessageUuid { get; set; }
        public string? VonageStatus { get; set; }
        public int? EnhanceStayItemsGuestRequestId { get; set; }
        public virtual Channels? Channels { get; set; }

        public virtual ChannelUsers? ChannelUsers { get; set; }
        public virtual GuestRequest? GuestRequests { get; set; }
        public virtual EnhanceStayItemsGuestRequest? EnhanceStayItemsGuestRequest { get; set; }

    }
}
