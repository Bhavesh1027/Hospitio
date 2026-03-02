using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;
using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleDepartment.Queries.GetDepartmentByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleDepartment.Queries;

public class GetDepartmentByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetDepartmentByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;    
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(()=> _dapper.GetSingle<DepartmentsOut>(A<string>.Ignored,null,CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.DepartmentsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.Id), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get department successful.");

        var departmentOut = (GetDepartmentByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.DepartmentsOut.Id;
        _fix.DepartmentsOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.DepartmentsOut.Id = actualId;
    }
}
public class GetDepartmentByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DepartmentsOut DepartmentsOut { get; set; } = new DepartmentsOut();

    public int Id { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var department = DepartmentFactory.SeedSingle(db);

        DepartmentsOut = new()
        {
            Id = department.Id,
            Name = department.Name,
            ManagerId = department.DepartmentMangerId,
        };
    }
    public GetDepartmentByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}