using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsersByGroupId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Queries.GetUsersByGroupIdHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Queries;

public class GetUsersByGroupIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetUsersByGroupIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<UsersByGroupIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.UsersOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get users successful.");

        var departmentsOut = (GetUsersByGroupIdOut)result.Response;
        Assert.NotNull(departmentsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetUsersByGroupIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetUsersByGroupIdIn In { get; set; } = new();
    public List<UsersByGroupIdOut> UsersOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var users = UserFactory.SeedMany(db, 1);

        foreach (var user in users)
        {
            UsersByGroupIdOut obj = new()
            {
                Id = user.Id,
                Name = user.FirstName + " " + user.LastName,
            };

            UsersOut.Add(obj);
        }

    }

    public GetUsersByGroupIdHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
