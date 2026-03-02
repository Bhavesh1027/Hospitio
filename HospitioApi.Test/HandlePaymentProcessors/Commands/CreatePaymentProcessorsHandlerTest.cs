using HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandlePaymentProcessors.Commands.CreatePaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandlePaymentProcessors.Commands
{
    public class CreatePaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreatePaymentProcessorsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create payment processor successful.");
        }
    }

    public class CreatePaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreatePaymentProcessorsIn In { get; set; } = new CreatePaymentProcessorsIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var user = UserFactory.SeedSingle(db);

            In.Name = "Test";
        }

        public CreatePaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}


