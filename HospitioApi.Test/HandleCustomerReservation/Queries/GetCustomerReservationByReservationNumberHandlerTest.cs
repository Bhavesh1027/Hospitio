using FakeItEasy;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumberHandlerFixure;

namespace HospitioApi.Test.HandleCustomerReservation.Queries
{
    public class GetCustomerReservationByReservationNumberHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerReservationByReservationNumberHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dappar = A.Fake<IDapperRepository>();
            A.CallTo(() => _dappar.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
                System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationByNumberOut);

            var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer reservation successful.");

            var reservationNumberOut = (GetCustomerReservationByReservationNumberOut)result.Response;
            Assert.NotNull(reservationNumberOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dappar = A.Fake<IDapperRepository>();

            var actualObj = _fix.customerReservationByNumberOut;
            _fix.customerReservationByNumberOut = null;

            A.CallTo(() => _dappar.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
                System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationByNumberOut);

            var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.customerReservationByNumberOut = actualObj;
        }
    }
    public class GetCustomerReservationByReservationNumberHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerReservationByReservationNumberIn In { get; set; } = new();
        public CustomerReservationByNumberOut customerReservationByNumberOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);

            In.ReservationNumber = customerReservation.ReservationNumber;

            customerReservationByNumberOut.Id = customerReservation.Id;
            customerReservationByNumberOut.CustomerId = customerReservation.CustomerId;
            customerReservationByNumberOut.Uuid = customerReservation.Uuid;
            customerReservationByNumberOut.ReservationNumber = customerReservation.ReservationNumber;
            customerReservationByNumberOut.Source = customerReservation.Source;
            customerReservationByNumberOut.NoOfGuestAdults = customerReservation.NoOfGuestAdults;
            customerReservationByNumberOut.NoOfGuestChildrens = customerReservation.NoOfGuestChildrens;
            customerReservationByNumberOut.CheckinDate = customerReservation.CheckinDate;
            customerReservationByNumberOut.CheckoutDate = customerReservation.CheckoutDate;

        }
        public GetCustomerReservationByReservationNumberHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);

    }
}
