using HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Commands.UpdateUserStatusHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Commands;

public class UpdateUserStatusHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateUserStatusHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualUserId = _fix.In.UserId;
        _fix.In.UserId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"User request not found.");

        _fix.In.UserId = actualUserId;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update request status successfully.");
    }
}

public class UpdateUserStatusHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateUserStatusIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var user = UserFactory.SeedSingle(db);
        In.UserId = user.Id;
    }

    public UpdateUserStatusHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
