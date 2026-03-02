using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestAppHousekeepingCategoryFactory
{
    private readonly Faker<CustomerGuestAppHousekeepingCategory> _faker;
    public CustomerGuestAppHousekeepingCategoryFactory()
    {
        _faker = new Faker<CustomerGuestAppHousekeepingCategory>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }
    public CustomerGuestAppHousekeepingCategory SeedSingle(ApplicationDbContext db, int? CustomerGuestAppBuilderId, int? CustomerId)
    {
        var customers = _faker.Generate();
        customers.CustomerId = CustomerId;
        customers.CustomerGuestAppBuilderId = CustomerGuestAppBuilderId;
        db.CustomerGuestAppHousekeepingCategories.Add(customers);
        db.SaveChanges();
        return customers;
    }
    public List<CustomerGuestAppHousekeepingCategory> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuests = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAppHousekeepingCategories.AddRange(customerGuests);
        db.SaveChanges();
        return customerGuests;
    }
}
