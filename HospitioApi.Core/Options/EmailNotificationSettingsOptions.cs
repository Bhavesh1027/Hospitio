

namespace HospitioApi.Core.Options;

public class EmailNotificationSettingsOptions
{
    public const string EmailNotificationSettings = "EmailNotificationSettings"; /** String must match property in appsettings.json file */

    public string SendGridAPIKey { get; set; } = string.Empty;
    public string EmailDefaultFrom { get; set; } = string.Empty;
    public string EmailNoReply { get; set; } = string.Empty;
    public string EmailInternalListCredentialAdded { get; set; } = string.Empty;
    public string EmailInternalListClinicianAccountCreated { get; set; } = string.Empty;
    public string EmailMonitor { get; set; } = string.Empty;
}