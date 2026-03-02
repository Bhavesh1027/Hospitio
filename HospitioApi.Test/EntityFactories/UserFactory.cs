using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class UserFactory
{
    private readonly Faker<User> _faker;
    public UserFactory()
    {
        _faker = new Faker<User>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true)
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.FirstName, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.LastName, f => f.Random.Guid().ToString().ToLower())
            .RuleFor(m => m.UserName, f => f.Internet.UserName());
    }

    public User SeedSingle(ApplicationDbContext db, int? departmentId = null, int? userLevelId = null, int? groupId = null)
    {
        var user = _faker.Generate();
        user.DepartmentId = departmentId;
        user.UserLevelId = userLevelId;
        user.GroupId = groupId;
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }
    public User SeedSingle(ApplicationDbContext db,UserLevel userLevel)
    {
        var user = _faker.Generate();
        if (userLevel != null)
        {
            user.UserLevel = userLevel;
        }
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

    public List<User> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var users = _faker.Generate(numberOfEntitiesToCreate);
        db.Users.AddRange(users);
        db.SaveChanges();
        return users;
    }
}
