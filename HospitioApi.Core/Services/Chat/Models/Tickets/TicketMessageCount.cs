using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Chat.Models.Tickets
{
    public class TicketMessageCount
    {
        public int TicketId { get; set; }
        public int TotalUnReadCount { get; set; }
    }
}
