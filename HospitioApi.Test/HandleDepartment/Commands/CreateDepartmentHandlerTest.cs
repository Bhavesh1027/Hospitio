using HospitioApi.Core.HandleDepartment.Commands.CreateDepartment;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
//using ThisTestFixture = HospitioApi.Test.HandleCustomerGuest.Commands.CreateDeparmentHandlerTestFixture;
using ThisTestFixture = HospitioApi.Test.HandleDepartment.Commands.CreateDeparmentHandlerTestFixture;
namespace HospitioApi.Test.HandleDepartment.Commands;

public class CreateDepartmentHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateDepartmentHandlerTest(ThisTestFixture fix)
    {
        _fix = fix;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualName = _fix.In.Name;
        _fix.In.Name = "UniqueName";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create department successful.");
        _fix.In.Name = actualName;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The department name already exists in the system.");
    }
}

public class CreateDeparmentHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateDepartmentIn In { get; set; } = new CreateDepartmentIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var department = DepartmentFactory.SeedSingle(db);
        In.Name = department.Name;
    }

    public CreateDepartmentHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
