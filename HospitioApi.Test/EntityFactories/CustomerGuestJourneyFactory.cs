using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestJourneyFactory
{
    private readonly Faker<CustomerGuestJourny> _faker;
    public CustomerGuestJourneyFactory()
    {
        _faker = new Faker<CustomerGuestJourny>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.JourneyStep, Convert.ToByte(1))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.VonageTemplateId, f => f.Lorem.Word())
            .RuleFor(m => m.VonageTemplateStatus, f => f.Lorem.Word())
            .RuleFor(m => m.Name, f => f.Name.FullName())
            .RuleFor(m => m.WhatsappTemplateName, f => f.Lorem.Word());
    }

    public CustomerGuestJourny SeedSingle(ApplicationDbContext db,int CustomerId)
    {
        var customerGuestJourny = _faker.Generate();
        customerGuestJourny.CutomerId = CustomerId;
        db.CustomerGuestJournies.Add(customerGuestJourny);
        db.SaveChanges();
        return customerGuestJourny;
    }
    public List<CustomerGuestJourny> SeedMany(ApplicationDbContext db,int MessageId,int CustomerId, int numberOfEntitiesToCreate)
    {
        var guestJournies = Generate(MessageId, CustomerId,numberOfEntitiesToCreate);
        db.CustomerGuestJournies.AddRange(guestJournies);
        db.SaveChanges();
        return guestJournies;
    }
    public CustomerGuestJourny Update(ApplicationDbContext db, CustomerGuestJourny customerGuestJourny)
    {
        db.CustomerGuestJournies.Update(customerGuestJourny);
        return customerGuestJourny;
    }
    private List<CustomerGuestJourny> Generate(int MessageId, int CustomerId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.CutomerId, CustomerId)
                   .RuleFor(m => m.GuestJourneyMessagesTemplateId, MessageId);
        return faker.Generate(numberOfEntitiesCreate);
    }
}
