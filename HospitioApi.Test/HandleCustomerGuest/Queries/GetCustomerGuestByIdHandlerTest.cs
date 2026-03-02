using Bogus;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Queries.GetCustomerGuestByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuest.Queries;

public class GetCustomerGuestByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerGuestByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerGuestByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerGuestByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest successful.");

        var customerGuestByIdOut = (GetCustomerGuestByIdOut)result.Response;
        Assert.NotNull(customerGuestByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerGuestByIdOut.Id;
        _fix.CustomerGuestByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomerGuestByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerGuestByIdOut.Id = actualId;
    }
}

public class GetCustomerGuestByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerGuestByIdOut CustomerGuestByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);

        CustomerGuestByIdOut = new()
        {
            Id = customerGuest.Id,
            ArrivalFlightNumber = customerGuest.ArrivalFlightNumber,
            BlePinCode = customerGuest.BlePinCode,
            City = customerGuest.City,
            Country = customerGuest.Country,
            CustomerReservationId = customerGuest.CustomerReservationId,
            DateOfBirth = customerGuest.DateOfBirth,
            DepartureAirline = customerGuest.DepartureAirline,
            DepartureFlightNumber = customerGuest.DepartureFlightNumber,
            Email = customerGuest.Email,
            FirstJourneyStep = customerGuest.FirstJourneyStep,
            Firstname = customerGuest.Firstname,
            IdProof = customerGuest.IdProof,
            IdProofNumber = customerGuest.IdProofNumber,
            IdProofType = customerGuest.IdProofType,
            IsActive = customerGuest.IsActive,
            Language = customerGuest.Language,
            Lastname = customerGuest.Lastname,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Picture = customerGuest.Picture,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Rating = customerGuest.Rating,
            RoomNumber = customerGuest.RoomNumber,
            Signature = customerGuest.Signature,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            TermsAccepted = customerGuest.TermsAccepted,
            Vat = customerGuest.Vat
        };
    }

    public GetCustomerGuestByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}