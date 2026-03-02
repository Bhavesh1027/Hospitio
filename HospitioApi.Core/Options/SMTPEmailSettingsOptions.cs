namespace HospitioApi.Core.Options;

public class SMTPEmailSettingsOptions
{
    public const string SMTPEmailSettings = "SMTPEmailSettings";/** String must match property in appsettings.json file */
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public string FromEmail { get; set; } = string.Empty;
}
