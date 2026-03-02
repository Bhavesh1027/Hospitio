using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class CustomerFactory
{
    private readonly Faker<Customer> _faker;
    public CustomerFactory()
    {
        _faker = new Faker<Customer>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.BusinessName, "Test")
            .RuleFor(m => m.CurrencyCode, f => f.Finance.Currency().Code)
            .RuleFor(m => m.Guid, f => f.Random.Guid())
            .RuleFor(m => m.PhoneCountry, f => f.Address.CountryCode())
            .RuleFor(m => m.PhoneNumber, f => f.Phone.PhoneNumber("###########"))
            .RuleFor(m => m.ClientDoamin, f => f.Internet.DomainName())
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.SmsTitle, f => f.Lorem.Sentence())
            .RuleFor(m => m.Messenger, f => f.Company.CatchPhrase());
    }

    public Customer SeedSingle(ApplicationDbContext db, int? productId = null, int? bussienssTypeId = null)
    {
        var customers = _faker.Generate();
        customers.ProductId = productId;
        customers.BusinessTypeId = bussienssTypeId;
        db.Customers.Add(customers);
        db.SaveChanges();
        return customers;
    }

    public Customer Update(ApplicationDbContext db, Customer customer)
    {
        db.Customers.Update(customer);
        db.SaveChanges();
        return customer;
    }

    public List<Customer> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var customers = _faker.Generate(numberOfEntitiesToCreate);
        db.Customers.AddRange(customers);
        db.SaveChanges();
        return customers;
    }
}
