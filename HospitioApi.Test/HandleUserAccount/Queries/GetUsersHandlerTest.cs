using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Queries.GetUsersHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Queries;

public class GetUsersHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetUsersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAllJsonData<UserOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.UsersOut);

        var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get users successful.");

        var userByIdOut = (GetUsersOut)result.Response;
        Assert.NotNull(userByIdOut);
    }
}

public class GetUsersHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetUsersIn In { get; set; } = new();
    public List<UserOut> UsersOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var users = UserFactory.SeedMany(db, 1);

        foreach (var user in users)
        {
            UserOut obj = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive ?? true,
                DepartmentId = user.DepartmentId,
                PhoneCountry = user.PhoneCountry,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Title = user.Title,
                UserLevelId = user.UserLevelId
            };

            UsersOut.Add(obj);
        }
    }

    public GetUsersHandler BuildHandler(ApplicationDbContext db, IDapperRepository dapper) =>
        new(db, dapper, Response);
}
