namespace HospitioApi.Core.Services.LanguageTranslator.Models;

public class LanguageDetail
{
    public detectedLanguage detectedLanguage { get; set; } = new();
    public List<translations> translations { get; set; } = new();
}

public class detectedLanguage
{
    public string language { get; set; } = string.Empty;
    public decimal score { get; set; }
}

public class translations
{
    public string text { get; set; } = string.Empty;
    public string to { get; set; } = string.Empty;
}