namespace HospitioApi.Core.Options;

public class MusementSettingsOptions
{
    public const string MusementSettings = "MusementSettings";

    public string? client_id { get; set; } = string.Empty;
    public string? client_secret { get; set; } = string.Empty;
    public string? grant_type { get; set; } = string.Empty;
    public string? musement_url { get; set; } = string.Empty;
}
