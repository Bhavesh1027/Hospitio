using FakeItEasy;
using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserLevels.Queries.GetUserLevelsHandlerTestFixture;

namespace HospitioApi.Test.HandleUserLevels.Queries;

public class GetUserLevelsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetUserLevelsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<UserLevelsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.UserLevels);

        var result = await _fix.BuildHandler(_dapper).Handle(new(UserTypeEnum.Hospitio), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get user levels successful.");

        var ticketsOut = (GetUserLevelsOut)result.Response;
        Assert.NotNull(ticketsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAll<UserLevelsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<UserLevelsOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(UserTypeEnum.Hospitio), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}


public class GetUserLevelsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetUserLevelsIn In { get; set; } = new GetUserLevelsIn();
    public List<UserLevelsOut> UserLevels { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var userLevels = UserLevelFactory.SeedMany(db, 1);

        foreach (var level in userLevels)
        {
            UserLevelsOut obj = new()
            {
                Id = level.Id,
                LevelName = level.LevelName,
                NormalizedLevelName = level.NormalizedLevelName,
            };
            UserLevels.Add(obj);
        }
    }

    public GetUserLevelsHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
