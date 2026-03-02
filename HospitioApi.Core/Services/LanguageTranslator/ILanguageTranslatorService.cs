using HospitioApi.Core.Services.LanguageTranslator.Models;
using HospitioApi.Data;

namespace HospitioApi.Core.Services.LanguageTranslator;

public interface ILanguageTranslatorService
{
   
    public Task<LanguageTranslate> GetLanguageTranslatedAsync(ApplicationDbContext _db, int userType, int customerId, int channelMessageId, string message);

    public Task<LanguageTranslate> GetGuestTranslatedLanguage(ApplicationDbContext _db, int userType, int guestId, int channelMessageId, string message, string translatelanguage);

}
