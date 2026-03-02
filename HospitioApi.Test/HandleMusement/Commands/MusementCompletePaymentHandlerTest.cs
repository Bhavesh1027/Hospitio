using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleMusement.Commands.MusementCompletePayment;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleMusement.Commands.MusementCompletePaymentHandlerFixure;

namespace HospitioApi.Test.HandleMusement.Commands
{
    public class MusementCompletePaymentHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public MusementCompletePaymentHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var _dapper  = A.Fake<IDapperRepository>();
            var _httpClinet  = A.Fake<IHttpClientFactory>();

            var result = await _fix.BuildHandler(_dapper,_httpClinet ,_fix.options,db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create MusementPayment successful.");
        }
    }
    public class MusementCompletePaymentHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public MusementCompletePaymentIn In { get; set; } = new();
        public IOptions<MusementSettingsOptions> options { get; set; } = A.Fake<IOptions<MusementSettingsOptions>>(); 
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var guest = CustomerGuestFactory.SeedSingle(db, reservation.Id);
            var GuestInfo = MusementGuestInfoFactory.SeedSingle(db,guest.Id,customer.Id);
            var OrderInfo = MusementOrderInfoFactory.SeedSingle(db, GuestInfo.Id);

            In.order_uuid = OrderInfo.OrderUUID;
            In.GuestId = GuestInfo.CustomerGuestId.ToString();
            In.CustomerId= GuestInfo.MusementCustomerId.ToString();
            In.PaymentMethod = "STRIPE";
        }
        public MusementCompletePaymentHandler BuildHandler(IDapperRepository dapper, IHttpClientFactory httpClientFactory, IOptions<MusementSettingsOptions> options,ApplicationDbContext db) =>
            new(dapper, Response,httpClientFactory,options,db);
    }
}
