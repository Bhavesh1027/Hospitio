using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerReservation;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReservation.Commands.CreateCustomerReservationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReservation.Commands;

public class CreateCustomerReservationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerReservationHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.ReservationNumber;
        _fix.In.ReservationNumber = "12345";
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer reservation successful.");
        _fix.In.ReservationNumber = actualName;
    }
    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

      
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer reservation already exists.");


    }
}

public class CreateCustomerReservationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerReservationIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        var reservation  = CustomerReservationFactory.SeedSingle(db, customer.Id);
        In.CustomerId = customer.Id;
        In.Uuid = "123";
        In.ReservationNumber = reservation.ReservationNumber;
        In.Source = "Test";
        In.NoOfGuestChilderns = 1;
        In.NoOfGuestAdults = 1;
        In.CheckinDate = DateTime.Now;
        In.CheckoutDate = DateTime.Now;
        In.IsActive = true;
    }
    public CreateCustomerReservationHandler BuildHandler(ApplicationDbContext db) =>
       new(db, Response);
}
