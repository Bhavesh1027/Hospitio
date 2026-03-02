using Azure.Core;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using HospitioApi.Core.HandleLeads.Commands.DeleteLead;
using HospitioApi.Core.HandlePaymentProcessors.Commands.DeletePaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandlePaymentProcessors.Commands.DeletePaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandlePaymentProcessors.Commands
{
    public class DeletePaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public DeletePaymentProcessorsHandlerTest(ThisTestFixture fixture)
        {
            _fix = fixture;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete payment processors successful");
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.Id;
            _fix.In.Id = 0;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Payment proccessor with Id {_fix.In.Id} could not be found.");

            _fix.In.Id = actualId;
        }
    }

    public class DeletePaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeletePaymentProcessorsIn In { get; set; } = new DeletePaymentProcessorsIn();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

            In.Id = paymentProcessor.Id;
        }

        public DeletePaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}


