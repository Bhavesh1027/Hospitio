using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerPropertyServiceFactory
{
    private readonly Faker<CustomerPropertyService> _faker;
    public CustomerPropertyServiceFactory()
    {
        _faker = new Faker<CustomerPropertyService>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerPropertyService SeedSingle(ApplicationDbContext db)
    {
        var customerProperty = _faker.Generate();
        db.CustomerPropertyServices.Add(customerProperty);
        db.SaveChanges();
        return customerProperty;
    }
    public List<CustomerPropertyService> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerPropertyServices = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerPropertyServices.AddRange(customerPropertyServices);
        db.SaveChanges();
        return customerPropertyServices;
    }

}
