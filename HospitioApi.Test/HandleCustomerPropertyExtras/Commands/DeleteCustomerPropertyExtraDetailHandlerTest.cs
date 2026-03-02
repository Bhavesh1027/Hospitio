using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetailHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Commands;

public class DeleteCustomerPropertyExtraDetailHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerPropertyExtraDetailHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer property extra detail successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer property extra detail could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerPropertyExtraDetailHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerPropertyExtraDetailIn In { get; set; } = new DeleteCustomerPropertyExtraDetailIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerPropertyExtraDetailFactory.SeedSingle(db);

        In.Id = customerProperty.Id;
    }

    public DeleteCustomerPropertyExtraDetailHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}