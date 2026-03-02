using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;
using HospitioApi.Core.HandleGroups.Commands.CreateGroup;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfoHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Commands
{
    public class CreateCustomersPropertiesInfoHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateCustomersPropertiesInfoHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create customer property info successful.");
        }
    }

    public class CreateCustomersPropertiesInfoHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateCustomersPropertiesInfoIn In { get; set; } = new CreateCustomersPropertiesInfoIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);

            In.CustomerId = customer.Id;
            In.CustomerGuestAppBuilderId = guestAppBulder.Id;
            In.WifiUsername = "Test";
            In.WifiPassword = "Password";
            In.Overview = "Test Overview";
            In.CheckInPolicy = "Test Policy";
            In.TermsAndConditions = "Term & Condition";
            In.Street = "Test Street";
            In.StreetNumber = "Test Street Number";
            In.City = "Test City";
            In.Postalcode = "Test Postal Code";
            In.Country = "Test Country";
            In.IsActive = true;
        }

        public CreateCustomersPropertiesInfoHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}



