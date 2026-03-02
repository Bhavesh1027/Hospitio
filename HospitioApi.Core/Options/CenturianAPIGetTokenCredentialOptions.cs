namespace HospitioApi.Core.Options
{
    public class CenturionAPIGetTokenCredentialOptions
    {
        public const string CenturionAPITokenCredentials = "CenturionAPITokenCredentials";

        public string? email { get; set; } = string.Empty;
        public string? password { get; set; } = string.Empty;

    }
}