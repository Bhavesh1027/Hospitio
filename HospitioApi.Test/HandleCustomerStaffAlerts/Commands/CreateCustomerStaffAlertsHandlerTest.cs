
using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlertsHandlerTestFixture;
namespace HospitioApi.Test.HandleCustomerStaffAlerts.Commands;
public class CreateCustomerStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerStaffAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer staff alert successful.");
    }
}

public class CreateCustomerStaffAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerStaffAlertsIn In { get; set; } = new CreateCustomerStaffAlertsIn();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id.ToString();

        In.Name = "test";
        In.Platfrom = "test";
        In.PhoneNumber = "test";
        In.PhoneCountry = "test";
        In.WaitTimeInMintes = 1;
        In.IsActive = true;

        
    }

    public CreateCustomerStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}