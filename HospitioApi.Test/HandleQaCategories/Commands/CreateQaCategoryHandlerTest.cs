using Azure.Core;
using HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQaCategories.Commands.CreateQaCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleQaCategories.Commands
{
    public class CreateQaCategoryHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateQaCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Already_Exists_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            var name = _fix.In.Name;

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The qa category {name} already exists.");
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create question category successful.");
        }
        
    }

    public class CreateQaCategoryHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateQaCategoryIn In { get; set; } = new CreateQaCategoryIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            In.Name = "Test";
            In.IsActive = true;
        }

        public CreateQaCategoryHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}


