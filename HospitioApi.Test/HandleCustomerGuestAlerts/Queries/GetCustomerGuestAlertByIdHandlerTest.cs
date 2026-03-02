using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAlerts.Queries;

public class GetCustomerGuestAlertByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerGuestAlertByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerGuestAlertByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestAlertByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerGuestAlertByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest alert successful.");

        var customerGuestAlertByIdOut = (GetCustomerGuestAlertByIdOut)result.Response;
        Assert.NotNull(customerGuestAlertByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerGuestAlertByIdOut.Id;
        _fix.CustomerGuestAlertByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomerGuestAlertByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerGuestAlertByIdOut.Id = actualId;
    }
}

public class GetCustomerGuestAlertByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerGuestAlertByIdOut CustomerGuestAlertByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerGuestAlert = CustomerGuestAlertFactory.SeedSingle(db, customer.Id);

        CustomerGuestAlertByIdOut = new()
        {
            Id = customerGuestAlert.Id,
            CustomerId = customer.Id,
            IsActive = customerGuestAlert.IsActive,
            OfficeHoursMsg = customerGuestAlert.OfficeHoursMsg,
            OfficeHoursMsgWaitTimeInMinutes = customerGuestAlert.OfficeHoursMsgWaitTimeInMinutes,
            OfflineHourMsg = customerGuestAlert.OfflineHourMsg,
            OfflineHoursMsgWaitTimeInMinutes = customerGuestAlert.OfflineHoursMsgWaitTimeInMinutes,
            ReplyAtDiffPeriod = customerGuestAlert.ReplyAtDiffPeriod
        };
    }

    public GetCustomerGuestAlertByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
