using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlerts;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertsHandlerFixture;

namespace HospitioApi.Test.HandleCustomerGuestAlerts.Queries;

public class GetCustomerGuestAlertsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerGuestAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerGuestAlertsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerGuestAlertsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest alerts successful.");

        var CustomerGuestsOut = (GetCustomerGuestAlertsOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetCustomerGuestAlertsHandlerFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public string CustomerId { get; set; } = string.Empty;
    public List<CustomerGuestAlertsOut> CustomerGuestAlertsOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerGuestAlerts = CustomerGuestAlertFactory.SeedMany(db, 1);
        CustomerId = customer.Id.ToString();

        foreach (var customerGuestAlert in customerGuestAlerts)
        {
            CustomerGuestAlertsOut obj = new()
            {
                Id = customerGuestAlert.Id,
                IsActive = customerGuestAlert.IsActive,
                OfficeHoursMsg = customerGuestAlert.OfficeHoursMsg,
                OfficeHoursMsgWaitTimeInMinutes = customerGuestAlert.OfficeHoursMsgWaitTimeInMinutes,
                OfflineHourMsg = customerGuestAlert.OfflineHourMsg,
                OfflineHoursMsgWaitTimeInMinutes = customerGuestAlert.OfflineHoursMsgWaitTimeInMinutes,
                ReplyAtDiffPeriod = customerGuestAlert.ReplyAtDiffPeriod
            };
            CustomerGuestAlertsOut.Add(obj);
        }
    }

    public GetCustomerGuestAlertsHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
