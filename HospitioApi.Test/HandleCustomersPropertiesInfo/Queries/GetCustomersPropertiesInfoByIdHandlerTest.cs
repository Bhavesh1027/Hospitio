using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Queries
{
    public class GetCustomersPropertiesInfoByIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomersPropertiesInfoByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<CustomersPropertiesInfoByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomersPropertiesInfoByIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomersPropertiesInfoByIdOut.Id },null), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get Customers Property Info successful.");

            var propertyInfoOut = (GetCustomersPropertiesInfoByIdOut)result.Response;
            Assert.NotNull(propertyInfoOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.CustomersPropertiesInfoByIdOut.Id;
            _fix.CustomersPropertiesInfoByIdOut.Id = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomersPropertiesInfoByIdOut.Id }, null), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customers propery info could not be found");

            _fix.CustomersPropertiesInfoByIdOut.Id = actualId;
        }
    }

    public class GetCustomersPropertiesInfoByIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CustomersPropertiesInfoByIdOut CustomersPropertiesInfoByIdOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var propertyInformation = customerProperyInformationFactory.SeedSingle(db);

            CustomersPropertiesInfoByIdOut = new()
            {
                Id = propertyInformation.Id,
                CustomerId = propertyInformation.CustomerId,
                CustomerGuestAppBuilderId = propertyInformation.Id,
                WifiUsername = propertyInformation.WifiUsername,
                WifiPassword = propertyInformation.WifiPassword,
                Overview = propertyInformation.Overview,
                CheckInPolicy = propertyInformation.CheckInPolicy,
                TermsAndConditions = propertyInformation.TermsAndConditions,
                Street = propertyInformation.Street,
                StreetNumber = propertyInformation.StreetNumber,
                City = propertyInformation.City,
                Postalcode = propertyInformation.Postalcode,
                Country = propertyInformation.Country,
                IsActive = propertyInformation.IsActive
            };
        }

        public GetCustomersPropertiesInfoByIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}


