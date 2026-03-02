namespace HospitioApi.Core.Options
{
    public class VonageTemplateEmailSettingsOptions
    {
        public const string VonageTemplateEmailSettings = "VonageTemplateEmailSettings";
        public string EmailTemplate { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public bool IsNoReply { get; set; }
        public int ExpirationHours { get; set; }
    }
}
