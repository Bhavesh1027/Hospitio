using FakeItEasy;
using Humanizer;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
using HospitioApi.Core.HandleCustomerReservation.Commands.ExtendCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using ThisTestFixure = HospitioApi.Test.HandleCustomerReservation.Commands.ExtendCustomerGuestReservationHandlerFixure;

namespace HospitioApi.Test.HandleCustomerReservation.Commands
{
    public class ExtendCustomerGuestReservationHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public ExtendCustomerGuestReservationHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Extend customer reservation successful.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actulaObj = _fix.customerReservationOut;
            _fix.customerReservationOut = null;
            A.CallTo(() => _dapper.GetSingle<CustomerReservationByNumberOut>(A<string>.Ignored, null, CancellationToken.None,
               System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerReservationOut);
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.customerReservationOut = actulaObj;
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
            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customer reservation could not be found.");
            _fix.customerReservationOut.Id= actualId;
        }
    }
    public class ExtendCustomerGuestReservationHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public ExtendCustomerGuestReservationIn In { get; set; } = new();
        public CustomerReservationByNumberOut customerReservationOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerroomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var reservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerGuest = CustomerGuestFactory.SeedSingle(db, reservation.Id);


            customerReservationOut.Id = reservation.Id;

            In.DepartureDate = TimeOnly.FromDateTime((DateTime)reservation.CheckoutDate).ToString();
            In.DepartureTime = TimeOnly.FromDateTime((DateTime)reservation.CheckoutDate).ToString();
        }

        public ExtendCustomerGuestReservationHandler BuildHandler(ApplicationDbContext db, IDapperRepository dapper) =>
            new(db, Response, dapper);
    }
}
