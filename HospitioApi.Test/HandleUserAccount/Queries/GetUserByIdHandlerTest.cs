using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Queries.GetUserByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Queries;

public class GetUserByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetUserByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAllJsonData<UserByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.UsersOut);

        var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get user successful.");

        var userByIdOut = (GetUserByIdOut)result.Response;
        Assert.NotNull(userByIdOut);
    }
}

public class GetUserByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetUserByIdIn In { get; set; } = new();
    public List<UserByIdOut> UsersOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var users = UserFactory.SeedMany(db, 1);

        foreach (var user in users)
        {
            UserByIdOut obj = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive ?? true,
                DepartmentId = user.DepartmentId,
                GroupId = user.GroupId,
                Password = user.Password,
                PhoneCountry = user.PhoneCountry,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                SupervisorId = user.SupervisorId,
                Title = user.Title,
                UserLevelId = user.UserLevelId
            };

            UsersOut.Add(obj);
        }
    }

    public GetUserByIdHandler BuildHandler(ApplicationDbContext db, IDapperRepository dapper) =>
        new(dapper, db, Response);
}
