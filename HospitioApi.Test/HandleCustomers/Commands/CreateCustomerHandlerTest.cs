using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using System.Net;
using System.Text;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Commands.CreateCustomerHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomers.Commands;

public class CreateCustomerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _vonageService = A.Fake<IVonageService>();


        var actualName = _fix.In.CustomerUserIn.UserName;
        _fix.In.CustomerUserIn.UserName = "Test Username";

        var _httpContext = new Mock<IHttpContextAccessor>();
        string fakeURI = "https://localhost:7079/index.html";

        _httpContext.Setup(x => x.HttpContext!.Request.Headers.Referer).Returns(fakeURI);

        A.CallTo(() => _fix.FakeSendEmail.ExecuteAsync(A<SendEmailOptions>.Ignored, A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);

        var _httpClinetFactoryMock = new Mock<IHttpClientFactory>();
        var _handlerMock = new Mock<HttpMessageHandler>();
        var _httpClientMock = new Mock<HttpClient>();

        var requestBody = new
        {
            username = _fix.middlewareApiSettingsOptions.Value.UserName,
            password = _fix.middlewareApiSettingsOptions.Value.Password,
        };

        var requestBodyJson = JsonConvert.SerializeObject(requestBody);
        var mockHttpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
        };

        _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post  && x.RequestUri == new Uri("https://localhost:7018/api/ChannelManager/token")),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(mockHttpResponseMessage);

        _httpClinetFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));

        A.CallTo(()=>_vonageService.CreateVonageSubAccount(A<string>.Ignored,A<int>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);
        A.CallTo(()=>_vonageService.CreateApplication(A<string>.Ignored,A<int>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);

        var result = await _fix.BuildHandler(db, _commonRepository,_fix.logger,_fix.FakeHostingEnvironment, _httpContext.Object, _fix.SMTPEmailSettingsOptions,_fix.FakeSendEmail ,_fix.CustomerCredentialSendEmailOptions, _vonageService, _httpClinetFactoryMock.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer successful.");

        _fix.In.CustomerUserIn.UserName = actualName;
    }

    [Fact]
    public async Task BusinessName_AlreadyExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _vonageService = A.Fake<IVonageService>();
        var httpclient = A.Fake<IHttpClientFactory>();

        var actualName = _fix.In.BusinessName;
        _fix.In.BusinessName = "Test";

        var result = await _fix.BuildHandler(db, _commonRepository, _fix.logger, _fix.FakeHostingEnvironment, _fix.FakeHttpContextAccessor, _fix.SMTPEmailSettingsOptions, _fix.FakeSendEmail, _fix.CustomerCredentialSendEmailOptions, _vonageService, httpclient).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The business name {_fix.In.BusinessName} already exists.");

        _fix.In.BusinessName = actualName;
    }

    [Fact]
    public async Task UserName_AlreadyExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _vonageService = A.Fake<IVonageService>();
        var httpclient = A.Fake<IHttpClientFactory>();


        var result = await _fix.BuildHandler(db, _commonRepository, _fix.logger, _fix.FakeHostingEnvironment, _fix.FakeHttpContextAccessor, _fix.SMTPEmailSettingsOptions, _fix.FakeSendEmail, _fix.CustomerCredentialSendEmailOptions,_vonageService, httpclient).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer user name {_fix.In.CustomerUserIn.UserName} already exists.");

    }
}

public class CreateCustomerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerIn In { get; set; } = new CreateCustomerIn();
    public IOptions<CustomerCredentialSendEmailOptions> CustomerCredentialSendEmailOptions { get; set; } = A.Fake<IOptions<CustomerCredentialSendEmailOptions>>();

    public IOptions<SMTPEmailSettingsOptions> SMTPEmailSettingsOptions { get; set; } = A.Fake<IOptions<SMTPEmailSettingsOptions>>();

    public IHttpContextAccessor FakeHttpContextAccessor { get; set; } = A.Fake<IHttpContextAccessor>();
    public ISendEmail FakeSendEmail { get; set; } = A.Fake<ISendEmail>();
    public ILogger<CreateCustomerHandler> logger { get; set; } = A.Fake<ILogger<CreateCustomerHandler>>();
    public IHostingEnvironment FakeHostingEnvironment { get; set; } = A.Fake<IHostingEnvironment>();
    public IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions=A.Fake<IOptions<MiddlewareApiSettingsOptions>>();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id);

        In.BusinessName = "Test businessname";
        In.BusinessTypeId = 1;
        In.NoOfRooms = 2;
        In.PhoneCountry = "Test";
        In.PhoneNumber = "2123456789";
        In.IsActive = true;

        In.CustomerUserIn.CustomerId = customer.Id;
        In.CustomerUserIn.FirstName = customerUser.FirstName;
        In.CustomerUserIn.LastName = customerUser.LastName;
        In.CustomerUserIn.Email = customerUser.Email;
        In.CustomerUserIn.Title = customerUser.Title;
        In.CustomerUserIn.UserName = customerUser.UserName;
        In.CustomerUserIn.Password = customerUser.Password;
        CustomerCredentialSendEmailOptions.Value.EmailTemplate = "customercredsend.html";

        middlewareApiSettingsOptions.Value.BaseUrl  = "https://localhost:7018/";
        middlewareApiSettingsOptions.Value.UserName  = "UserName";
        middlewareApiSettingsOptions.Value.Password  = "Password";
    }

    public CreateCustomerHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository, ILogger<CreateCustomerHandler> logger, IHostingEnvironment _environment, IHttpContextAccessor httpContextAccessor, IOptions<SMTPEmailSettingsOptions> smtpEmailSettings, ISendEmail sendEmail, IOptions<CustomerCredentialSendEmailOptions> customerCredentialSendEmail,IVonageService vonageService,IHttpClientFactory httpClientFactory) =>
        new(db, Response, commonRepository,logger, _environment, httpContextAccessor, smtpEmailSettings, sendEmail, customerCredentialSendEmail,vonageService, middlewareApiSettingsOptions, httpClientFactory);
}
