
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsByCustomerId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertByCustomerIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerStaffAlerts.Queries;

public class GetCustomerStaffAlertsByCustomerIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerStaffAlertsByCustomerIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {

        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerStaffAlertsByCustomerIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerStaffAlertsByCustomerIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer staff alert successful.");

        //var customerStaffAlertsByCustomerIdOut = (GetCustomerStaffAlertsByCustomerIdOut)result.Response;
        //Assert.NotNull(customerStaffAlertsByCustomerIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerStaffAlertsByCustomerIdOut.First().Id;
        _fix.CustomerStaffAlertsByCustomerIdOut.First().Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerStaffAlertsByCustomerIdOut.First().Id = actualId;
    }
}

public class GetCustomerStaffAlertByCustomerIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerStaffAlertsByCustomerIdOut> CustomerStaffAlertsByCustomerIdOut { get; set; } = new();
    public int CustomerId { get; set; } 
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerStaffAlerts = CustomerStaffAlertFactory.SeedMany(db, 10);
        CustomerId = customer.Id;

        foreach (var customerStaffAlert in customerStaffAlerts)
        {
            CustomerStaffAlertsByCustomerIdOut obj = new()
            {
                Id = customerStaffAlert.Id,
                CustomerId = customer.Id,
                IsActive = customerStaffAlert.IsActive,

            };
            CustomerStaffAlertsByCustomerIdOut.Add(obj);
        }
    }

    public GetCustomerStaffAlertsByCustomerIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}