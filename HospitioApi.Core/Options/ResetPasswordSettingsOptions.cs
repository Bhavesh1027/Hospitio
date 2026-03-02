namespace HospitioApi.Core.Options;

public class ResetPasswordSettingsOptions
{
    public const string ResetPasswordSettings = "ResetPasswordSettings";
    /** String must match property in appsettings.json file */

    public string EmailTemplate { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public bool IsNoReply { get; set; }
    public int ExpirationHours { get; set; }
}
