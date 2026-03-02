using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.CreateVonageRecordsReport
{
    public class CreateVonageRecordsReportOut : BaseResponseOut
    {
        public CreateVonageRecordsReportOut(string message, VonageRecordsReportOut recordsOut) : base(message)
        {
            vonageRecordsReportsOut = recordsOut;
        }
        public VonageRecordsReportOut vonageRecordsReportsOut { get; set; }

    }
    public class VonageRecordsReportOut
    {
        public string request_id { get; set; }
        public string request_status { get; set; }
        public DateTime receive_time { get; set; }
        public Links _links { get; set; }
        public string product { get; set; }
        public string account_id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public bool include_subaccounts { get; set; }
        public string callback_url { get; set; }
        public bool include_message { get; set; }
        public string direction { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }
}
