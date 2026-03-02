using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestAppBuilderFactory
{
    private readonly Faker<CustomerGuestAppBuilder> _faker;
    public CustomerGuestAppBuilderFactory()
    {
        _faker = new Faker<CustomerGuestAppBuilder>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerGuestAppBuilder SeedSingle(ApplicationDbContext db, int? customerRoomNameId = null)
    {
        var customerAppBuilder = _faker.Generate();
        customerAppBuilder.CustomerRoomNameId = customerRoomNameId;
        db.CustomerGuestAppBuilders.Add(customerAppBuilder);
        db.SaveChanges();
        return customerAppBuilder;
    }

    public CustomerGuestAppBuilder Update(ApplicationDbContext db, CustomerGuestAppBuilder customer)
    {
        db.CustomerGuestAppBuilders.Update(customer);
        return customer;
    }

    public List<CustomerGuestAppBuilder> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuestAppBuilders = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppBuilders.AddRange(customerGuestAppBuilders);
        db.SaveChanges();
        return customerGuestAppBuilders;
    }
}
