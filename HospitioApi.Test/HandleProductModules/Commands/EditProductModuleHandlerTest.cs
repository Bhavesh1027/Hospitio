using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Core.HandleProductModule.Commands.EditProductModule;
using ThisTestFixture = HospitioApi.Test.HandleProductModules.Commands.EditProductModuleHandlerTestFixture;
using Xunit;

namespace HospitioApi.Test.HandleProductModules.Commands;

public class EditProductModuleHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public EditProductModuleHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var product = _fix.ProductFactory.SeedSingle(db);
        var module = _fix.ModuleFactory.SeedSingle(db);
        _fix.ProductModule.ProductId = product.Id;
        _fix.ProductModule.ModuleId = module.Id;
        _fix.ProductModuleFactory.Update(db, _fix.ProductModule);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Edit product module successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product module not found.");

        _fix.Id = actualId;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The product module is already exists in the system.");
    }
}

public class EditProductModuleHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditProductModuleIn In { get; set; } = new();
    public int Id { get; set; }
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

        Id = productModule.Id;
        In.ProductId = product.Id;
        In.ModuleId = module.Id;
        In.Price = 100;
        In.Currency = "IN";
    }

    public EditProductModuleHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}