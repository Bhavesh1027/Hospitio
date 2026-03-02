//using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
//using HospitioApi.Core.Services.HandlerResponse;
//using HospitioApi.Data;
//using HospitioApi.Test.EntityFactories;
//using Xunit;
//using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Commands.CreateCustomerGuestHandlerTestFixture;

//namespace HospitioApi.Test.HandleCustomerGuest.Commands;

//public class CreateCustomerGuestHandlerTest : IClassFixture<ThisTestFixture>
//{
//    private readonly ThisTestFixture _fix;

//    public CreateCustomerGuestHandlerTest(ThisTestFixture fixture) => _fix = fixture;

//    [Fact]
//    public async Task Success()
//    {
//        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

//        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

//        Assert.True(result.HasResponse);
//        Assert.True(result.Response!.Message == "Create customer guest successful.");
//    }
//}

//public class CreateCustomerGuestHandlerTestFixture : DbFixture
//{
//    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
//    public CreateCustomerGuestIn In { get; set; } = new CreateCustomerGuestIn();

//    protected override void Seed()
//    {
//        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
//        db.Database.EnsureDeleted();
//        db.Database.EnsureCreated();
//        var customer = CustomerFactory.SeedSingle(db);
//        var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id);
//        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);

//        In.CustomerReservationId = customerReservation.Id;
//        In.Firstname = "Test";
//        In.Lastname = "Test";
//        In.Email = "test@gmail.com";
//        In.Picture = "Test Picture";
//        In.PhoneCountry = "in";
//        In.PhoneNumber = "1234567890";
//        In.Country = "in";
//        In.Language = "eng";
//        In.IdProof = "12345";
//        In.IdProofType = "1";
//        In.IdProofNumber = "123456";
//        In.BlePinCode = "12345";
//        In.Pin = "7777";
//        In.Street = "test";
//        In.StreetNumber = "120";
//        In.City = "test";
//        In.Postalcode = "579";
//        In.ArrivalFlightNumber = "12345";
//        In.DepartureAirline = "test";
//        In.DepartureFlightNumber = "12345";
//        In.Signature = "test sign";
//        In.RoomNumber = "1301";
//        In.TermsAccepted = true;
//        In.FirstJourneyStep = 1;
//        In.Rating = 5;
//        In.IsActive = true;
//        In.Vat = "131";
//    }

//    public CreateCustomerGuestHandler BuildHandler(ApplicationDbContext db) =>
//        new(db, Response);
//}
