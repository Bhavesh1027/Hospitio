using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByIdForHospitio;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomerByIdForHospitioHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomers.Queries;

public class GetCustomerByIdForHospitioHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerByIdForHospitioHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerByIdForHospitioOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerByIdForHospitioOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        CustomerByIdForHospitioOut? obj = null;

        A.CallTo(() => _dapper.GetSingle<CustomerByIdForHospitioOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomerByIdForHospitioHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerByIdForHospitioIn In { get; set; } = new();
    public CustomerByIdForHospitioOut CustomerByIdForHospitioOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerByIdForHospitioOut.Id = customer.Id;

    }
    public GetCustomerByIdForHospitioHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}
