namespace HospitioApi.Core.Services.Language;

public interface ILanguageService
{
    Task<string> GetSupportedLanguageAsync(CancellationToken cancellationToken);
}
