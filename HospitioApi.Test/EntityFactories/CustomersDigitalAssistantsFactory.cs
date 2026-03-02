using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomersDigitalAssistantsFactory
{
    private readonly Faker<CustomerDigitalAssistant> _faker;

    public CustomersDigitalAssistantsFactory()
    {
        _faker = new Faker<CustomerDigitalAssistant>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.Name,"Test")
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(M => M.UpdateAt, f => f.Date.Recent());
    }

    public CustomerDigitalAssistant SeedSingle(ApplicationDbContext db,int? CustomerId)
    {
        var digitalassistant = _faker.Generate();
        digitalassistant.CustomerId = CustomerId;
        db.CustomerDigitalAssistants.Add(digitalassistant);
        db.SaveChanges();
        return digitalassistant;
    }
    public List<CustomerDigitalAssistant> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var department = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerDigitalAssistants.AddRange(department);
        db.SaveChanges();
        return department;
    }
    public CustomerDigitalAssistant Update(ApplicationDbContext db, CustomerDigitalAssistant customerDigitalAssistant)
    {
        db.CustomerDigitalAssistants.Update(customerDigitalAssistant);
        return customerDigitalAssistant;
    }
}
