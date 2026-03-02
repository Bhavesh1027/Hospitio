using Bogus;
using HospitioApi.Data.Models;
using HospitioApi.Data;

namespace HospitioApi.Test.EntityFactories;

public class UserLevelFactory
{
    private readonly Faker<UserLevel> _faker;
    public UserLevelFactory()
    {
        _faker = new Faker<UserLevel>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 5))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.LevelName, f => f.Lorem.Word());
    }

    public List<UserLevel> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var userLevels = _faker.Generate(numberOfEntitiesToCreate);
        db.UserLevels.AddRange(userLevels);
        db.SaveChanges();
        return userLevels;
    }

    public UserLevel SeedSingle(ApplicationDbContext db, int? id = null)
    {
        var userLevel = _faker.Generate();

        if (id != null)
        {
            userLevel.Id = id ?? 0;
        }

        db.UserLevels.Add(userLevel);
        db.SaveChanges();
        return userLevel;
    }
}
