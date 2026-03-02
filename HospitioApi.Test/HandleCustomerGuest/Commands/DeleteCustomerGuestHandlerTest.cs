using HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Commands.DeleteCustomerGuestHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuest.Commands;

public class DeleteCustomerGuestHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerGuestHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer guest successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
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

public class DeleteCustomerGuestHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerGuestIn In { get; set; } = new DeleteCustomerGuestIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);

        In.Id = customerGuest.Id;
    }

    public DeleteCustomerGuestHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
