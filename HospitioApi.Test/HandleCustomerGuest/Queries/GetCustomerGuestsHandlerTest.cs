using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Queries.GetCustomerGuestsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuest.Queries;

public class GetCustomerGuestsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerGuestsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerGuestsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerGuestsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In,_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guests successful.");

        var CustomerGuestsOut = (GetCustomerGuestsOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        _fix.CustomerId = 0;
        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In,_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetCustomerGuestsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerGuestsOut> CustomerGuestsOut { get; set; } = new();
    public GetCustomerGuestsIn In { get; set; } = new();
    public int CustomerId { get; set; }
    public IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings { get; set; } = A.Fake<IOptions<FrontEndLinksSettingsOptions>>();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var customerGuests = CustomerGuestFactory.SeedMany(db, 1);

        //In.CustomerReservationId = customerReservation.Id;
        In.SearchValue = "test";
        //In.SearchColumn = "FirstName";

        foreach (var customerGuest in customerGuests)
        {
            CustomerGuestsOut obj = new()
            {
                Id = customerGuest.Id,
                CustomerReservationId = customerGuest.CustomerReservationId,
                Firstname = customerGuest.Firstname,
                Lastname = customerGuest.Lastname,
                RoomNumber = customerGuest.RoomNumber
            };
            CustomerGuestsOut.Add(obj);
        }
    }

    public GetCustomerGuestsHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response , frontEndLinksSettings);
}
