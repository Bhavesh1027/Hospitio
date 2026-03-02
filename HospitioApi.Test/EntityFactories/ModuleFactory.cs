using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class ModuleFactory
{
    private readonly Faker<Module> _faker;
    public ModuleFactory()
    {
        _faker = new Faker<Module>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent())
            .RuleFor(m => m.IsActive, true);
    }

    public Module SeedSingle(ApplicationDbContext db)
    {
        var module = _faker.Generate();
        db.Modules.Add(module);
        db.SaveChanges();
        return module;
    }

    public List<Module> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var modules = _faker.Generate(numberOfEntitiesToCreate);
        db.Modules.AddRange(modules);
        db.SaveChanges();
        return modules;
    }
}
