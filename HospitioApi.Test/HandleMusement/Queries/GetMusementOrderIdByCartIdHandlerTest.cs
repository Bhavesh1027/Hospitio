using FakeItEasy;
using HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIdByCartId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Queries.GetMusementOrderIdByCartIdHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Queries
{
    public class GetMusementOrderIdByCartIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetMusementOrderIdByCartIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get orderId successful.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var actualCartId = _fix.In.CartId;
            _fix.In.CartId = 0.ToString();
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.In.CartId = actualCartId;
        }
    }
    public class GetMusementOrderIdByCartIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetMusementOrderIdByCartIdIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var guest = CustomerGuestFactory.SeedSingle(db, reservation.Id);
            var GuestInfo = MusementGuestInfoFactory.SeedSingle(db, guest.Id, customer.Id);
            var OrderInfo = MusementOrderInfoFactory.SeedSingle(db, GuestInfo.Id);

            In.CartId = OrderInfo.CartUUID;
        }
        public GetMusementOrderIdByCartIdHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
    }
}
