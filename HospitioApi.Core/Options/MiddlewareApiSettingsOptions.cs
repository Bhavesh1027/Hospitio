namespace HospitioApi.Core.Options;

public class MiddlewareApiSettingsOptions
{
    public const string MiddlewareApiSettings = "MiddlewareApiSettings"; /** String must match property in appsettings.json file */
    public string BaseUrl { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
