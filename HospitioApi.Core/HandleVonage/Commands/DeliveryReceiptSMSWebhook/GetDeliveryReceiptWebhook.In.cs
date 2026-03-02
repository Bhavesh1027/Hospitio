using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonage.Commands.DeliveryReceiptSMSWebhook
{
    public class GetDeliveryReceiptWebhookIn
    {
        public string? msisdn { get; set; }  
        public string? to { get; set; }  
        public string? network_code { get; set; }  
        public string? messageId { get; set; }  
        public string? price { get; set; }  
        public string? status { get; set; }  
        public string? scts { get; set; }  
        public string? err_code { get; set; }  
        public string? api_key { get; set; }  
        public string? message_timestamp { get; set; }  
    }
}
