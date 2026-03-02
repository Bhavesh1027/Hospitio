namespace HospitioApi.Core.Options;

public class JwtSettingsOptions
{
    public const string JwtSettings = "JwtSettings"; /** String must match property in appsettings.json file */

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string JwtPemPrivateKey { get; set; } = string.Empty;
    public string JwtPemPublicKey { get; set; } = string.Empty;
    public int JwtValidForMinutes { get; set; } = default;
    public int RefreshTokenValidForHours { get; set; } = default;
}
