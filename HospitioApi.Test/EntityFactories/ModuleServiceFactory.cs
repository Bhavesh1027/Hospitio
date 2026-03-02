using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class ModuleServiceFactory
{
    private readonly Faker<ModuleService> _faker;
    public ModuleServiceFactory()
    {
        _faker = new Faker<ModuleService>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public ModuleService SeedSingle(ApplicationDbContext db, Module module)
    {
        var moduleService = _faker.Generate();
        moduleService.Module = module;
        db.ModuleServices.Add(moduleService);
        db.SaveChanges();
        return moduleService;
    }

    public List<ModuleService> SeedMany(ApplicationDbContext db, int moduleId, int numberOfEntitiesToCreate)
    {
        var moduleServices = Generate(moduleId, numberOfEntitiesToCreate);
        db.ModuleServices.AddRange(moduleServices);
        db.SaveChanges();
        return moduleServices;
    }

    private List<ModuleService> Generate(int moduleId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.ModuleId, moduleId);
        return faker.Generate(numberOfEntitiesCreate);
    }
}
