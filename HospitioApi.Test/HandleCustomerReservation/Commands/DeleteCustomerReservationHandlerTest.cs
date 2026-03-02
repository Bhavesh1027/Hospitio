using HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.DeleteCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReservation.Commands.DeleteCustomerReservationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReservation.Commands;

public class DeleteCustomerReservationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerReservationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer reservation successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer reservation could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomerReservationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerReservationIn In { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);

        In.Id = reservation.Id;
    }

    public DeleteCustomerReservationHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
