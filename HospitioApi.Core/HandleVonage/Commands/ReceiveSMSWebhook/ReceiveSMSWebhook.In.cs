using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonage.Commands.ReceiveSMSWebhook
{
    public class ReceiveSMSWebhookIn
    {
        public string? api_key { get; set; }
        public string? msisdn { get; set; }
        public string? to { get; set; }
        public string? messageId { get; set; }
        public string? text { get; set; }
        public string? type { get; set; }
        public string? keyword { get; set; }
        public string? timestamp { get; set; }
        public string? nonce { get; set; }
        public string? concat { get; set; }
        public string? concat_ref {get;set;}
        public string? concat_total {get;set;}
        public string? concat_part {get;set;}
        public string? data { get; set; }
        public string? udh { get; set; }
    }
}
