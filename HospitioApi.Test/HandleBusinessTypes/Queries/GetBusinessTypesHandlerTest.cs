using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleBusinessTypes.Queries.GetBusinessTypesHandlerTestFixture;

namespace HospitioApi.Test.HandleBusinessTypes.Queries;

public class GetBusinessTypesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetBusinessTypesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<BusinessTypesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.businessTypesOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get business type successful.");

        var businessTypesOut = (GetBusinessTypesOut)result.Response;
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
public class GetBusinessTypesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<BusinessTypesOut> businessTypesOuts { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var businessTypes = BusinessTypeFactory.SeedMany(db, 1);

        foreach (var businessType in businessTypes)
        {
            BusinessTypesOut obj = new()
            {
                Id = businessType.Id
            };
            businessTypesOuts.Add(obj);
        }
    }

    public GetBusinessTypesHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}