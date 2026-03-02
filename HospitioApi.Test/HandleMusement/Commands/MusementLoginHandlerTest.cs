using FakeItEasy;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using HospitioApi.Core.HandleMusement.Commands.MusementLogin;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Net;
using System.Text;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Commands.MusementLoginHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Commands
{
    public class MusementLoginHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public MusementLoginHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Login_Fail_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();

            var _httpClinetFactoryMock = new Mock<IHttpClientFactory>();
            var _handlerMock = new Mock<HttpMessageHandler>();
            var _httpClientMock = new Mock<HttpClient>();

            string jsonContent = $@"
{{
  ""access_token"": null,
  ""expires_in"": 20,
  ""token_type"": ""test"",
  ""scope"": null
}}
";
            var mockHttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"https://sandbox.musement.com/api/v3/login")),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockHttpResponseMessage);

            _httpClinetFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));

            var result = await _fix.BuildHandler(_dapper, _httpClinetFactoryMock.Object, _fix.options).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Login failed");
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();

            var _httpClinetFactoryMock = new Mock<IHttpClientFactory>();
            var _handlerMock = new Mock<HttpMessageHandler>();
            var _httpClientMock = new Mock<HttpClient>();

            string jsonContent = $@"
{{
  ""access_token"": null,
  ""expires_in"": 20,
  ""token_type"": ""test"",
  ""scope"": null
}}
";
            var mockHttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"https://sandbox.musement.com/api/v3/login")),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockHttpResponseMessage);

            _httpClinetFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));

            var result = await _fix.BuildHandler(_dapper, _httpClinetFactoryMock.Object, _fix.options).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Login successful.");
        }
    }
    public class MusementLoginHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public IOptions<MusementSettingsOptions> options { get; set; } = A.Fake<IOptions<MusementSettingsOptions>>();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var customer = CustomerFactory.SeedSingle(db);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var guest = CustomerGuestFactory.SeedSingle(db, reservation.Id);
            var GuestInfo = MusementGuestInfoFactory.SeedSingle(db, guest.Id, customer.Id);
            var OrderInfo = MusementOrderInfoFactory.SeedSingle(db, GuestInfo.Id);

            options.Value.musement_url = "https://sandbox.musement.com/api/v3/";
            options.Value.client_id = "576_5umb65zgnqo8os0oo88gw0scc4s8g804o8s0gc0o8g8s8gwkww";
            options.Value.client_secret = "5fhcrstq0yskwk8wc4ockwo808gs8ow4sco00wcckgkws0ksww";
            options.Value.grant_type = "client_credentials";
        }

        public MusementLoginHandler BuildHandler(IDapperRepository dapper, IHttpClientFactory httpClientFactory, IOptions<MusementSettingsOptions> options) =>
            new(dapper, Response, httpClientFactory, options);
    }
}
