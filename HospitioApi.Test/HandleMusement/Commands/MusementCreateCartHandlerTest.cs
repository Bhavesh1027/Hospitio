using FakeItEasy;
using HospitioApi.Core.HandleMusement.Commands.MusementCreateCart;
using HospitioApi.Core.HandlePaymentProcessors.Commands.CreatePaymentProcessors;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Commands.MusementCreateCartHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Commands
{
    public class MusementCreateCartHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public MusementCreateCartHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var actualCartId = _fix.In.cartid;
            _fix.In.cartid = 0.ToString();
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create CartId successful.");
            _fix.In.cartid = actualCartId;
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper = A.Fake<IDapperRepository>();
            var _httpClinet = A.Fake<IHttpClientFactory>();

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Cart Id alredy exits");
        }
    }
    public class MusementCreateCartHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public MusementCreateCartIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerGuest = CustomerGuestFactory.SeedSingle(db,reservation.Id); 
            var musementGuestInfo = MusementGuestInfoFactory.SeedSingle(db,customerGuest.Id,customer.Id);

            In.guestId = customerGuest.Id.ToString();
            In.cartid = musementGuestInfo.CartUUID;
        }
        public MusementCreateCartHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}
