namespace HospitioApi.Core.Options
{
    public class CustomerGuestCheckInFormSendPDftoEmailSettingsOptions
    {
        public const string CustomerGuestCheckInFormSendPDftoEmailSettings = "CustomerGuestCheckInFormSendPdftoEmailSettings"; 
        public string Body { get;set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string IsNoReply { get; set; } = string.Empty;
        public string ExpirationHours { get; set; } = string.Empty;

    }
}
