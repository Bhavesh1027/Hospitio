using HospitioApi.Core.HandleProduct.Commands.CreateProduct;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProduct.Commands.CreateProductHandlerTestFixture;

namespace HospitioApi.Test.HandleProduct.Commands;

public class CreateProductHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateProductHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Save product successful.");
    }
}

public class CreateProductHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateProductIn In { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var moduleObj = ModuleFactory.SeedSingle(db);
        var productModuleObj = ProductModuleFactory.SeedSingle(db, product.Id, moduleObj.Id);
        ProductModuleServiceFactory.SeedSingle(db);
        var moduleServiceObj = ModuleServiceFactory.SeedSingle(db, moduleObj);

        In.ProductId = product.Id;
        In.ProductName = "test";
        In.IsActive = true;

        ProductModuleRequest productModule = new()
        {
            ModuleId = moduleObj.Id,
            Price = 100,
            Currency = "test",
            IsActive = true,
            Id = productModuleObj.Id,
        };

        List<ProductModuleRequest> productModules = new List<ProductModuleRequest>();
        productModules.Add(productModule);
        In.ProductModules = productModules;

        ProductModuleService productModuleService = new()
        {
            ProductModuleId = productModuleObj.Id,
            ProductId = product.Id,
            ModuleServiceId = moduleServiceObj.Id,
            IsActive = true,
        };

        foreach (var module in In.ProductModules)
        {
            module.ProductModuleServices.Add(productModuleService);
        }
    }

    public CreateProductHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
