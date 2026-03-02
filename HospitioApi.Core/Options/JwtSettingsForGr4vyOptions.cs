using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Options;

public class JwtSettingsForGr4vyOptions
{
    public const string JwtSettingsForGr4vy = "JwtSettingsForGr4vy"; /** String must match property in appsettings.json file */
    public string JwtPemPrivateKey { get; set; } = string.Empty;
    public string JwtKidKey { get; set; } = string.Empty;
    public int JwtValidForMinutes { get; set; } = default;
}
