using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessors;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandleHospitioPaymentProcessors.Queries
{
    public class GetHospitioPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetHospitioPaymentProcessorsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<HospitioPaymentProcessorsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.HospitioPaymentProcessorsOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get hospitio payment processors successful.");

            var hospitioPaymentProcessorsOut = (GetHospitioPaymentProcessorsOut)result.Response;
            Assert.NotNull(hospitioPaymentProcessorsOut);
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

    public class GetHospitioPaymentProcessorsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetHospitioPaymentProcessorsIn In = new GetHospitioPaymentProcessorsIn();
        public List<HospitioPaymentProcessorsOut> HospitioPaymentProcessorsOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

            var hospitioPaymentProcessors = HospitioPaymentProcessorFactory.SeedMany(db, paymentProcessor.Id,1);

            In.PageSize = 10;
            In.PageNo = 1;

            foreach (var hospitioPaymentProcessor in hospitioPaymentProcessors)
            {
                HospitioPaymentProcessorsOut obj = new()
                {
                    Id = hospitioPaymentProcessor.Id,
                    PaymentProcessorId = hospitioPaymentProcessor.Id,
                    //ClientId = hospitioPaymentProcessor.ClientId,
                    //ClientSecret = hospitioPaymentProcessor.ClientSecret,
                    //Currency = hospitioPaymentProcessor.Currency
                };
                HospitioPaymentProcessorsOut.Add(obj);
            }
        }

        public GetHospitioPaymentProcessorsHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}




