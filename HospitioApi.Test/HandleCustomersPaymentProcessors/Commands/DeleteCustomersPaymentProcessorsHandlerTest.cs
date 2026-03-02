using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPaymentProcessors.Commands
{
    public class DeleteCustomersPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public DeleteCustomersPaymentProcessorsHandlerTest(ThisTestFixture fixture)
        {
            _fix = fixture;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete customers payment processors successful.");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.Id;
            _fix.In.Id = 0;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Customers payment pocessors with Id {_fix.In.Id} could not be found.");

            _fix.In.Id = actualId;
        }
    }

    public class DeleteCustomersPaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteCustomersPaymentProcessorsIn In { get; set; } = new DeleteCustomersPaymentProcessorsIn();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);

            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

            var customerPaymentProcessorFactory = CustomerPaymentProcessorFactory.SeedSingle(db, customer.Id, paymentProcessor.Id);

            In.Id = customerPaymentProcessorFactory.Id;
        }

        public DeleteCustomersPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}

