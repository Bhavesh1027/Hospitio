using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleModuleServices.Queries.GetModuleServices;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleModuleServices.Queries.GetModuleServicesHandlerTestFixture;

namespace HospitioApi.Test.HandleModuleServices.Queries
{
    public class GetModuleServicesHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetModuleServicesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<ModuleServicesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.ModuleServicesOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get module services successful.");

            var moduleServicesOut = (GetModuleServicesOut)result.Response;
            Assert.NotNull(moduleServicesOut);
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

    public class GetModuleServicesHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<ModuleServicesOut> ModuleServicesOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var module = ModuleFactory.SeedSingle(db);
            var moduleServices = ModuleServiceFactory.SeedMany(db, module.Id,1);

            foreach (var service in moduleServices)
            {
                ModuleServicesOut obj = new()
                {
                    Id = service.Id,
                    Name = service.Name,
                    ModuleId = module.Id
                };
                ModuleServicesOut.Add(obj);
            }
        }

        public GetModuleServicesHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}




