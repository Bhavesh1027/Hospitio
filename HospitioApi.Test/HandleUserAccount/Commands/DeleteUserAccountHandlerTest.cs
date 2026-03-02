using HospitioApi.Core.HandleUserAccount.Commands.DeleteUserAccount;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Commands.DeleteUserAccountHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Commands;

public class DeleteUserAccountHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteUserAccountHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualUserId = _fix.In.UserId;
        _fix.In.UserId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, true), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Group with {_fix.In.UserId} not found.");

        _fix.In.UserId = actualUserId;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, true), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete group successful.");
    }
}

public class DeleteUserAccountHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteUserAccountIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var user = UserFactory.SeedSingle(db);
        In.UserId = user.Id;
    }

    public DeleteUserAccountHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
