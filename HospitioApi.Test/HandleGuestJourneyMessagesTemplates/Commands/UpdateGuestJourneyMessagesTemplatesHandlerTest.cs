using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplates;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplatesHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands;

public class UpdateGuestJourneyMessagesTemplatesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateGuestJourneyMessagesTemplatesHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();
        A.CallTo(() => _VonageService.UpdateTemplate(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
      .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _VonageService, _fix.vonageOptions).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update GuestJourneyMessagesTemplates successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();
        var actualId = _fix.In.Id;
        var actualName = _fix.In.Name;
        _fix.In.Name = "TestTest";
        _fix.In.Id = 0;
        var result = await _fix.BuildHandler(db, _VonageService, _fix.vonageOptions).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"GuestJourneyMessagesTemplates with Id {_fix.In.Id} could not be found.");
        _fix.In.Id = actualId;
        _fix.In.Name = actualName;
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

        A.CallTo(() => _VonageService.UpdateTemplate(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
       .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _VonageService, _fix.vonageOptions).Handle(new(_fix.In), CancellationToken.None);


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

        A.CallTo(() => _VonageService.UpdateTemplate(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
       .Returns(_fix.responseData);
        var result = await _fix.BuildHandler(db, _VonageService, _fix.vonageOptions).Handle(new(_fix.In), CancellationToken.None);


        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Please Enter a Right URI or Select a Right Parameter For URL");
        _fix.responseData.Status = actualStatus;
        _fix.responseData.Response = actualResponse;
    }
}

public class UpdateGuestJourneyMessagesTemplatesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateGuestJourneyMessagesTemplatesIn In { get; set; } = new();
    public IVonageService vonageService { get; set; } = A.Fake<IVonageService>();
    public IOptions<VonageSettingsOptions> vonageOptions { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
    public VonageTemplateReponseDto responseData { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var guestJourneyMessages = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);

        In.Id = guestJourneyMessages.Id;
        In.Name = guestJourneyMessages.Name;
        In.TempleteType = 1;
        In.TempletMessage = "Test";
        In.IsActive = true;

        responseData.Status = "success";
        responseData.Response = "{\"title\":\"Unauthorised\",\"detail\":\"Invalid Token\"}";
        responseData.Buttons = "";
        responseData.TemplateName = "onlinecheckin_2039";
    }
    public UpdateGuestJourneyMessagesTemplatesHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService,IOptions<VonageSettingsOptions> vonageOptions) => new(db, Response, vonageService,vonageOptions);
}