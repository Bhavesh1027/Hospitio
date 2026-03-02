using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Queries.GetDepartmentsUsersHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Queries;

public class GetDepartmentsUsersHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetDepartmentsUsersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<DepartmentsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.DepartmentsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get users successful.");

        var departmentsOut = (GetDepartmentsUsersOut)result.Response;
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

public class GetDepartmentsUsersHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetDepartmentsUsersIn In { get; set; } = new();
    public List<DepartmentsOut> DepartmentsOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var deparments = DepartmentFactory.SeedMany(db, 1);

        foreach (var department in deparments)
        {
            DepartmentsOut obj = new()
            {
                Id = department.Id,
                Name = department.Name,
                IsActive = department.IsActive ?? true,
                ManagerId = department.DepartmentMangerId,
                ManagerName = department.DepartmentManger?.UserName
            };

            DepartmentsOut.Add(obj);
        }
    }

    public GetDepartmentsUsersHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
