using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class CustomerGuestAlertFactory
{
    private readonly Faker<CustomerGuestAlert> _faker;
    public CustomerGuestAlertFactory()
    {
        _faker = new Faker<CustomerGuestAlert>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerGuestAlert SeedSingle(ApplicationDbContext db, int? customerId)
    {
        var customerGuestAlert = _faker.Generate();
        customerGuestAlert.CustomerId = customerId;
        customerGuestAlert.OfficeHoursMsg = "test";
        db.CustomerGuestAlerts.Add(customerGuestAlert);
        db.SaveChanges();
        return customerGuestAlert;
    }

    public List<CustomerGuestAlert> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerGuestAlerts = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerGuestAlerts.AddRange(customerGuestAlerts);
        db.SaveChanges();
        return customerGuestAlerts;
    }
}
