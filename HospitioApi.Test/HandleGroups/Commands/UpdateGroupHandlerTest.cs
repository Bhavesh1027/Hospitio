using HospitioApi.Core.HandleGroups.Commands.UpdateGroup;
using HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Commands.UpdateGroupHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Commands;

public class UpdateGroupHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public UpdateGroupHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.Name;
        _fix.In.Name = "Test";
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update group successful");
        _fix.In.Name = actualName;

    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new UpdateGroupRequest(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The group {_fix.In.Name} already exists.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Group with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class UpdateGroupHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateGroupIn In { get; set; } = new UpdateGroupIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var department = DepartmentFactory.SeedSingle(db);
        var group = GroupsFactory.SeedSingle(db,department.Id);
        In.UserType = 1;
        In.Id = group.Id;
        In.Name = group.Name;
        In.IsActive = true;
        In.DepartmentId = department.Id;
    }

    public UpdateGroupHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}

