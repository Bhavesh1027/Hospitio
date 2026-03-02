using HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServices;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductModuleServices.Queries.GetProductModuleServicesHandlerTestFixture;

namespace HospitioApi.Test.HandleProductModuleServices.Queries;

public class GetProductModuleServicesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductModuleServicesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get product module services successful.");

        var productOut = (GetProductModuleServicesOut)result.Response;
        Assert.NotNull(productOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product module services not found.");

        _fix.ProductModuleServiceFactory.SeedMany(db, 1);
    }
}

public class GetProductModuleServicesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    public GetProductModuleServicesHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
