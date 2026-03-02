using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplatesHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands;

public class CreateGuestJourneyMessagesTemplatesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateGuestJourneyMessagesTemplatesHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _vonageService = A.Fake<IVonageService>();

        var actualName = _fix.In.Name;
        _fix.In.Name = "Test2";
        A.CallTo(() => _vonageService.CreateTemplate(A<string>.Ignored, 0, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
       .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _vonageService, _fix.VonageSettingOption).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create guest journey message templates successful.");
    }
    [Fact]
    public async Task Buttons__Valid_PhoneNumber_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();

        var actualStatus = _fix.responseData.Status;
        var actualResponse = _fix.responseData.Response;
        _fix.responseData.Status = "failed";
        _fix.responseData.Response = "{\"title\":\"Unauthorised\",\"detail\":\"not a valid phone number\"}";

        A.CallTo(() => _VonageService.CreateTemplate(A<string>.Ignored, 0, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
       .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _VonageService,_fix.VonageSettingOption).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Please Enter a Right Phone Number or Select a Right Parameter For Phone Number");
        _fix.responseData.Status = actualStatus;
        _fix.responseData.Response = actualResponse;
    }
    [Fact]
    public async Task Buttons__Valid_URI_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();

        var actualStatus = _fix.responseData.Status;
        var actualResponse = _fix.responseData.Response;
        _fix.responseData.Status = "failed";
        _fix.responseData.Response = "{\"title\":\"Unauthorised\",\"detail\":\"not a valid URI\"}";

        A.CallTo(() => _VonageService.CreateTemplate(A<string>.Ignored, 0, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
       .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _VonageService,_fix.VonageSettingOption).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Please Enter a Right URI or Select a Right Parameter For URL");
        _fix.responseData.Status = actualStatus;
        _fix.responseData.Response = actualResponse;
    }
}
public class CreateGuestJourneyMessagesTemplatesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateGuestJourneyMessagesTemplatesIn In { get; set; } = new();
    public IOptions<VonageSettingsOptions> VonageSettingOption { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
    public VonageTemplateReponseDto responseData { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();        

        var messagesTemplate = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);
        In.Name = messagesTemplate.Name;
        In.TempleteType = 1;
        In.TempletMessage = "Test";
        In.IsActive = true;

        responseData.Status = "success";
        responseData.Response = "{\"title\":\"Unauthorised\",\"detail\":\"Invalid Token\"}";
        responseData.Buttons = "";
        responseData.TemplateName = "onlinecheckin_2039";

    }
    public CreateGuestJourneyMessagesTemplatesHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService, IOptions<VonageSettingsOptions> VonageSettingOption) =>

       new(db,Response, vonageService, VonageSettingOption);
}