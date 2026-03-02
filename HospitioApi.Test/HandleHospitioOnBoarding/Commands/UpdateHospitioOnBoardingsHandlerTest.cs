using Azure.Core;
using HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardingsTestFixture;

namespace HospitioApi.Test.HandleHospitioOnBoarding.Commands;


public class UpdateHospitioOnBoardingsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateHospitioOnBoardingsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Update_Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update hospitio on boarding successful.");

        //_fix.HospitioOnboardingsFactory.Update(db, new() { Name = actualName });
    }

    [Fact]
    public async Task Create_Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create hospitio on boarding successful.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Not_Exist_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 1;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The hospitio on boarding with given Id {_fix.In.Id} not exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateHospitioOnBoardingsTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateHospitioOnBoardingsIn In { get; set; } = new();
    public int Id { get; set; }
    public HospitioOnboarding HospitioOnboarding { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var hospitioOnboardingObj = HospitioOnboardingsFactory.SeedSingle(db);
        HospitioOnboarding = hospitioOnboardingObj;
    
        In.Id = hospitioOnboardingObj.Id;
        In.WhatsappCountry = "999";
        In.WhatsappNumber = "999";
        In.ViberCountry = "999";
        In.ViberNumber = "999";
        In.TelegramCountry = "999";
        In.TelegramNumber = "999";
        In.PhoneCountry = "999";
        In.PhoneNumber = "999";
        In.SmsTitle = "Test";
        In.Messenger = "Msg";
        In.Email = "Test@test1.com";
        In.Cname = "TestName";
        In.IncomingTranslationLanguage = "Eng";
        In.NoTranslateWords = "Test";
    }

    public UpdateHospitioOnBoardingsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
