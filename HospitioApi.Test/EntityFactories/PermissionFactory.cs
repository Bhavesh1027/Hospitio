using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class PermissionFactory
{
    private readonly Faker<Permission> _faker;
    public PermissionFactory()
    {
        _faker = new Faker<Permission>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public Permission SeedSingle(ApplicationDbContext db)
    {
        var permission = _faker.Generate();
        db.Permissions.Add(permission);
        db.SaveChanges();
        return permission;
    }
    public List<Permission> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var permissions = _faker.Generate(numberOfEntitiesToCreate);
        db.Permissions.AddRange(permissions);
        db.SaveChanges();
        return permissions;
    }

    public Permission Update(ApplicationDbContext db, Permission permission)
    {
        db.Permissions.Update(permission);
        return permission;
    }
    public Permission Remove(ApplicationDbContext db, Permission permission)
    {
        db.Permissions.Remove(permission);

        return permission;
    }
}
