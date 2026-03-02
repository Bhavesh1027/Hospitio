using FakeItEasy;
using HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunicationByReservationId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandlerCustomerCommunication.Queries.GetCustomerCommunicationByReservationIdHandlerFixure;

namespace HospitioApi.Test.HandlerCustomerCommunication.Queries
{
    public class GetCustomerCommunicationByReservationIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerCommunicationByReservationIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GetCustomerCommunicationByReservationIdResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.ResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customers guest reservation successfully.");

            var resultOut = (GetCustomerCommunicationByReservationOut)result.Response;
            Assert.NotNull(resultOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.ResponseOut;
            _fix.ResponseOut = null;

            A.CallTo(() => _dapper.GetSingle<GetCustomerCommunicationByReservationIdResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.ResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

            _fix.ResponseOut = actualObj;
        }
    }
    public class GetCustomerCommunicationByReservationIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerCommunicationByReservationIdIn In { get; set; } = new();
        public GetCustomerCommunicationByReservationIdResponseOut ResponseOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var cuustoerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerguest = CustomerGuestFactory.SeedSingle(db, cuustoerReservation.Id);

            ResponseOut.Id = cuustoerReservation.Id;
            ResponseOut.FirstName = customerguest.Firstname;
            ResponseOut.LastName = customerguest.Lastname;
            ResponseOut.Email = customerguest.Email;
            ResponseOut.ProfilePicture = customerguest.Picture;
            ResponseOut.PhoneCountry = customerguest.PhoneCountry;
            ResponseOut.PhoneNumber = customerguest.PhoneNumber;
            ResponseOut.CreatedAt = cuustoerReservation.CreatedAt;
            ResponseOut.CheckinDate = cuustoerReservation.CheckinDate;
            ResponseOut.CheckoutDate = cuustoerReservation.CheckoutDate;
        }
        public GetCustomerCommunicationByReservationIdHandler BuildHandler(IDapperRepository _dapper) =>
                new(_dapper, Response);
    }
}
