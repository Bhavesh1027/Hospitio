using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationById;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReservation.Queries.GetCustomerReservationByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReservation.Queries;

public class GetCustomerReservationByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerReservationByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerReservationByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerReservationByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer reservation successful.");

        var departmentOut = (GetCustomerReservationByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerReservationByIdOut.Id;
        _fix.CustomerReservationByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.CustomerReservationByIdOut.Id = actualId;
    }
}
public class GetCustomerReservationByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerReservationByIdIn In { get; set; } = new();
    public CustomerReservationByIdOut CustomerReservationByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        In.Id = reservation.Id;
        CustomerReservationByIdOut = new()
        {
            Id = reservation.Id,
            CustomerId = reservation.Id,
            Uuid = reservation.Uuid,
            ReservationNumber = reservation.ReservationNumber,
            Source = reservation.Source,
            NoOfGuestChildrens = reservation.NoOfGuestChildrens,
            NoOfGuestAdults = reservation.NoOfGuestAdults,
            CheckinDate = reservation.CheckinDate,
            CheckoutDate = reservation.CheckoutDate,
            IsActive = reservation.IsActive,
        };
    }
    public GetCustomerReservationByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}