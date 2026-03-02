using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
using HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerReservation.Commands.UpdateCustomerGuestReservationHandlerFixure;

namespace HospitioApi.Test.HandleCustomerReservation.Commands
{
    public class UpdateCustomerGuestReservationHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public UpdateCustomerGuestReservationHandlerTest(ThisTestFixure fix)
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
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Update customer reservation successful.");
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
    }
    public class UpdateCustomerGuestReservationHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CustomerReservationByNumberOut customerReservationOut { get; set; } = new();
        public CustomerGuestByReservationIdOut customerGuestOut { get; set; } = new();
        public UpdateCustomerGuestReservationIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureCreated();
            db.Database.EnsureDeleted();

            var customer = CustomerFactory.SeedSingle(db);
            var customerroomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerGuest = CustomerGuestFactory.SeedSingle(db, reservation.Id);


            customerGuestOut.Id = customerGuest.Id;
            customerReservationOut.Id = reservation.Id;

        }
        public UpdateCustomerGuestReservationHandler BuildHandler(ApplicationDbContext db, IDapperRepository _dapper) => new(db, Response, _dapper);

    }
}


