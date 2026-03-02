using HospitioApi.Core.HandleProductModuleService.Commands.CreateProductModuleService;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModuleServices.Commands.CreateProductModuleServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModuleServices.Commands;

public class CreateProductModuleServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateProductModuleServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

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

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create product module service successful.");
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        _fix.ProductModuleServiceFactory.SeedSingle(db, _fix.In.ModuleServiceId, _fix.In.ProductId, _fix.In.ProductModuleId);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The product module service is already exists in the system.");
    }
}

public class CreateProductModuleServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateProductModuleServiceIn In { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var module = ModuleFactory.SeedSingle(db);
        var moduleService = ModuleServiceFactory.SeedSingle(db, module);
        var product = ProductFactory.SeedSingle(db);
        var productModule = ProductModuleFactory.SeedSingle(db, product.Id, module.Id);

        In.ModuleServiceId = moduleService.Id;
        In.ProductId = product.Id;
        In.ProductModuleId = productModule.Id;
    }

    public CreateProductModuleServiceHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}