using HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomerCurrencyHandlerFixure;

namespace HospitioApi.Test.HandleCustomers.Queries
{
    public class GetCustomerCurrencyHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomerCurrencyHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer currency successful.");
        }
        [Fact]
        public async Task NotFound_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var actualId = _fix.In.Id;
            _fix.In.Id = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

            _fix.In.Id = actualId;
        }
    }
    public class GetCustomerCurrencyHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerCurrencyIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var customers = CustomerFactory.SeedSingle(db);
            In.Id = customers.Id;
        }
        public GetCustomerCurrencyHandler BuildHandler(ApplicationDbContext _db) => new(_db, Response);
    }
}
