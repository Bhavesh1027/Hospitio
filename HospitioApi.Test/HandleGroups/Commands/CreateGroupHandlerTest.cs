using HospitioApi.Core.HandleGroups.Commands.CreateGroup;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Commands.CreateGroupHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Commands
{
    public class CreateGroupHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateGroupHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Already_Exists_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
           
            var result2 = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None); 

            Assert.True(result2.HasFailure);
            Assert.True(result2.Failure!.Message == $"The group {_fix.In.Name} already exists.");
          
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var actualName = _fix.In.Name;
            _fix.In.Name = "UniqueTestName";
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create group successful.");
            _fix.In.Name = actualName;
        }
    }

    public class CreateGroupHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateGroupIn In { get; set; } = new CreateGroupIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = DepartmentFactory.SeedSingle(db);
            var groups = GroupsFactory.SeedSingle(db, department.Id);
            In.UserType = 1;
            In.Name = groups.Name;
            In.DepartmentId = department.Id;
            In.IsActive = true;
        }

        public CreateGroupHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}


