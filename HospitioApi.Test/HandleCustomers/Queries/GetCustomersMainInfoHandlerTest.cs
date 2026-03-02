using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomersMainInfoHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomers.Queries;

public class GetCustomersMainInfoHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersMainInfoHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomersMainInfoOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomersMainInfoOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer main info successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomersMainInfoOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(new List<CustomersMainInfoOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomersMainInfoHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomersMainInfoIn In { get; set; } = new();
    public List<CustomersMainInfoOut> CustomersMainInfoOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customers = CustomerFactory.SeedMany(db, 1);

        foreach (var customer in customers)
        {
            CustomersMainInfoOut obj = new()
            {
                Id = customer.Id,
                FirstName = customer.Cname,
                LastName = customer.Cname,
                BusinessName = customer.BusinessName
            };
            CustomersMainInfoOut.Add(obj);
        }
    }
    public GetCustomersMainInfoHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}
