
using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Test.EntityFactories;
public class LoginFactory
{
    private readonly Faker<User> _faker;
    public LoginFactory()
    {
        _faker = new Faker<User>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.UserName, f => f.Random.String())
             .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.Password, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public User SeedSingle(ApplicationDbContext db,out string password)
    {
        var user = _faker.Generate();
        password = user.Password;
        user.Password = CryptoExtension.Encrypt(user.Password, user.Id.ToString());
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

}