using FakeItEasy;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorByPaymentProcessorsId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandlePaymentProcessorsDefinations.Queries.GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandlerFixure;

namespace HospitioApi.Test.HandlePaymentProcessorsDefinations.Queries
{
    public class GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            var _response = A.Fake<IHandlerResponseFactory>();

            A.CallTo(() => _dapper.GetSingle<PaymentProcessorsDefinationsByPaymentProcessorsIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get payment processorDetailes successful.");

            var productOut = (GetPaymentProcessorsDefinationsByPaymentProcessorsIdOut)result.Response;
            Assert.NotNull(productOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();
            var _response = A.Fake<IHandlerResponseFactory>();

            A.CallTo(() => _dapper.GetSingle<PaymentProcessorsDefinationsByPaymentProcessorsIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.FakeOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }
    public class GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandlerFixure  : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetPaymentProcessorsDefinationsByPaymentProcessorsIdIn In { get; set; } = new();
        public PaymentProcessorsDefinationsByPaymentProcessorsIdOut Out { get; set; } 
        public PaymentProcessorsDefinationsByPaymentProcessorsIdOut FakeOut { get; set; } 
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);
            var paymentProcessorDefinition = PaymentProcessorsDefinationsFactory.SeedSingle(db,paymentProcessor.Id);
            In.PaymentProcessorId = paymentProcessor.Id;

            Out = new PaymentProcessorsDefinationsByPaymentProcessorsIdOut();
            Out.PaymentProcessorId  = paymentProcessor.Id;
            Out.GRFields  = paymentProcessorDefinition.GRFields;
            Out.GRSupportedCountries  = paymentProcessorDefinition.GRSupportedCountries;
            Out.GRSupportedCurrencies  = paymentProcessorDefinition.GRSupportedCurrencies;
            Out.GRSupportedFeatures  = paymentProcessorDefinition.GRSupportedFeatures;
        }
        public GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandler BuildHandler(IDapperRepository dapper) =>new(dapper,Response);
    }
}
