using Humanizer;
using HospitioApi.Core.HandleDepartment.Commands.EditDepartment;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleDepartment.Commands.EditDepartmentHandlerTestFixture;

namespace HospitioApi.Test.HandleDepartment.Commands;

public class EditDepartmentHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public EditDepartmentHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In,_fix.Id),CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Edit department successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Department not found.");

        _fix.Id = actualId;
    }
}
public class EditDepartmentHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditDepartmentIn In { get; set; } = new();
    public int Id { get; set; }
    public Department Department { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var department = DepartmentFactory.SeedSingle(db);
        Department = department;

        Id = department.Id;
        In.UserType = 1;
        In.Name = "Test";
        In.IsActive = true;
    }
    public EditDepartmentHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}