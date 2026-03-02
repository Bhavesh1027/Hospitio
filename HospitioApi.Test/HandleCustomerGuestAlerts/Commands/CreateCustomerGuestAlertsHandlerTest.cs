using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.CreateCustomerGuestAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAlerts.Commands.CreateCustomerGuestAlertsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAlerts.Commands;

public class CreateCustomerGuestAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerGuestAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer guest alert successful.");
    }
}

public class CreateCustomerGuestAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestAlertsIn In { get; set; } = new CreateCustomerGuestAlertsIn();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id.ToString();

        In.OfflineHourMsg = "test";
        In.OfficeHoursMsg = "test";
        In.OfficeHoursMsgWaitTimeInMinutes = 1;
        In.OfflineHoursMsgWaitTimeInMinutes = 1;
        In.ReplyAtDiffPeriod = false;
        In.IsActive = true;
    }

    public CreateCustomerGuestAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}