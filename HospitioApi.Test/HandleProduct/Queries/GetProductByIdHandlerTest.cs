using HospitioApi.Core.HandleProduct.Queries.GetProductById;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProduct.Queries.GetProductByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleProduct.Queries;

public class GetProductByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.Id, _fix.UserId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get product successful.");

        var productOut = (GetProductByIdOut)result.Response;
        Assert.NotNull(productOut);
    }
}

public class GetProductByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var user = UserFactory.SeedSingle(db);
        var product = ProductFactory.SeedSingle(db, user);
        Id = product.Id;
        var module = ModuleFactory.SeedMany(db, 1);
        ModuleServiceFactory.SeedSingle(db, module[0]);
        ProductModuleFactory.SeedSingle(db, product.Id, module[0].Id);
        UserId = user.Id.ToString();
    }

    public GetProductByIdHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}