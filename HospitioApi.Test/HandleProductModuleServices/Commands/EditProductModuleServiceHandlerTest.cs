using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Core.HandleProductModuleService.Commands.EditProductModuleService;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModuleServices.Commands.EditProductModuleServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModuleServices.Commands;

public class EditProductModuleServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public EditProductModuleServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var module = _fix.ModuleFactory.SeedSingle(db);
        var moduleService = _fix.ModuleServiceFactory.SeedSingle(db, module);
        var product = _fix.ProductFactory.SeedSingle(db);
        var productModule = _fix.ProductModuleFactory.SeedSingle(db, product.Id, module.Id);
        _fix.In.ModuleServiceId = moduleService.Id;
        _fix.In.ProductId = product.Id;
        _fix.In.ProductModuleId = productModule.Id;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Edit product module service successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product module service not found.");

        _fix.Id = actualId;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The product module service is already exists in the system.");
    }
}

public class EditProductModuleServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditProductModuleServiceIn In { get; set; } = new();
    public int Id { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var module = ModuleFactory.SeedSingle(db);
        var moduleService = ModuleServiceFactory.SeedSingle(db, module);
        var product = ProductFactory.SeedSingle(db);
        var productModule = ProductModuleFactory.SeedSingle(db, product.Id, module.Id);
        var productModuleService = ProductModuleServiceFactory.SeedSingle(db, moduleService.Id, product.Id, productModule.Id);

        Id = productModuleService.Id;
        In.ModuleServiceId = moduleService.Id;
        In.ProductId = product.Id;
        In.ProductModuleId = productModule.Id;
    }

    public EditProductModuleServiceHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
