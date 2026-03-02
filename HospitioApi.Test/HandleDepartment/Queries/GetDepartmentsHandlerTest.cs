using FakeItEasy;
using Humanizer;
using Moq;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleDepartment.Queries.GetDepartmentsHandlerTestFixture;

namespace HospitioApi.Test.HandleDepartment.Queries;

public class GetDepartmentsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly GetDepartmentsIn In;
    public GetDepartmentsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<GetDepartmentsResponseOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GetDepartmentsResponseOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get departments successful.");

        var departmentOut = (GetDepartmentsOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}
public class GetDepartmentsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GetDepartmentsResponseOut> GetDepartmentsResponseOut { get; set; } = new();

    public GetDepartmentsIn In { get; set; } = new GetDepartmentsIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        In.UserId = 1;
        In.UserType = 1;
        var deprtment = DepartmentFactory.SeedMany(db,1);

        foreach (var item in deprtment)
        {
            GetDepartmentsResponseOut obj = new()
            {
                Id = item.Id,
                Name = item.Name,
                DepartmentMangerId = item.DepartmentMangerId
            };
            GetDepartmentsResponseOut.Add(obj);
        }
    }
    public GetDepartmentsHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}
