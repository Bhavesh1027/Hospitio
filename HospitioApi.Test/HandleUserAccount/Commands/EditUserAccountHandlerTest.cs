using HospitioApi.Core.HandleUserAccount.Commands.EditUserAccount;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Commands.EditUserAccountHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Commands;

public class EditUserAccountHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public EditUserAccountHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success_Department()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        _fix.In.UserLevelId = _fix.UserLevelDepartmentId;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "User edited successfully");
    }

    [Fact]
    public async Task Success_Group()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        _fix.In.UserLevelId = _fix.UserLevelGroupId;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "User edited successfully");
    }

    [Fact]
    public async Task Department_NotExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualDepartmentId = _fix.In.DepartmentId;
        _fix.In.DepartmentId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Department not exist.");

        _fix.In.DepartmentId = actualDepartmentId;
    }

    [Fact]
    public async Task Group_NotExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualGroupId = _fix.In.GroupId;
        _fix.In.GroupId = 0;
        _fix.In.UserLevelId = (int)Shared.Enums.UserLevel.Guest;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Group not exist .");

        _fix.In.GroupId = actualGroupId;
    }

    [Fact]
    public async Task UserLevelType_AlreadyExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualUserId = _fix.In.Id;
        _fix.In.Id = 0;
        _fix.In.UserLevelId = _fix.UserLevelDepartmentId;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Department manager of given department already exist.");

        _fix.In.Id = actualUserId;
    }
}

public class EditUserAccountHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditUserAccountIn In { get; set; } = new();
    public int UserLevelDepartmentId { get; set; }
    public int UserLevelGroupId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var department = DepartmentFactory.SeedSingle(db);
        var group = GroupsFactory.SeedSingle(db);
        var userLevelDepartment = UserLevelFactory.SeedSingle(db, 3);
        var userLevelGroup = UserLevelFactory.SeedSingle(db, 5);
        var user = UserFactory.SeedSingle(db);

        UserLevelDepartmentId = userLevelDepartment.Id;
        UserLevelGroupId = userLevelGroup.Id;

        In.Id = user.Id;
        In.FirstName = "Test First";
        In.LastName = "Test Last";
        In.Email = "Test Email";
        In.Title = "Test Title";
        In.ProfilePicture = "Test ProfilePicture";
        In.PhoneCountry = "Test PhoneCountry";
        In.PhoneNumber = "1234567890";
        In.DepartmentId = department.Id;
        In.GroupId = group.Id;
        In.UserName = "Test Username";
        In.Password = "12345";
        In.IsActive = true;
    }

    public EditUserAccountHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
