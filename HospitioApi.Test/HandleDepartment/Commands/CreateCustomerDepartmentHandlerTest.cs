using FakeItEasy;
using HospitioApi.Core.HandleDepartment.Commands.CreateCustomerDepartment;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleDepartment.Commands.CreateCustomerDepartmentHandlerFixure;

namespace HospitioApi.Test.HandleDepartment.Commands
{
    public class CreateCustomerDepartmentHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public CreateCustomerDepartmentHandlerTest(ThisTestFixture fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            string actualName = _fix.In.Name;
            _fix.In.Name = "UniqueDepartmentName";

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create department successful.");
            _fix.In.Name = actualName;
        }
        [Fact]
        public async Task Already_Exists_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "The department name already exists in the system.");
        }
    }
    public class CreateCustomerDepartmentHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateCustomerDepartmentIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customers = CustomerFactory.SeedSingle(db);
            var customerUsers = CustomerUserFactory.SeedSingle(db, customers.Id);
            var customerDepartment = CustomerDepartmentsFactory.SeedSingle(db, customers.Id);

            In.Name = customerDepartment.Name;
            In.CustomerId = customers.Id;
        }

        public CreateCustomerDepartmentHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}
