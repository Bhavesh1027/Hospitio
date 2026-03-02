using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;


namespace HospitioApi.Test.EntityFactories;
public class HospitioOnboardingsFactory
{
    private readonly Faker<HospitioOnboarding> _faker;
    public HospitioOnboardingsFactory()
    {
        _faker = new Faker<HospitioOnboarding>()
            .RuleFor(m => m.Id, f => f.Random.Int(2, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true)
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.WhatsappCountry, f => f.Lorem.Word())
            .RuleFor(m => m.WhatsappNumber, f => f.Lorem.Word())
            .RuleFor(m => m.ViberCountry,f => f.Lorem.Word())
            .RuleFor(m => m.TelegramCounty,f => f.Lorem.Word())
            .RuleFor(m => m.TelegramNumber, f => f.Lorem.Word())
            .RuleFor(m => m.PhoneCountry, f => f.Lorem.Word())
            .RuleFor(m => m.PhoneNumber, f => f.Lorem.Word())
            .RuleFor(m => m.SmsTitle, f => f.Lorem.Word())
            .RuleFor(m => m.Messenger, f => f.Lorem.Word())
            .RuleFor(m => m.IncomingTranslationLangage, f => f.Lorem.Word())
            .RuleFor(m => m.NoTranslateWords, f => f.Lorem.Word())
            .RuleFor(m => m.Cname, f => f.Lorem.Word());
    }

    public HospitioOnboarding SeedSingle(ApplicationDbContext db)
    {
        var hospitioOnboarding = _faker.Generate();
        db.HospitioOnboardings.Add(hospitioOnboarding);
        db.SaveChanges();
        return hospitioOnboarding;
    }
}

