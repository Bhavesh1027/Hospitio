
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerUsers.Queries.GetCustomerSupervisorUsersHandlerFixure;
using Dapper;
using MediatR;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using FakeItEasy;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerSupervisorUsers; 
namespace HospitioApi.Test.HandleCustomerUsers.Queries
{
    public class GetCustomerSupervisorUsersHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerSupervisorUsersHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAllJsonData<CustomerUserOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get users successful.");

            var departmentOut = (GetCustomerSupervisorUsersOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.responseOut;
            _fix.responseOut = null;

            A.CallTo(() => _dapper.GetAll<CustomerUserOut>(A<string>.Ignored, null, CancellationToken.None,
                  CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(db,_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.responseOut = actualObj;
        }
    }
    public class GetCustomerSupervisorUsersHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerSupervisorUsersIn In { get; set; } = new();
        public List<CustomerUserOut> responseOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);
            var customerUsers = CustomerUserFactory.SeedSingle(db, customer.Id);

            CustomerUserOut customerUserOut = new();
            customerUserOut.Id = customerUsers.Id;
            customerUserOut.FirstName = customerUsers.FirstName;
            customerUserOut.LastName= customerUsers.LastName;
            customerUserOut.Title= customerUsers.Title;
            customerUserOut.Title= customerUsers.Title;
            customerUserOut.ProfilePicture = customerUsers.ProfilePicture;
            responseOut.Add(customerUserOut);

        }
        public GetCustomerSupervisorUsersHandler BuildHandler(ApplicationDbContext db,IDapperRepository _dapper) => new(db,_dapper, Response);
    }
}
