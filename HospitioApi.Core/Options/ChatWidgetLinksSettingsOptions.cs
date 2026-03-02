namespace HospitioApi.Core.Options;

public class ChatWidgetLinksSettingsOptions
{
    public const string ChatWidgetLinksSettings = "ChatWidgetLinksSettings"; /** String must match property in appsettings.json file */
    public string ChatWidget { get; set; } = string.Empty;
    public string ChatWidgetJs { get; set; } = string.Empty;
}
