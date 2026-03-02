using HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServiceById;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModuleServices.Queries.GetProductModuleServiceByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModuleServices.Queries;

public class GetProductModuleServiceByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductModuleServiceByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.Id), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get product module service successful.");

        var productOut = (GetProductModuleServiceByIdOut)result.Response;
        Assert.NotNull(productOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product module service not found.");

        _fix.Id = actualId;
    }
}

public class GetProductModuleServiceByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
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
    }

    public GetProductModuleServiceByIdHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
