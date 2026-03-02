using FakeItEasy;
using Moq;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessors;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandlePaymentProcessors.Queries.GetPaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandlePaymentProcessors.Queries
{
    public class GetPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetPaymentProcessorsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<PaymentProcessorsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.PaymentProcessorsOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get payment processor successful.");

            var paymentProcessorsOut = (GetPaymentProcessorsOut)result.Response;
            Assert.NotNull(paymentProcessorsOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetPaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<PaymentProcessorsOut> PaymentProcessorsOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var paymentProcessors = PaymentProcessorFactory.SeedMany(db, 1);


            foreach (var paymentProcessor in paymentProcessors)
            {
                PaymentProcessorsOut obj = new()
                {
                    Id = paymentProcessor.Id,
                    GRName = paymentProcessor.GRName,
                };
                PaymentProcessorsOut.Add(obj);
            }
        }

        public GetPaymentProcessorsHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}



