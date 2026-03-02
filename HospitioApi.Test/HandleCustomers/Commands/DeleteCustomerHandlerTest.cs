using FakeItEasy;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using HospitioApi.Core.HandleCustomers.Commands.DeleteCustomer;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Net;
using System.Text;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomers.Commands.DeleteCustomerHandlerFixure;
using Newtonsoft.Json;

namespace HospitioApi.Test.HandleCustomers.Commands
{
    public class DeleteCustomerHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public DeleteCustomerHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);            
            var _http = A.Fake<IHttpClientFactory>();


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

            var result = await _fix.BuildHandler(db, _httpClinetFactoryMock.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete customer successful.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _http = A.Fake<IHttpClientFactory>();

            int actualId = _fix.In.Id;
            _fix.In.Id = 0;

            var result = await _fix.BuildHandler(db,_http).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customer could not be found.");
            _fix.In.Id = actualId;
        }
    }
    public class DeleteCustomerHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions { get; set; } = A.Fake<IOptions<MiddlewareApiSettingsOptions>>();
        public DeleteCustomerIn In { get; set; } = new();
        public HttpResponseMessage responseMessage { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerUsers = CustomerUserFactory.SeedMany(db,5, customer.Id);
            var customerRoomNames = CustomerRoomNamesRepository.SeedMany(db, customer.Id, 2);

            foreach(var customerUser in customerUsers)
            {
                var channels = ChannelUserCustomerUserFactory.SeedSingle(db, customerUser.Id);
                var channelUsers = ChannelUserTypeCustomerUserFactory.SeedSingle(db, channels.Id, customerUser.Id);
            }
            In.Id = customer.Id;

            middlewareApiSettingsOptions.Value.BaseUrl  = "https://localhost:7018/";
            middlewareApiSettingsOptions.Value.UserName  = "UserName";
            middlewareApiSettingsOptions.Value.Password  = "Password";
        }

        public DeleteCustomerHandler BuildHandler(ApplicationDbContext db,IHttpClientFactory httpClientFactory) =>
            new(db, Response, middlewareApiSettingsOptions, httpClientFactory);
    }
}

