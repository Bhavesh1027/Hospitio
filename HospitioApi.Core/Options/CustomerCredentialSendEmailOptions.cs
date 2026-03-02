namespace HospitioApi.Core.Options
{
    public class CustomerCredentialSendEmailOptions
    {
        public const string CustomerCredentialEmailSettings = "CustomerCredentialEmailSettings";

        public string EmailTemplate { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public bool IsNoReply { get; set; }
        public int ExpirationHours { get; set; }
        public string CustomerLoginPageURL { get; set; }

    }
}
