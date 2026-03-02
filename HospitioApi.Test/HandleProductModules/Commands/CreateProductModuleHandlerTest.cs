using HospitioApi.Core.HandleProduct.Queries.GetProductById;
using HospitioApi.Core.HandleProductModule.Commands.CreateProductModule;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModules.Commands.CreateProductModuleHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModules.Commands;

public class CreateProductModuleHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateProductModuleHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The product module is already exists in the system.");
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var product = _fix.ProductFactory.SeedSingle(db);
        var module = _fix.ModuleFactory.SeedSingle(db);
        _fix.ProductModule.ProductId = product.Id;
        _fix.ProductModule.ModuleId = module.Id;
        _fix.ProductModuleFactory.Update(db, _fix.ProductModule);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create product module successful.");
    }
}

public class CreateProductModuleHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateProductModuleIn In { get; set; } = new();
    public Data.Models.ProductModule ProductModule { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var module = ModuleFactory.SeedSingle(db);
        var productModule = ProductModuleFactory.SeedSingle(db, product.Id, module.Id);

        ProductModule.Id = productModule.Id;
        ProductModule.ProductId = product.Id;
        ProductModule.ModuleId = module.Id;

        In.ProductId = product.Id;
        In.ModuleId = module.Id;
        In.Price = 100;
        In.Currency = "IN";
    }

    public CreateProductModuleHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}