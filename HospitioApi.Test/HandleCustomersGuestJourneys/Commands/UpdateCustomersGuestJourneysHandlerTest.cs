using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneysHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersGuestJourneys.Commands;

public class UpdateCustomersGuestJourneysHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateCustomersGuestJourneysHandlerTest(ThisTestFixture fixture)
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
        var result = await _fix.BuildHandler(db, _VonageService, _fix.VonageOptions).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer guest journey successful");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();
        var actualId = _fix.In.Id;
        var actualName = _fix.In.JourneyStep;
        _fix.In.JourneyStep = 2;
        _fix.In.Id = 0;
        var result = await _fix.BuildHandler(db, _VonageService, _fix.VonageOptions).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers guest journey with Id {_fix.In.Id} could not be found.");
        _fix.In.Id = actualId;
        _fix.In.JourneyStep = actualName;
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
        var result = await _fix.BuildHandler(db, _VonageService, _fix.VonageOptions).Handle(new(_fix.In), CancellationToken.None);


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
        var result = await _fix.BuildHandler(db, _VonageService, _fix.VonageOptions).Handle(new(_fix.In), CancellationToken.None);


        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Please Enter a Right URI or Select a Right Parameter For URL");
        _fix.responseData.Status = actualStatus;
        _fix.responseData.Response = actualResponse;
    }
}

public class UpdateCustomersGuestJourneysHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomersGuestJourneysIn In { get; set; } = new();
    public IOptions<VonageSettingsOptions>   VonageOptions { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
    public VonageTemplateReponseDto? responseData { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var messageTemplete = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);
        var guestJourny = CustomerGuestJourneyFactory.SeedSingle(db, customer.Id);
        var CustomerVonageCredential = VonageCredentialFactory.SeedSingle(db, customer.Id);

        In.Id = guestJourny.Id;
        In.JourneyStep = 1;
        In.Name = "Test";
        In.SendType = 1;
        In.TimingOption1 = 1;
        In.TimingOption2 = 1;
        In.TimingOption3 = 1;
        In.Timing = 1;
        In.NotificationDays = "1,2,3";
        In.NotificationTime = null;
        In.GuestJourneyMessagesTemplateId = messageTemplete.Id;
        In.TempletMessage = messageTemplete.TempletMessage;

        responseData.Status = "success";
        responseData.Response = "{\"title\":\"Unauthorised\",\"detail\":\"Invalid Token\"}";
        responseData.Buttons = "";
        responseData.TemplateName = "onlinecheckin_2039";
    }
    public UpdateCustomersGuestJourneysHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService,IOptions<VonageSettingsOptions> options) => new(db, Response, vonageService, VonageOptions);
}
