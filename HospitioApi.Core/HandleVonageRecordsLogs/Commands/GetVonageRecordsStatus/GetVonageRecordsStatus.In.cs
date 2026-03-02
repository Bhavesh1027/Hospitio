using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordsStatus
{
    public class GetVonageRecordsStatusIn
    {
        public string request_id { get; set; }
        public string request_status { get; set; }
        public string product { get; set; }
        public string account_id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public bool include_subaccounts { get; set; }
        public string callback_url { get; set; }
        public DateTime receive_time { get; set; }
        public DateTime start_time { get; set; }
        public Links _links { get; set; }
        public int items_count { get; set; }
        public string direction { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string id { get; set; }
        public bool include_message { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public DownloadReport download_report { get; set; }
    }
    public class Self
    {
        public string href { get; set; }
    }
    public class DownloadReport
    {
        public string href { get; set; }
    }

}
