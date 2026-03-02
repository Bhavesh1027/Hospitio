using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReservation.Queries.GetCustomerReservationsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReservation.Queries;

public class GetCustomerReservationsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerReservationsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<CustomerReservationsOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerReservationsOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer reservations successful.");

        var departmentOut = (GetCustomerReservationsOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}
public class GetCustomerReservationsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerReservationsIn In { get; set; } = new();
    public List<CustomerReservationsOut> CustomerReservationsOut { get; set; } = new();

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        //In.CustomerId = customer.Id;
        var reservations = CustomerReservationFactory.SeedMany(db, 1);
        In.CustomerId = customer.Id;
        In.PageNo = 1;
        In.PageSize = 10;
        foreach (var item in reservations)
        {
            CustomerReservationsOut obj = new()
            {
                Id = item.Id,
                CustomerId = item.Id,
                Uuid = item.Uuid,
                ReservationNumber = item.ReservationNumber,
                Source = item.Source,
                NoOfGuestChildrens = item.NoOfGuestChildrens,
                NoOfGuestAdults = item.NoOfGuestAdults,
                CheckinDate = item.CheckinDate,
                CheckoutDate = item.CheckoutDate,
                IsActive = item.IsActive,
            };
            CustomerReservationsOut.Add(obj);
        }
    }
    public GetCustomerReservationsHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}