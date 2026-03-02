namespace HospitioApi.Core.Options;

public class ThirdPartyAPIUrlOptions
{
    public const string ThirdPartyAPIUrl = "ThirdPartyAPIUrl"; /** String must match property in appsettings.json file */

    public string Languages { get; set; } = string.Empty;
}
