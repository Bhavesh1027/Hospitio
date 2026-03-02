using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Commands.UpdateCustomerGuestHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuest.Commands;

public class UpdateCustomerGuestHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerGuestHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _fix.chatService, _fix.hubContext, _fix._vonage).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "update customer guest successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualCustomerReservationId = _fix.In.CustomerReservationId;
        _fix.In.CustomerReservationId = 0;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db,_fix.chatService,_fix.hubContext,_fix._vonage).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customer guest could not be found.");

        _fix.In.Id = actualId;
        _fix.In.CustomerReservationId = actualCustomerReservationId;
    }

    //[Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db, _fix.chatService, _fix.hubContext, _fix._vonage).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer guest already exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerGuestHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerGuestIn In { get; set; } = new UpdateCustomerGuestIn();

    public IChatService   chatService { get; set; } = A.Fake<IChatService>();
    public IHubContext<ChatHub> hubContext { get; set; } = A.Fake<IHubContext<ChatHub>>();
    public IVonageService _vonage { get; set; } = A.Fake<IVonageService>(); 
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var bussienssType = BusinessTypeFactory.SeedSingle(db);
        var customer = CustomerFactory.SeedSingle(db,product.Id,bussienssType.Id);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);

        In.Id = customerGuest.Id;
        In.CustomerReservationId = customerGuest.CustomerReservationId;
        In.Firstname = "Test";
        In.Lastname = "Test";
        In.Email = "test@gmail.com";
        In.Picture = "Test Picture";
        In.PhoneCountry = "in";
        In.PhoneNumber = "1234567890";
        In.Country = "in";
        In.Language = "eng";
        In.IdProof = "12345";
        In.IdProofType = "1";
        In.IdProofNumber = "123456";
        In.BlePinCode = "12345";
        In.Pin = "7777";
        In.Street = "test";
        In.StreetNumber = "120";
        In.City = "test";
        In.Postalcode = "579";
        In.ArrivalFlightNumber = "12345";
        In.DepartureAirline = "test";
        In.DepartureFlightNumber = "12345";
        In.Signature = "test sign";
        In.RoomNumber = "1301";
        In.TermsAccepted = true;
        In.FirstJourneyStep = 1;
        In.Rating = 5;
        In.IsActive = true;
        In.Vat = "131";
    }

    public UpdateCustomerGuestHandler BuildHandler(ApplicationDbContext db, IChatService chatService, IHubContext<ChatHub> hubContext, IVonageService _vonage) =>
        new(db, Response, chatService, hubContext, _vonage);
}