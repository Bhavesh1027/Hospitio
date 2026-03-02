namespace HospitioApi.Core.Options;

public class WelComePickUpsSettingsOptions
{
    public const string WelComePickUpsSettings = "WelComePickUpsSettings";
    public string? WelComePickUps_URL { get; set; } = string.Empty;
    public string? WelComePickUps_APIKey { get; set; } = string.Empty;
}
