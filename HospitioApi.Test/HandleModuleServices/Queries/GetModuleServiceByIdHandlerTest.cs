using FakeItEasy;
using HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleModuleServices.Queries.GetModuleServiceByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleModuleServices.Queries
{
    public class GetModuleServiceByIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetModuleServiceByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<ModuleServiceById>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.ModuleServiceById);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get module service successful.");

            var moduleServiceOut = (GetModuleServiceByIdOut)result.Response;
            Assert.NotNull(moduleServiceOut);
        }

        [Fact]
        public async Task NotFound_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            ModuleServiceById? obj = null;
            A.CallTo(() => _dapper.GetSingle<ModuleServiceById>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }

    public class GetModuleServiceByIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetModuleServiceByIdIn In { get; set; } = new GetModuleServiceByIdIn();
        public ModuleServiceById ModuleServiceById { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var module = ModuleFactory.SeedSingle(db);
            var moduleService = ModuleServiceFactory.SeedSingle(db, module);

            ModuleServiceById = new()
            {
                Id = moduleService.Id,
                Name = moduleService.Name,
                ModuleId = module.Id
            };
        }

        public GetModuleServiceByIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}

