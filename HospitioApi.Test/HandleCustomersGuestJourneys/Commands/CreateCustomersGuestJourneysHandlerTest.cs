using FakeItEasy;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneysHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersGuestJourneys.Commands;

public class CreateCustomersGuestJourneysHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomersGuestJourneysHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.JourneyStep;
        _fix.In.JourneyStep = 2;
        var _VonageService = A.Fake<IVonageService>();

        A.CallTo(() => _VonageService.CreateTemplate(A<string>.Ignored, 0, A<string>.Ignored, A<string>.Ignored, null, CancellationToken.None, MessageSenderEnum.Customer.ToString(), A<string>.Ignored, 0)).WhenArgumentsMatch(x => x.Count() > 0)
        .Returns(_fix.responseData);

        var result = await _fix.BuildHandler(db, _VonageService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer guest journey successful.");
    }
    [Fact]
    public async Task Meta_account_Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _VonageService = A.Fake<IVonageService>();

        int actualCustomerId = _fix.In.CustomerId;
        _fix.In.CustomerId = _fix.CustomerId;
        var result = await _fix.BuildHandler(db, _VonageService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer lacks a proper Meta account or a linked Vonage account, preventing template creation.");
        _fix.In.CustomerId =  actualCustomerId;
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
        var result = await _fix.BuildHandler(db, _VonageService).Handle(new(_fix.In), CancellationToken.None);

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
        var result = await _fix.BuildHandler(db, _VonageService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Please Enter a Right URI or Select a Right Parameter For URL");
        _fix.responseData.Status = actualStatus;
        _fix.responseData.Response = actualResponse;
    }
}
public class CreateCustomersGuestJourneysHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomersGuestJourneysIn In { get; set; } = new();
    public VonageTemplateReponseDto? responseData { get; set; } = new();
    public int CustomerId { get; set; }
    
    protected override async void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var SecondCustomer = CustomerFactory.SeedSingle(db);
        var messageTemplete = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);
        var customerGuestJourney = CustomerGuestJourneyFactory.SeedSingle(db, customer.Id);

        var CustomerVonageCredential = VonageCredentialFactory.SeedSingle(db, customer.Id);
        var vonageNullCreds = VonageCredentialFactory.SeedSingle(db, SecondCustomer.Id,true);

        CustomerId = SecondCustomer.Id;

        In.CustomerId = customer.Id;
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
    public CreateCustomersGuestJourneysHandler BuildHandler(ApplicationDbContext db, IVonageService VonageService) =>
       new(db, Response, VonageService);
}