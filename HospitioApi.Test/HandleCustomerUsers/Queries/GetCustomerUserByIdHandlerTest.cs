using FakeItEasy;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserById;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using TheTestFixure = HospitioApi.Test.HandleCustomerUsers.Queries.GetCustomerUserByIdHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Queries
{
    public class GetCustomerUserByIdHandlerTest : IClassFixture<TheTestFixure>
    {
        private readonly TheTestFixure _fix;
        public GetCustomerUserByIdHandlerTest(TheTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();


            A.CallTo(() => _dapper.GetAllJsonData<CustomerUserByIdOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get user successful.");

            var departmentOut = (GetCustomerUserByIdOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.fakeResponseOut;
            _fix.fakeResponseOut = null;

            A.CallTo(() => _dapper.GetAllJsonData<CustomerUserByIdOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.fakeResponseOut = actualObj;
        }
    }
    public class GetCustomerUserByIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerUserByIdIn In { get; set; } = new();
        public List<CustomerUserByIdOut> fakeResponseOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var customer = CustomerFactory.SeedSingle(db);
            var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id);

            CustomerUserByIdOut customerUserByIdOut = new();
            customerUserByIdOut.Id = customerUser.Id;
            customerUserByIdOut.FirstName = customerUser.FirstName;
            customerUserByIdOut.LastName = customerUser.LastName;
            customerUserByIdOut.Email = customerUser.Email;
            customerUserByIdOut.Title = customerUser.Title;
            fakeResponseOut.Add(customerUserByIdOut);
        }
        public GetCustomerUserByIdHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db) => new(_dapper, db, Response);
    }
}
