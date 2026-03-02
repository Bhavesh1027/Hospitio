using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;


namespace HospitioApi.Test.EntityFactories;
public class CustomerStaffAlertFactory
{
    private readonly Faker<CustomerStaffAlert> _faker;
    public CustomerStaffAlertFactory()
    {
        _faker = new Faker<CustomerStaffAlert>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.Name, f => f.Random.String())
            .RuleFor(m => m.PhoneCountry, f => f.Random.String())
            .RuleFor(m => m.Platfrom, f => f.Random.String())
            .RuleFor(m => m.PhoneCountry, f => f.Random.String())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerStaffAlert SeedSingle(ApplicationDbContext db, int? customerId)
    {
        var customerStaffAlert = _faker.Generate();
        customerStaffAlert.CustomerId = customerId;
        db.CustomerStaffAlerts.Add(customerStaffAlert);
        db.SaveChanges();
        return customerStaffAlert;
    }
    public List<CustomerStaffAlert> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customerStaffAlerts = _faker.Generate(numberOfEntitiesToCreate);
        db.CustomerStaffAlerts.AddRange(customerStaffAlerts);
        db.SaveChanges();
        return customerStaffAlerts;
    }
}