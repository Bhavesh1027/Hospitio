using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class ProductModuleFactory
{
    private readonly Faker<ProductModule> _faker;
    public ProductModuleFactory()
    {
        _faker = new Faker<ProductModule>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public ProductModule SeedSingle(ApplicationDbContext db, int productId, int moduleId)
    {
        var productModule = _faker.Generate();
        productModule.ProductId = productId;
        productModule.ModuleId = moduleId;
        db.ProductModules.Add(productModule);
        db.SaveChanges();
        return productModule;
    }

    public List<ProductModule> SeedMany(ApplicationDbContext db, int productId, int moduleId, int numberOfEntitiesToCreate)
    {
        var productModules = Generate(productId, moduleId, numberOfEntitiesToCreate);
        db.ProductModules.AddRange(productModules);
        db.SaveChanges();
        return productModules;
    }

    public void Update(ApplicationDbContext db, ProductModule module)
    {
        db.ProductModules.Update(module);
        db.SaveChanges();
    }

    public void Remove(ApplicationDbContext db, List<ProductModule> module)
    {
        db.ProductModules.RemoveRange(module);
        db.SaveChanges();
    }

    private List<ProductModule> Generate(int productId, int moduleId, int numberOfEntitiesCreate)
    {
        var faker = _faker.Clone()
                   .RuleFor(m => m.ProductId, productId)
                   .RuleFor(m => m.ModuleId, moduleId);
        return faker.Generate(numberOfEntitiesCreate);
    }
}
