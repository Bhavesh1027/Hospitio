using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerStaffAlerts.Queries;

public class GetCustomerStaffAlertsByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerStaffAlertsByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerStaffAlertsByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerStaffAlertsByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerStaffAlertsByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer staff alert successful.");

        var customerStaffAlertByIdOut = (GetCustomerStaffAlertsByIdOut)result.Response;
        Assert.NotNull(customerStaffAlertByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerStaffAlertsByIdOut.Id;
        _fix.CustomerStaffAlertsByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomerStaffAlertsByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerStaffAlertsByIdOut.Id = actualId;
    }
}

public class GetCustomerStaffAlertHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerStaffAlertsByIdOut CustomerStaffAlertsByIdOut { get; set; } = new();
    
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerStaffAlert = CustomerStaffAlertFactory.SeedSingle(db, customer.Id);

        CustomerStaffAlertsByIdOut = new()
        {
            Id = customerStaffAlert.Id,
            CustomerId = customer.Id,
            IsActive = customerStaffAlert.IsActive,
         
        };
    }

    public GetCustomerStaffAlertsByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
