using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerProduct;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Commands.UpdateCustomerProductHandlerTestFixture;
using Xunit;

namespace HospitioApi.Test.HandleCustomers.Commands;

public class UpdateCustomerProductHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerProductHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer product update successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerProductHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerProductIn In { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var customer = CustomerFactory.SeedSingle(db, product.Id);
        In.Id = customer.Id;
        In.ProductId = product.Id;
    }

    public UpdateCustomerProductHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}