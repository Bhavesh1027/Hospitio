using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleHospitioOnBoarding.Queries.GetHospitioOnBoarding;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioOnBoarding.Queries.GetHospitioOnBoardingHandlerTestFixture;

namespace HospitioApi.Test.HandleHospitioOnBoarding.Queries;

public class GetHospitioOnBoardingHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetHospitioOnBoardingHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<HospitioOnBoardingOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.HospitioOnBoardingOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get hospitio on boarding successful.");

        var getHospitioOnBoardingOut = (GetHospitioOnBoardingOut)result.Response;
        Assert.NotNull(getHospitioOnBoardingOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.HospitioOnBoardingOut.Id;
        _fix.HospitioOnBoardingOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new() , CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.HospitioOnBoardingOut.Id = actualId;
    }
}

public class GetHospitioOnBoardingHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public HospitioOnBoardingOut HospitioOnBoardingOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        HospitioOnBoardingOut = new()
        {
            Id = 1
        };
    }

    public GetHospitioOnBoardingHandler BuildHandler(IDapperRepository _dapper) =>
        new(Response, _dapper);
}
