using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReservation.Commands.UpdateCustomerReservationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReservation.Commands;

public class UpdateCustomerReservationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateCustomerReservationHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer reservation successful.");
    }


    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;
        
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer reservation already exists.");
        _fix.In.Id = actualId;
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.In.Id;
        _fix.In.Id = 0;
        var actualnumvber = _fix.In.ReservationNumber;
        _fix.In.ReservationNumber = "12345";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer reservation could not be found.");

        _fix.In.Id = actualId;
        _fix.In.ReservationNumber = actualnumvber;
    }
}

public class UpdateCustomerReservationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public UpdateCustomerReservationIn In { get; set; } = new();
    public CustomerReservation CustomerReservation { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        CustomerReservation = reservation;

        In.Id = reservation.Id;
        In.CustomerId = customer.Id;
        In.Uuid = "123";
        In.ReservationNumber = "1234";
        In.Source = "Test";
        In.NoOfGuestChilderns = 1;
        In.NoOfGuestAdults = 1;
        In.CheckinDate = DateTime.Now;
        In.CheckoutDate = DateTime.Now;
        In.IsActive = true;
    }
    public UpdateCustomerReservationHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}
