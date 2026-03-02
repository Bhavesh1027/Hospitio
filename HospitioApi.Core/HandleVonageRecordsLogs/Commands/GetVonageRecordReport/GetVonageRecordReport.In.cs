using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordReport
{
    public class GetVonageRecordReportIn
    {
        public int CustomerId { get; set; }
        public string Product { get; set; }
        public string Direction { get; set; }
        //public string id { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public bool Include_message { get; set; }
        public bool Show_concatenated { get; set; }
        public string Status { get; set; }
    }
}
