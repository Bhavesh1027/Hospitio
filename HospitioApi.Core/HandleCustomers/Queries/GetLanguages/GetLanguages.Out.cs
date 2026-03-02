using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguages;

public class GetLanguagesOut : BaseResponseOut
{
    public GetLanguagesOut(string message, List<LanguagesOut> languagesOut) : base(message)
    {
        LanguagesOut = languagesOut;
    }
    public List<LanguagesOut> LanguagesOut { get; set; }
}
public class LanguagesOut
{
    public string LanguageCode { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Dir { get; set; } = string.Empty;
}