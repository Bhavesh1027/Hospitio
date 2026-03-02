using Bogus;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleModules.Queries.GetModules;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleModules.Queries.GetModulesHandlerTestFixture;

namespace HospitioApi.Test.HandleModules.Queries
{
    public class GetModulesHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetModulesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<ModulesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.ModulesOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get modules successful.");

            var moduleOut = (GetModulesOut)result.Response;
            Assert.NotNull(moduleOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetModulesHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<ModulesOut> ModulesOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var modules = ModuleFactory.SeedMany(db, 1);


            foreach (var module in modules)
            {
                ModulesOut obj = new()
                {
                    Id = module.Id,
                    Name = module.Name,
                    ModuleType = module.ModuleType
                };
                ModulesOut.Add(obj);
            }
        }

        public GetModulesHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}



