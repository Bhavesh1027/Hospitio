using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
using HospitioApi.Core.HandleLeads.Commands.CreateLead;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleLeads.Commands.CreateLeadHandlerTestFixture;

namespace HospitioApi.Test.HandleLeads.Commands
{
    public class CreateLeadHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateLeadHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Lead created successfully.");
        }
    }

    public class CreateLeadHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateLeadIn In { get; set; } = new CreateLeadIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);

            In.FirstName = "Test";
            In.LastName = "Test";
            In.Company = "Test Company";
            In.Comment = "This is test comment";
            In.Email = "test@gmail.com";
            In.PhoneCountry = "in";
            In.PhoneNumber = "1234567890";
            In.ContactFor = user.Id;
            In.IsActive = true;
    }

        public CreateLeadHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}

