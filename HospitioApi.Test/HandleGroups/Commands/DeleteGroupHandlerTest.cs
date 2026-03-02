using HospitioApi.Core.HandleGroups.Commands.DeleteGroup;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Commands.DeleteGroupHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Commands;

public class DeleteGroupHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteGroupHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete group successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.GroupId;
        _fix.In.GroupId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Group with {_fix.In.GroupId} not found.");

        _fix.In.GroupId = actualId;
    }
}

public class DeleteGroupHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteGroupIn In { get; set; } = new DeleteGroupIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var group = GroupsFactory.SeedSingle(db);
        In.GroupId = group.Id;
        In.UserType = 1;
    }

    public DeleteGroupHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}


