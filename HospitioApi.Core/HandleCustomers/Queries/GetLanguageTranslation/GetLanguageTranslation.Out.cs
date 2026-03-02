using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;

public class GetLanguageTranslationOut : BaseResponseOut
{
    public GetLanguageTranslationOut(string message, LanguageTranslationOut languageTranslationOut) : base(message)
    {
        LanguageTranslationOut = languageTranslationOut;
    }
    public LanguageTranslationOut LanguageTranslationOut { get; set; }
}

public class LanguageTranslationOut
{
    public string detectedLanguageCode { get; set; } = string.Empty;
    public string convertedMessage { get; set; } = string.Empty;
    public string convertedLanguageCode { get; set; } = string.Empty;
}
