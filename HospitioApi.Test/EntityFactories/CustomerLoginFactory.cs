using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;
public class CustomerLoginFactory
{
    private readonly Faker<CustomerUser> _faker;
    public CustomerLoginFactory()
    {
        _faker = new Faker<CustomerUser>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.UserName, f => f.Random.String())
             .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.Password, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public CustomerUser SeedSingle(ApplicationDbContext db)
    {
        var customerUser = _faker.Generate();
        db.CustomerUsers.Add(customerUser);
        db.SaveChanges();
        return customerUser;
    }

}
