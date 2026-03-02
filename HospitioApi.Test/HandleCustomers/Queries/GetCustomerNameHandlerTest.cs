using HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomerNameHandlerFixure;

namespace HospitioApi.Test.HandleCustomers.Queries
{
    public class GetCustomerNameHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomerNameHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customerName successful.");
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
    public class GetCustomerNameHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerNameIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customers = CustomerFactory.SeedSingle(db);
            In.Id = customers.Id;
        }
        public GetCustomerNameHandler BuildHandler(ApplicationDbContext _db) => new(_db, Response);

    }
}
