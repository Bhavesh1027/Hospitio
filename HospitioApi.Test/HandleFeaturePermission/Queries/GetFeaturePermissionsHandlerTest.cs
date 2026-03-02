using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
using HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleFeaturePermission.Queries.GetFeaturePermissionsHandlerTestFixture;

namespace HospitioApi.Test.HandleFeaturePermission.Queries;

public class GetFeaturePermissionsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetFeaturePermissionsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetFeaturePermissionsResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GetFeaturePermissionsResponseOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get feature permission successful.");

        var businessTypesOut = (GetFeaturePermissionsOut)result.Response;
        Assert.NotNull(businessTypesOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}
public class GetFeaturePermissionsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GetFeaturePermissionsResponseOut> GetFeaturePermissionsResponseOut { get; set; } = new();
    //public int number { get; set; }

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        //number = 1;
        var digitalAssistants = PermissionFactory.SeedMany(db, 1);
        foreach (var item in digitalAssistants)
        {
            GetFeaturePermissionsResponseOut obj = new()
            {
                Id = item.Id,
                Name = item.Name
            };
            GetFeaturePermissionsResponseOut.Add(obj);
        }
    }
    public GetFeaturePermissionsHandler BuildHandler(IDapperRepository dapper) => new(dapper, Response);
}