using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerPropertyExtraDetailFactory
{
    private readonly Faker<CustomerPropertyExtraDetails> _faker;
    public CustomerPropertyExtraDetailFactory()
    {
        _faker = new Faker<CustomerPropertyExtraDetails>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerPropertyExtraDetails SeedSingle(ApplicationDbContext db)
    {
        var customerProperty = _faker.Generate();
        db.CustomerPropertyExtraDetails.Add(customerProperty);
        db.SaveChanges();
        return customerProperty;
    }
}
