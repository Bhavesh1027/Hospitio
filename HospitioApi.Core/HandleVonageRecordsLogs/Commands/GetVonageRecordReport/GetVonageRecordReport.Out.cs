using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordReport
{
    public class GetVonageRecordReportOut : BaseResponseOut
    {
        public GetVonageRecordReportOut(string message, VonageRecordReports vonageRecordReports) : base(message)
        {
            this.vonageRecordReports = vonageRecordReports;
        }
        public VonageRecordReports vonageRecordReports { get; set; }
    }
    public class Self
    {
        public string? href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Record
    {
        public string account_id { get; set; }
        public string message_id { get; set; }
        public string client_ref { get; set; }
        public string concatenated { get; set; }
        public string direction { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string network { get; set; }
        public string network_name { get; set; }
        public string country { get; set; }
        public string country_name { get; set; }
        public DateTime date_received { get; set; }
        public DateTime date_finalized { get; set; }
        public string latency { get; set; }
        public string status { get; set; }
        public string error_code { get; set; }
        public string error_code_description { get; set; }
        public string currency { get; set; }
        public string total_price { get; set; }
        public string message_body { get; set; }
    }

    public class VonageRecordReports
    {
        public Links _links { get; set; }
        public string request_id { get; set; }
        public string request_status { get; set; }
        public DateTime received_at { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string account_id { get; set; }
        public string ids_not_found { get; set; }
        public string product { get; set; }
        public string direction { get; set; }
        public string include_message { get; set; }
        public string show_concatenated { get; set; }
        public int items_count { get; set; }
        public List<Record> records { get; set; }
        public string FileURI { get; set; }
    }

}

