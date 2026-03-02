namespace HospitioApi.Core.Options;

public class VonageSettingsOptions
{
    public const string VonageSettings = "VonageSettings";

    public string APIKey { get; set; } = string.Empty;
    public string APISecret { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string AppPrivatKey { get; set;} = string.Empty;
    public string WABAId { get; set; } = string.Empty;  
    public string HospitioCallBackBaseURL { get; set; } = string.Empty;  

}
