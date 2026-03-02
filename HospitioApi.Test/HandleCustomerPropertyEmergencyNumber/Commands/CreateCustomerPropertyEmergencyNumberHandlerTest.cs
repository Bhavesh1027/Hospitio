using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands.CreateCustomerPropertyEmergencyNumberHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands
{
    public class CreateCustomerPropertyEmergencyNumberHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateCustomerPropertyEmergencyNumberHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Already_Exists_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);


            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            var phoneNumber = _fix.In.PhoneNumber;

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The property emergency number  {phoneNumber} already exists.");
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create customer property emergency number successful.");
        }

    }

    public class CreateCustomerPropertyEmergencyNumberHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateCustomerPropertyEmergencyNumberIn In { get; set; } = new CreateCustomerPropertyEmergencyNumberIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);

            In.CustomerPropertyInformationId = propertyInfo.Id;
            In.Name = "Test";
            In.PhoneCountry = "USA";
            In.PhoneNumber = "1234567890";
            In.IsActive = true;
        }

        public CreateCustomerPropertyEmergencyNumberHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}



