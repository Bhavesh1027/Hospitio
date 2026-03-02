using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumbers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Queries.GetCustomerPropertyEmergencyNumbersHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Queries
{
    public class GetCustomerPropertyEmergencyNumbersHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomerPropertyEmergencyNumbersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomerPropertyEmergencyNumbersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerPropertyEmergencyNumbersOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer property emergency number successful.");

            var customerPropertyEmergencyNumberByIdOut = (GetCustomerPropertyEmergencyNumbersOut)result.Response;
            Assert.NotNull(customerPropertyEmergencyNumberByIdOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetCustomerPropertyEmergencyNumbersHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerPropertyEmergencyNumbersIn In = new GetCustomerPropertyEmergencyNumbersIn();
        public List<CustomerPropertyEmergencyNumbersOut> CustomerPropertyEmergencyNumbersOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
            var propertyEmergencyNumbers = CustomerPropertyEmergencyNumberFactory.SeedMany(db, propertyInfo.Id,1);


            In.PropertyId = propertyInfo.Id;

            foreach (var propertyEmergencyNumber in propertyEmergencyNumbers)
            {
                CustomerPropertyEmergencyNumbersOut obj = new()
                {
                    Id = propertyEmergencyNumber.Id,
                    CustomerPropertyInformationId = propertyInfo.Id,
                    Name = propertyEmergencyNumber.Name,
                    PhoneCountry = propertyEmergencyNumber.PhoneCountry,
                    PhoneNumber = propertyEmergencyNumber.PhoneNumber
                };
                CustomerPropertyEmergencyNumbersOut.Add(obj);
            }
        }

        public GetCustomerPropertyEmergencyNumbersHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}





