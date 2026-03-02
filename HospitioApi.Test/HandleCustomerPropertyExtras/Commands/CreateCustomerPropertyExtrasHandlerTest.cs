using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtras;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtrasHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Commands;

public class CreateCustomerPropertyExtrasHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerPropertyExtrasHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer property extra successful.");
    }
}
public class CreateCustomerPropertyExtrasHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerPropertyExtrasIn In { get; set; } = new CreateCustomerPropertyExtrasIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        In.CustomerPropertyInformationId = customerProperty.Id;
    }

    public CreateCustomerPropertyExtrasHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}