using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlertsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAlerts.Commands;

public class UpdateCustomerGuestAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerGuestAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In,_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer guest alerts successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;
        var actualOfficeHoursMsg = _fix.In.OfficeHoursMsg;
        _fix.In.OfficeHoursMsg = null;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Ticket customer guest alert could not be found.");

        _fix.In.Id = actualId;
        _fix.In.OfficeHoursMsg = actualOfficeHoursMsg;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer guest alerts already exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerGuestAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerGuestAlertsIn In { get; set; } = new();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id.ToString();
        var customerGuestAlert =  CustomerGuestAlertFactory.SeedSingle(db, customer.Id);

        In.Id = customerGuestAlert.Id;
        In.OfflineHourMsg = "test";
        In.OfficeHoursMsg = "test";
        In.OfficeHoursMsgWaitTimeInMinutes = 1;
        In.OfflineHoursMsgWaitTimeInMinutes = 1;
        In.ReplyAtDiffPeriod = false;
        In.IsActive = true;
    }

    public UpdateCustomerGuestAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
