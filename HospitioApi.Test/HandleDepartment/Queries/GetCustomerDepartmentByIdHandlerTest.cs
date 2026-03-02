using DocumentFormat.OpenXml.Drawing;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleDepartment.Queries.GetCustomerDepartmentByIdHandlerFixure;

namespace HospitioApi.Test.HandleDepartment.Queries
{
    public class GetCustomerDepartmentByIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerDepartmentByIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GetCustomerDepartmentByIdResponseOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.ResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.Id), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get department successful.");

            var departmentOut = (GetCustomerDepartmentByIdOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.ResponseOut;
            _fix.ResponseOut = null;
            A.CallTo(() => _dapper.GetSingle<GetCustomerDepartmentByIdResponseOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.ResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.Id), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

            _fix.ResponseOut = actualObj;
        }
    }
    public class GetCustomerDepartmentByIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerDepartmentByIdResponseOut ResponseOut { get; set; } = new();
        public int Id { get; set; }
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerDepartmentFactory = CustomerDepartmentsFactory.SeedSingle(db,customer.Id);

            ResponseOut.DepartmentMangerId = customerDepartmentFactory.DepartmentMangerId;
            ResponseOut.Name = customerDepartmentFactory.Name;
            ResponseOut.Id = customerDepartmentFactory.Id;

        }
        public GetCustomerDepartmentByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
    }
}
