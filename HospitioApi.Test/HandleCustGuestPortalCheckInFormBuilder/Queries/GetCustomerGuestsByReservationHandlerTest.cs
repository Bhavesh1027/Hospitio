using FakeItEasy;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestsByReservation;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestsByReservationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Queries;

public class GetCustomerGuestsByReservationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerGuestsByReservationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();
        A.CallTo(() => _dapper.GetAll<CustomerGuestsByReservationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestsByReservationOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guests successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        A.CallTo(() => _dapper.GetAll<CustomerGuestsByReservationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(new List<CustomerGuestsByReservationOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomerGuestsByReservationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerGuestsByReservationIn In { get; set; } = new GetCustomerGuestsByReservationIn();
    public List<CustomerGuestsByReservationOut> CustomerGuestsByReservationOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);

        CustomerGuestsByReservationOut obj = new()
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerReservation.Id,
        };

        CustomerGuestsByReservationOut.Add(obj);
    }

    public GetCustomerGuestsByReservationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
