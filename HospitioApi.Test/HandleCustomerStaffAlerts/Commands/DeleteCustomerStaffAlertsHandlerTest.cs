
using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlertsHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomerStaffAlerts.Commands;
public class DeleteCustomerStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerStaffAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer staff alert successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer staff alert with {_fix.In.Id} not found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomerStaffAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerStaffAlertsIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerStaffAlert = CustomerStaffAlertFactory.SeedSingle(db, customer.Id);

        In.Id = customerStaffAlert.Id;
    }

    public DeleteCustomerStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
