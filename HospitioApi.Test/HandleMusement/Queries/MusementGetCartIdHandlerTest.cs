using FakeItEasy;
using HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Queries.MusementGetCartIdHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Queries
{
    public class MusementGetCartIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public MusementGetCartIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get CartId successful.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var actualCartId = _fix.In.GuestId;
            _fix.In.GuestId = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.In.GuestId = actualCartId;
        }
    }
    public class MusementGetCartIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public MusementGetCartIdIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var guest = CustomerGuestFactory.SeedSingle(db, reservation.Id);
            var GuestInfo = MusementGuestInfoFactory.SeedSingle(db, guest.Id, customer.Id);

            In.GuestId = guest.Id;
        }
        public MusementGetCartIdHandler BuildHandler(ApplicationDbContext db) =>
           new(db, Response);
    }
}
