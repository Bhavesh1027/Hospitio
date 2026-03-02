using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlertsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAlerts.Commands;

public class DeleteCustomerGuestAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerGuestAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer guest alerts successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customer guest alerts could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomerGuestAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerGuestAlertsIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerGuestAlert = CustomerGuestAlertFactory.SeedSingle(db, customer.Id);

        In.Id = customerGuestAlert.Id;
    }

    public DeleteCustomerGuestAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
