using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Queries
{
    public class GetCustomersPropertiesInfoByAppBuilderIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomersPropertiesInfoByAppBuilderIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomersPropertiesInfoByAppBuilderIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomersPropertiesInfoByAppBuilderIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In,_fix.In.CustomerId.ToString(), UserTypeEnum.Customer), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer guest property info successful.");

            var propertyInfoOut = (GetCustomersPropertiesInfoByAppBuilderIdOut)result.Response;
            Assert.NotNull(propertyInfoOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In, _fix.In.CustomerId.ToString(), UserTypeEnum.Customer), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }

    public class GetCustomersPropertiesInfoByAppBuilderIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<CustomersPropertiesInfoByAppBuilderIdOut> CustomersPropertiesInfoByAppBuilderIdOut { get; set; } = new();
        public GetCustomersPropertiesInfoByAppBuilderIdIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);
            var guestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfos = customerProperyInformationFactory.SeedMany(db, customer.Id, guestAppBuilder.Id, 1);

            In.AppBuilderId = guestAppBuilder.Id;
            In.CustomerId = customer.Id;

            foreach (var info in propertyInfos)
            {
                CustomersPropertiesInfoByAppBuilderIdOut obj = new()
                {
                    Id = info.Id,
                    CustomerGuestAppBuilderId = guestAppBuilder.Id,
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
                };
                CustomersPropertiesInfoByAppBuilderIdOut.Add(obj);
            }
        }

        public GetCustomersPropertiesInfoByAppBuilderIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}




