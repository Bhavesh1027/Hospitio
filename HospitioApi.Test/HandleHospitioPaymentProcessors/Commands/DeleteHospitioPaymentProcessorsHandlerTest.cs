using HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.DeleteHospitioPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioPaymentProcessors.Commands.DeleteHospitioPaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandleHospitioPaymentProcessors.Commands
{
    public class DeleteHospitioPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public DeleteHospitioPaymentProcessorsHandlerTest(ThisTestFixture fixture)
        {
            _fix = fixture;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete hospitio payment processor successful.");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.Id;
            _fix.In.Id = 0;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Hospitio payment processor with Id {_fix.In.Id} could not be found.");

            _fix.In.Id = actualId;
        }
    }

    public class DeleteHospitioPaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteHospitioPaymentProcessorsIn In { get; set; } = new DeleteHospitioPaymentProcessorsIn();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();


            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

            var hospitioPaymentProcessorFactory = HospitioPaymentProcessorFactory.SeedSingle(db, paymentProcessor.Id);

            In.Id = hospitioPaymentProcessorFactory.Id;
        }

        public DeleteHospitioPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}


