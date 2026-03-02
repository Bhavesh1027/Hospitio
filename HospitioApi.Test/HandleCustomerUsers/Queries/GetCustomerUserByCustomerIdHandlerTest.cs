using FakeItEasy;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserByCustomerId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerUsers.Queries.GetCustomerUserByCustomerIdHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Queries
{
    public class GetCustomerUserByCustomerIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerUserByCustomerIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();


            A.CallTo(() => _dapper.GetAll<CustomerUsersByCustomerIdOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.CustomerId, _fix.UserId), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer staff successful.");

            var departmentOut = (GetCustomerUserByCustomerIdOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.fakeResponseOut;
            _fix.fakeResponseOut = null;

            A.CallTo(() => _dapper.GetAll<CustomerUsersByCustomerIdOut>(A<string>.Ignored, null, CancellationToken.None,
                  CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.CustomerId, _fix.UserId), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.fakeResponseOut = actualObj;
        }
    }
    public class GetCustomerUserByCustomerIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public string CustomerId { get; set; }
        public int UserId { get; set; }
        public List<CustomerUsersByCustomerIdOut> fakeResponseOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id);

            CustomerId = customer.Id.ToString();
            CustomerUsersByCustomerIdOut response = new();
            response.Id = customerUser.Id;
            response.Name = customerUser.FirstName;
            response.PhoneNumber = customerUser.PhoneNumber;
            response.PhoneCountry = customerUser.PhoneCountry;
            fakeResponseOut.Add(response);
        }
        public GetCustomerUserByCustomerIdHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db) => new(_dapper, Response, db);
    }
}
