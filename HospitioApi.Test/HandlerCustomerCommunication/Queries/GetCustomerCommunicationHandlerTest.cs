using FakeItEasy;
using HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunication;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandlerCustomerCommunication.Queries.GetCustomerCommunicationHandlerFixture;

namespace HospitioApi.Test.HandlerCustomerCommunication.Queries
{
    public class GetCustomerCommunicationHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerCommunicationHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomerCommunicationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customers lists successfully.");

            var resultOut = (GetCustomerCommunicationOut)result.Response;
            Assert.NotNull(resultOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.responseOut;
            _fix.responseOut = null;
            A.CallTo(() => _dapper.GetAll<CustomerCommunicationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

            _fix.responseOut = actualObj;
        }
    }
    public class GetCustomerCommunicationHandlerFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerCommunicationIn In { get; set; } = new();
        public List<CustomerCommunicationOut> responseOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var cuustoerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
            var customerguest = CustomerGuestFactory.SeedSingle(db, cuustoerReservation.Id);

            CustomerCommunicationOut customerCommunicationOut = new CustomerCommunicationOut();
            customerCommunicationOut.Id = customerguest.Id;
            customerCommunicationOut.CustomerId = cuustoerReservation.CustomerId;
            customerCommunicationOut.CustomerReservationId = cuustoerReservation.Id;
            customerCommunicationOut.FirstName = customerguest.Firstname;
            customerCommunicationOut.LastName = customerguest.Lastname;
            customerCommunicationOut.Email = customerguest.Email;
            customerCommunicationOut.Picture = customerguest.Picture;
            customerCommunicationOut.PhoneCountry = customerguest.PhoneCountry;
            customerCommunicationOut.PhoneNumber = customerguest.PhoneNumber;
            customerCommunicationOut.ChatType = "inbox";
            responseOut.Add(customerCommunicationOut);

        }
        public GetCustomerCommunicationHandler BuildHandler(IDapperRepository _dapper) =>
           new(_dapper, Response);
    }
}
