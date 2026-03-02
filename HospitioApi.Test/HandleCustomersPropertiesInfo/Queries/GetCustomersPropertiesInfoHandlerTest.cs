using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfo;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Queries
{
    public class GetCustomersPropertiesInfoHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomersPropertiesInfoHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomersPropertiesInfoOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x!= null && x.Count() > 0).Returns(_fix.CustomersPropertiesInfoOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get property info successful.");

            var propertiesInfoOut = (GetCustomersPropertiesInfoOut)result.Response;
            Assert.NotNull(propertiesInfoOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.In.CustomerId;
            _fix.In.CustomerId = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");

            _fix.In.CustomerId = actualId;
        }
    }

    public class GetCustomersPropertiesInfoHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<CustomersPropertiesInfoOut> CustomersPropertiesInfoOut { get; set; } = new();
        public GetCustomersPropertiesInfoIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);
            var guestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfos = customerProperyInformationFactory.SeedMany(db, customer.Id, guestAppBuilder.Id, 1);

            /*In.SearchColumn = null;*/
            /*In.SearchValue = "test";*/
            In.PageNo = 1;
            In.PageSize = 10;
            /*In.SortColumn = "Id";
            In.SortOrder = "ASC";*/
            In.CustomerId = customer.Id;

            foreach (var info in propertyInfos)
            {
                CustomersPropertiesInfoOut obj = new()
                {
                    Id = info.Id,
                    CustomerId = info.CustomerId,
                    CustomerGuestAppBuilderId = info.CustomerGuestAppBuilderId,
                    WifiUsername = info.WifiUsername,
                    WifiPassword = info.WifiPassword,
                    Overview = info.Overview,
                    CheckInPolicy = info.CheckInPolicy,
                    TermsAndConditions = info.TermsAndConditions,
                    Street = info.Street,
                    StreetNumber = info.StreetNumber,
                    City = info.City,
                    Postalcode = info.Postalcode,
                    Country = info.Country,
                    IsActive = info.IsActive
                };
                CustomersPropertiesInfoOut.Add(obj);
            }
        }

        public GetCustomersPropertiesInfoHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}


