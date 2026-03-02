namespace HospitioApi.Core.Options;

public class EndpointSettings
{
    public const string CenturionEndpointSettings = "Endpoints";/** String must match property in appsettings.json file */
    public ApiSettings Centurion { get; set; }
}
public class ApiSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
}