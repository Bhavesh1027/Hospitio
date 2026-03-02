using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInGuest;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInGuestHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands;

public class EditCustomerGuestPortalCheckInGuestHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public EditCustomerGuestPortalCheckInGuestHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "update customer guest successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customer guest could not be found.");

        _fix.In.Id = actualId;
    }
}

public class EditCustomerGuestPortalCheckInGuestHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditCustomerGuestPortalCheckInGuestIn In { get; set; } = new EditCustomerGuestPortalCheckInGuestIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db,customerReservation.Id);

        In.Id = customerGuest.Id;
    }

    public EditCustomerGuestPortalCheckInGuestHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
