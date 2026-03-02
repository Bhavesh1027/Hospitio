using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class GuestJourneyMessagesTemplatesFactory
{
    private readonly Faker<GuestJourneyMessagesTemplate> _faker;
    public GuestJourneyMessagesTemplatesFactory()
    {
        _faker = new Faker<GuestJourneyMessagesTemplate>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.Name, "Test")
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.VonageTemplateId, f => f.Lorem.Word())
            .RuleFor(m => m.VonageTemplateStatus, f => f.Lorem.Word())
            .RuleFor(m => m.WhatsappTemplateName, f => f.Lorem.Word());

    }

    public GuestJourneyMessagesTemplate SeedSingle(ApplicationDbContext db)
    {
        var GuestJournyMessage = _faker.Generate();
        db.GuestJourneyMessagesTemplates.Add(GuestJournyMessage);
        db.SaveChanges();
        return GuestJournyMessage;
    }

    public List<GuestJourneyMessagesTemplate> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var guestJourneyMessages = _faker.Generate(numberOfEntitiesToCreate);
        db.GuestJourneyMessagesTemplates.AddRange(guestJourneyMessages);
        db.SaveChanges();
        return guestJourneyMessages;
    }
    public GuestJourneyMessagesTemplate Update(ApplicationDbContext db, GuestJourneyMessagesTemplate guestJourneyMessagesTemplate)
    {
        db.GuestJourneyMessagesTemplates.Update(guestJourneyMessagesTemplate);
        return guestJourneyMessagesTemplate;
    }
}
