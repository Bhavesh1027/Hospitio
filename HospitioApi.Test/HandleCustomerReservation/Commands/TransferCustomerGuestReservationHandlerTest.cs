using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerReservation.Commands.TransferCustomerGuestReservationHandlerFixure;

namespace HospitioApi.Test.HandleCustomerReservation.Commands
{
    public class TransferCustomerGuestReservationHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public TransferCustomerGuestReservationHandlerTest(ThisTestFixure fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            A.CallTo(() => _dapper.GetSingle<CustomerGuestByReservationIdOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerGuestOut);
            var result = await _fix.BuildHandler(db,_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Transfer customer reservation successful.");
        }
        [Fact]
        public async Task CustomerReservation_Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.customerReservationOut;
            _fix.customerReservationOut = null;
            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
                          System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            A.CallTo(() => _dapper.GetSingle<CustomerGuestByReservationIdOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerGuestOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.customerReservationOut = actualObj;
        }
        [Fact]
        public async Task CustomerGuest_Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.customerGuestOut;
            _fix.customerGuestOut = null;
            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
              System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            A.CallTo(() => _dapper.GetSingle<CustomerGuestByReservationIdOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerGuestOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.customerGuestOut =  actualObj;
        }
        [Fact]
        public async Task CustomerGuest_Not_Found()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualId = _fix.customerGuestOut.Id;
            _fix.customerGuestOut.Id = 0;
            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
              System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            A.CallTo(() => _dapper.GetSingle<CustomerGuestByReservationIdOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerGuestOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customer guest could not be found.");
            _fix.customerGuestOut.Id =  actualId;
        }
        [Fact]
        public async Task CustomerReservation_Not_Found()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualId = _fix.customerReservationOut.Id;
            _fix.customerReservationOut.Id = 0;
            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
                          System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            A.CallTo(() => _dapper.GetSingle<CustomerGuestByReservationIdOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerGuestOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customer reservation could not be found.");
            _fix.customerReservationOut.Id= actualId;
        }
    }
    public class TransferCustomerGuestReservationHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public TransferCustomerGuestReservationIn In { get; set; } = new();
        public CustomerReservationByNumberOut customerReservationOut { get; set; } = new();
        public CustomerGuestByReservationIdOut customerGuestOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerroomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerGuest = CustomerGuestFactory.SeedSingle(db, reservation.Id);

            customerGuestOut.Id = customerGuest.Id;
            In.NewLocationCode = customerroomName.Guid;
            customerReservationOut.Id = reservation.Id;

            In.ArrivalDate = DateOnly.FromDateTime((DateTime)reservation.CheckinDate).ToString();
            In.ArrivalTime = TimeOnly.FromDateTime((DateTime)reservation.CheckinDate).ToString();
            In.DepartureDate = TimeOnly.FromDateTime((DateTime)reservation.CheckoutDate).ToString();
            In.DepartureTime = TimeOnly.FromDateTime((DateTime)reservation.CheckoutDate).ToString();
        }

        public TransferCustomerGuestReservationHandler BuildHandler(ApplicationDbContext db,IDapperRepository dapper) =>
            new(db, Response,dapper);
    }
}
