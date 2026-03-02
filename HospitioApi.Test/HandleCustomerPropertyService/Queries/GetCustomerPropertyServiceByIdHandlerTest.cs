using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyService.Queries;

public class GetCustomerPropertyServiceByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerPropertyServiceByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerPropertyServiceByd>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerPropertyServiceByd);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerPropertyServiceByd.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer property service successful.");

        var customerPropertyServiceByIdOut = (GetCustomerPropertyServiceByIdOut)result.Response;
        Assert.NotNull(customerPropertyServiceByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerPropertyServiceByd.Id;
        _fix.CustomerPropertyServiceByd.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomerPropertyServiceByd.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.CustomerPropertyServiceByd.Id = actualId;
    }
}
public class GetCustomerPropertyServiceByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerPropertyServiceByd CustomerPropertyServiceByd { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerPropertyService = CustomerPropertyServiceFactory.SeedSingle(db);

        CustomerPropertyServiceByd = new()
        {
            Id = customerPropertyService.Id
        };
    }

    public GetCustomerPropertyServiceByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}