using FakeItEasy;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Net;
using System.Text;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Commands.MusementCreateOrderHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Commands
{
    public class MusementCreateOrderHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public MusementCreateOrderHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();

            var _httpClinetFactoryMock = new Mock<IHttpClientFactory>();
            var _handlerMock = new Mock<HttpMessageHandler>();
            var _httpClientMock = new Mock<HttpClient>();

            int customerId = _fix.CustomerId; // Replace this with the actual customerId you have

            string jsonContent = $@"
{{
  ""identifier"": null,
  ""uuid"": """",
  ""status"": null,
  ""trustpilot_url"": null,
  ""customer"": {{
    ""id"": {customerId},
    ""email"": null,
    ""firstname"": null,
    ""lastname"": null
  }},
  ""items"": [
    {{
      ""uuid"": null,
      ""cart_item_uuid"": null,
      ""transaction_code"": null,
      ""product"": {{
        ""type"": null,
        ""max_confirmation_time"": null,
        ""price_tag"": {{
          ""price_feature"": null,
          ""ticket_holder"": null,
          ""price_feature_code"": null,
          ""ticket_holder_code"": null
        }},
        ""id"": null,
        ""title"": null,
        ""activity_uuid"": null,
        ""api_url"": null,
        ""url"": null,
        ""cover_image_url"": null,
        ""original_retail_price"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""original_retail_price_without_service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""retail_price"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""retail_price_without_service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""discount_amount"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""meeting_point"": null,
        ""meeting_point_markdown"": null,
        ""meeting_point_html"": null
      }},
      ""quantity"": null,
      ""retail_price_in_order_currency"": {{
        ""currency"": null,
        ""value"": null,
        ""formatted_value"": null,
        ""formatted_iso_value"": null
      }},
      ""total_retail_price_in_order_currency"": {{
        ""currency"": null,
        ""value"": null,
        ""formatted_value"": null,
        ""formatted_iso_value"": null
      }},
      ""status"": null,
      ""vouchers"": [
        {{
          ""uuid"": null,
          ""code"": null,
          ""discountAmount"": {{
            ""currency"": null,
            ""value"": null,
            ""formatted_value"": null,
            ""formatted_iso_value"": null
          }},
          ""status"": null,
          ""type"": null
        }}
      ],
      ""is_error_status"": null
    }}
  ],
  ""total_price"": {{
    ""currency"": null,
    ""value"": null,
    ""formatted_value"": null,
    ""formatted_iso_value"": null
  }},
  ""discount_amount"": {{
    ""currency"": null,
    ""value"": null,
    ""formatted_value"": null,
    ""formatted_iso_value"": null
  }},
  ""extra_data"": null,
  ""market"": null,
  ""is_agency"": null,
  ""is_paid"": null
}}
";
            var mockHttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"https://sandbox.musement.com/api/v3/orders/fb7122c6-c0c4-4a3c-9bd7-e2d64f53b6f5")),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockHttpResponseMessage);

            _httpClinetFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));

            var result = await _fix.BuildHandler(_dapper, _httpClinetFactoryMock.Object, db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create Order successful.");
        }
        [Fact]
        public async Task Order_Not_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();

            var _httpClinetFactoryMock = new Mock<IHttpClientFactory>();
            var _handlerMock = new Mock<HttpMessageHandler>();
            var _httpClientMock = new Mock<HttpClient>();

            int customerId = _fix.CustomerId; // Replace this with the actual customerId you have

            string jsonContent = $@"
{{
  ""identifier"": null,
  ""uuid"": """",
  ""status"": null,
  ""trustpilot_url"": null,
  ""customer"": {{
    ""id"": {customerId},
    ""email"": null,
    ""firstname"": null,
    ""lastname"": null
  }},
  ""items"": [
    {{
      ""uuid"": null,
      ""cart_item_uuid"": null,
      ""transaction_code"": null,
      ""product"": {{
        ""type"": null,
        ""max_confirmation_time"": null,
        ""price_tag"": {{
          ""price_feature"": null,
          ""ticket_holder"": null,
          ""price_feature_code"": null,
          ""ticket_holder_code"": null
        }},
        ""id"": null,
        ""title"": null,
        ""activity_uuid"": null,
        ""api_url"": null,
        ""url"": null,
        ""cover_image_url"": null,
        ""original_retail_price"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""original_retail_price_without_service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""retail_price"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""retail_price_without_service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""discount_amount"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""service_fee"": {{
          ""currency"": null,
          ""value"": null,
          ""formatted_value"": null,
          ""formatted_iso_value"": null
        }},
        ""meeting_point"": null,
        ""meeting_point_markdown"": null,
        ""meeting_point_html"": null
      }},
      ""quantity"": null,
      ""retail_price_in_order_currency"": {{
        ""currency"": null,
        ""value"": null,
        ""formatted_value"": null,
        ""formatted_iso_value"": null
      }},
      ""total_retail_price_in_order_currency"": {{
        ""currency"": null,
        ""value"": null,
        ""formatted_value"": null,
        ""formatted_iso_value"": null
      }},
      ""status"": null,
      ""vouchers"": [
        {{
          ""uuid"": null,
          ""code"": null,
          ""discountAmount"": {{
            ""currency"": null,
            ""value"": null,
            ""formatted_value"": null,
            ""formatted_iso_value"": null
          }},
          ""status"": null,
          ""type"": null
        }}
      ],
      ""is_error_status"": null
    }}
  ],
  ""total_price"": {{
    ""currency"": null,
    ""value"": null,
    ""formatted_value"": null,
    ""formatted_iso_value"": null
  }},
  ""discount_amount"": {{
    ""currency"": null,
    ""value"": null,
    ""formatted_value"": null,
    ""formatted_iso_value"": null
  }},
  ""extra_data"": null,
  ""market"": null,
  ""is_agency"": null,
  ""is_paid"": null
}}
";
            var mockHttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"https://sandbox.musement.com/api/v3/orders/fb7122c6-c0c4-4a3c-9bd7-e2d64f53b6f5")),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockHttpResponseMessage);

            _httpClinetFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));

            var result = await _fix.BuildHandler(_dapper, _httpClinetFactoryMock.Object, db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Create order does not exits");
        }
    }
    public class MusementCreateOrderHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public IOptions<MusementSettingsOptions> options { get; set; } = A.Fake<IOptions<MusementSettingsOptions>>();
        public MusementCreateOrderIn In { get; set; } = new();
        public HttpClient httpClient { get; set; } = new();
        public int CustomerId { get; set; }
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

            In.Url = "orders/fb7122c6-c0c4-4a3c-9bd7-e2d64f53b6f5";
            In.Cart_uuid = "e85efb42-e411-46ef-893b-79edbaf53de8";
            In.Email_notification = "NONE";
            In.Guest_Id = $"{guest.Id}";
            In.Order_uuid = "fb7122c6-c0c4-4a3c-9bd7-e2d64f53b6f5";

            CustomerId = customer.Id;
            options.Value.musement_url = "https://sandbox.musement.com/api/v3/";
        }
        public MusementCreateOrderHandler BuildHandler(IDapperRepository dapper, IHttpClientFactory httpClient,ApplicationDbContext db) =>
            new(dapper, Response, httpClient,options,db);
    }
}
