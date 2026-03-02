namespace HospitioApi.Core.Services.LanguageTranslator.Models;

public class LanguageTranslate
{
    //public string detectedLanguageCode { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
    //public string convertedLanguageCode { get; set; } = string.Empty;
    public int channelMessageId { get; set; }
}
