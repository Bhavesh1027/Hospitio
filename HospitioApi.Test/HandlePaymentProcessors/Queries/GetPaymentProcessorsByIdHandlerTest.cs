using FakeItEasy;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandlePaymentProcessors.Queries.GetPaymentProcessorsByIdHandlerTestFixture;

namespace HospitioApi.Test.HandlePaymentProcessors.Queries
{
    public class GetPaymentProcessorsByIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetPaymentProcessorsByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<PaymentProcessorsByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.PaymentProcessorsByIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get payment processor successful.");

            var paymentProcessorOut = (GetPaymentProcessorsByIdOut)result.Response;
            Assert.NotNull(paymentProcessorOut);
        }

        [Fact]
        public async Task NotFound_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            PaymentProcessorsByIdOut? obj = null;
            A.CallTo(() => _dapper.GetSingle<PaymentProcessorsByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }

    public class GetPaymentProcessorsByIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetPaymentProcessorsByIdIn In { get; set; } = new GetPaymentProcessorsByIdIn();
        public PaymentProcessorsByIdOut PaymentProcessorsByIdOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

            PaymentProcessorsByIdOut = new()
            {
                Id = paymentProcessor.Id,
                Name = paymentProcessor.GRName
            };
        }

        public GetPaymentProcessorsByIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}


