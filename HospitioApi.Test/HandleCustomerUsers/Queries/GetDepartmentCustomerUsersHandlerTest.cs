using FakeItEasy;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatId;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;
using HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerUsers.Queries.GetDepartmentCustomerUsersHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Queries
{
    public class GetDepartmentCustomerUsersHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetDepartmentCustomerUsersHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAllJsonData<CustomerDepartmentsOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get users successful.");

            var departmentOut = (GetDepartmentCustomerUsersOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.fakeResponseOut;
            _fix.fakeResponseOut = null;
            A.CallTo(() => _dapper.GetAllJsonData<CustomerDepartmentsOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.fakeResponseOut = actualObj;
        }
    }
    public class GetDepartmentCustomerUsersHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetDepartmentCustomerUsersIn In { get; set; } = new();
        public List<CustomerDepartmentsOut> fakeResponseOut { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerLevel = CustomerLevelFactory.SeedSingle(db,2);
            var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id,2);
            var customerDepartment = CustomerDepartmentsFactory.SeedSingle(db,customer.Id);

            CustomerDepartmentsOut customerDepartmentsOut = new();
            customerDepartmentsOut.Id = customerDepartment.Id;
            customerDepartmentsOut.Name = customerDepartment.Name   ;
            customerDepartmentsOut.CeoId = customerUser.Id   ;
            customerDepartmentsOut.CeoName = customerUser.FirstName + customerUser.LastName;
            fakeResponseOut.Add(customerDepartmentsOut);
        }
        public GetDepartmentCustomerUsersHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
    }
}
