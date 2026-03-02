using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonage.Commands.DeliveryReceiptSMSWebhook
{
    public class GetDeliveryReceiptWebhookOut : BaseResponseOut
    {
        public GetDeliveryReceiptWebhookOut(string message) : base(message) { }
    }
}
