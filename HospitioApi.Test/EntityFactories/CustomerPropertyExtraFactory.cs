using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerPropertyExtraFactory
{
    private readonly Faker<CustomerPropertyExtra> _faker;
    public CustomerPropertyExtraFactory()
    {
        _faker = new Faker<CustomerPropertyExtra>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerPropertyExtra SeedSingle(ApplicationDbContext db)
    {
        var customerProperty = _faker.Generate();
        db.CustomerPropertyExtras.Add(customerProperty);
        db.SaveChanges();
        return customerProperty;
    }
    public List<CustomerPropertyExtra> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerPropertyExtras = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerPropertyExtras.AddRange(customerPropertyExtras);
        db.SaveChanges();
        return customerPropertyExtras;
    }
}
