namespace HospitioApi.Core.Options;

public class Gr4vyApiSettingsOptions
{
    public const string Gr4vyApiSettings = "Gr4vyApiSettings"; /** String must match property in appsettings.json file */
    public string BaseUrl { get; set; } = string.Empty;
}
