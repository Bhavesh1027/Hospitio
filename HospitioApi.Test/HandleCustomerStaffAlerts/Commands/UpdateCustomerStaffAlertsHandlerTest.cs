using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlertsHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomerStaffAlerts.Commands;
public class UpdateCustomerStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerStaffAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer staff alert successful.");
    }

    
    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer staff alert {_fix.In.Name} already exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerStaffAlertsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerStaffAlertsIn In { get; set; } = new();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id.ToString();
        var customerStaffAlert = CustomerStaffAlertFactory.SeedSingle(db, customer.Id);

        In.Id = customerStaffAlert.Id;
        In.Name = "Test";
        In.IsActive = true;
    }

    public UpdateCustomerStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
