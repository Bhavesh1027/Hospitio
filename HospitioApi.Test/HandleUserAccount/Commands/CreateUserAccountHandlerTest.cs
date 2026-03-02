using HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleUserAccount.Commands.CreateUserAccountHandlerTestFixture;

namespace HospitioApi.Test.HandleUserAccount.Commands;

public class CreateUserAccountHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateUserAccountHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "User added successfully");
    }

    [Fact]
    public async Task Department_NotExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualDepartmentId = _fix.In.DepartmentId;
        _fix.In.DepartmentId = 0;
        var actualUserLevelId = _fix.In.UserLevelId;
        _fix.In.UserLevelId =   (int)Shared.Enums.UserLevel.Staff;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Department not exist.");

        _fix.In.DepartmentId = actualDepartmentId;
        _fix.In.UserLevelId =  actualUserLevelId;
    }

    [Fact]
    public async Task Group_NotExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualGroupId = _fix.In.GroupId;
        _fix.In.GroupId = 0;
        var actualUserLevelId = _fix.In.UserLevelId;
        _fix.In.UserLevelId =   (int)Shared.Enums.UserLevel.Staff;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Group not exist .");

        _fix.In.GroupId = actualGroupId;
        _fix.In.UserLevelId =  actualUserLevelId;
    }

    [Fact]
    public async Task Email_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        string actualEmail = _fix.In.Email;
        _fix.In.Email = _fix.Email;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Email already exits.");
        _fix.In.Email = actualEmail;
    }
    [Fact]
    public async Task UserName_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        string actualUserName = _fix.In.UserName;
        _fix.In.UserName = _fix.UserName;
        string actualEmail = _fix.In.Email;
        _fix.In.Email = "Unique Mail";
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Username already exits.");
        _fix.In.UserName = actualUserName;
        _fix.In.Email = actualEmail;
    }
    [Fact]
    public async Task UserName_Or_Email_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        string actualUserName = _fix.In.UserName;
        _fix.In.UserName = _fix.Email;
        string actualEmail  = _fix.In.Email;
        _fix.In.Email = "Unique Mail";
       var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Email or username already exits.");
        _fix.In.UserName = actualUserName;
        _fix.In.Email = actualEmail;
    }

}

public class CreateUserAccountHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateUserAccountIn In { get; set; } = new();
    public int UserLevelDepartmentId { get; set; }
    public int UserLevelGroupId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get;set; } = string.Empty;
    public int DepartmentId { get; set; } 
    public int GroupId { get; set; } 
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var department = DepartmentFactory.SeedSingle(db);
        var group = GroupsFactory.SeedSingle(db);
        //var userLevelDepartment = UserLevelFactory.SeedSingle(db, 3);
        var userLevel = UserLevelFactory.SeedSingle(db);
        var user = UserFactory.SeedSingle(db,department.Id,userLevel.Id,group.Id);

        //UserLevelDepartmentId = userLevelDepartment.Id;
        //UserLevelGroupId = userLevelGroup.Id;
        DepartmentId = department.Id;
        GroupId = group.Id;

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

        Email = user.Email;
        UserName = user.UserName;
    }

    public CreateUserAccountHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
