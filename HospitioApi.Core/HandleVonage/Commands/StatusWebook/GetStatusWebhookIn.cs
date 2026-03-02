namespace HospitioApi.Core.HandleVonage.Commands.StatusWebook
{
    public class GetStatusWebhookIn
    {
        public string message_uuid {  get; set; }
        public string to {  get; set; }
        public string from {  get; set; }
        public string timestamp {  get; set; }
        public string status { get; set; }
        public Error error { get; set; }
        public Usage usage { get; set; }
        public string client_ref { get; set; }
        public string channel { get; set; }
    }

    public class Error
    {
        public string type { get; set; }
        public int title { get; set; }
        public string detail { get; set; }
        public string instance { get; set; }
    }

    public class Usage
    {
        public string currency { get; set; }
        public string price { get; set; }
    }
}

