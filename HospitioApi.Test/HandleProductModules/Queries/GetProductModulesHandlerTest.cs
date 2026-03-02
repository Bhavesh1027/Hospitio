using HospitioApi.Core.HandleProductModule.Queries.GetProductModules;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModules.Queries.GetProductModulesHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModules.Queries;

public class GetProductModulesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductModulesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get product modules successful.");

        var productOut = (GetProductModulesOut)result.Response;
        Assert.NotNull(productOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        _fix.ProductModuleFactory.Remove(db, _fix.ProductModules);

        var result = await _fix.BuildHandler(db).Handle(new(), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product modules not found.");

        var product = _fix.ProductFactory.SeedSingle(db);
        var module = _fix.ModuleFactory.SeedSingle(db);
        _fix.ProductModuleFactory.SeedMany(db, product.Id, module.Id, 1);
    }
}

public class GetProductModulesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<ProductModule> ProductModules { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var module = ModuleFactory.SeedSingle(db);
        var productModules = ProductModuleFactory.SeedMany(db, product.Id, module.Id, 1);

        foreach (var productModule in productModules)
        {
            ProductModule obj = new()
            {
                Id = productModule.Id,
                ProductId = productModule.ProductId,
                ModuleId = productModule.ModuleId
            };

            ProductModules.Add(obj);
        }
    }

    public GetProductModulesHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
