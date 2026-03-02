using Bogus;
using HospitioApi.Data;
using HospitioApi.Data.Models;

namespace HospitioApi.Test.EntityFactories;

public class ProductModuleServiceFactory
{
    private readonly Faker<ProductModuleService> _faker;
    public ProductModuleServiceFactory()
    {
        _faker = new Faker<ProductModuleService>()
            .RuleFor(m => m.Id, f => f.Random.Int(1, 999))
            .RuleFor(m => m.CreatedAt, f => f.Date.Recent())
            .RuleFor(m => m.UpdateAt, f => f.Date.Recent());
    }

    public ProductModuleService SeedSingle(ApplicationDbContext db,int? moduleServiceId = null, int? productId = null, int? productModuleId = null)
    {
        var productModuleService = _faker.Generate();
        productModuleService.ModuleServiceId = moduleServiceId;
        productModuleService.ProductId = productId; 
        productModuleService.ProductModuleId = productModuleId;
        db.ProductModuleServices.Add(productModuleService);
        db.SaveChanges();
        return productModuleService;
    }

    public List<ProductModuleService> SeedMany(ApplicationDbContext db, int numberOfEntitiesToCreate)
    {
        var ProductModuleServices = _faker.Generate(numberOfEntitiesToCreate);
        db.ProductModuleServices.AddRange(ProductModuleServices);
        db.SaveChanges();
        return ProductModuleServices;
    }
}
