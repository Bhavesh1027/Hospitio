namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.CreateVonageRecordsReport
{
    public class CreateVonageRecordsReportIn
    {
        public string? Product { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public string? Direction { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public int CustomerId { get; set; }
        //public string? Client_ref { get; set; }
        //public string? Account_ref { get; set; }
    }
    public class VonageRecordReportIn
    {
        public string? Product { get; set; }
        public string? Account_id { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public bool? Include_subaccounts { get; set; }
        public string? Callback_url { get; set; }
        public string? Direction { get; set; }
        public string? Status { get; set; }
        public bool Include_message { get; set; } = true;
        public bool Show_concatenated { get; set; } = false;
        public string? Network { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        //public string? Client_ref { get; set; }
        //public string? Account_ref { get; set; }
    }
}
