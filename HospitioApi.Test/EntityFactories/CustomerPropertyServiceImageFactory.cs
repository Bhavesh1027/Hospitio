using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerPropertyServiceImageFactory
{
    private readonly Faker<CustomerPropertyServiceImage> _faker;
    public CustomerPropertyServiceImageFactory()
    {
        _faker = new Faker<CustomerPropertyServiceImage>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerPropertyServiceImage SeedSingle(ApplicationDbContext db, int? PropertyServiceId)
    {
        var customerProperty = _faker.Generate();
        customerProperty.CustomerPropertyServiceId = PropertyServiceId;
        db.CustomerPropertyServiceImages.Add(customerProperty);
        db.SaveChanges();
        return customerProperty;
    }
}
