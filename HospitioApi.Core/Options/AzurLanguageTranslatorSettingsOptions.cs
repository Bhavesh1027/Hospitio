namespace HospitioApi.Core.Options;

public class AzurLanguageTranslatorSettingsOptions
{
    public const string AzurLanguageTranslatorSettings = "AzurLanguageTranslatorSettings";
    public string AzurKey { get; set; } = string.Empty;
    public string EndPoint { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}
