using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleGroups.Queries.GetGroupsByDepartmentId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Queries.GetGroupsByDepartmentIdHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Queries
{
    public class GetGroupsByDepartmentIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetGroupsByDepartmentIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<GroupsByDepartmentIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GroupsByDepartmentIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get groups successful.");

            var groupsOut = (GetGroupsByDepartmentIdOut)result.Response;
            Assert.NotNull(groupsOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "No group available.");
        }
    }

    public class GetGroupsByDepartmentIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<GroupsByDepartmentIdOut> GroupsByDepartmentIdOut { get; set; } = new();
        public GetGroupsByDepartmentIdIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = DepartmentFactory.SeedSingle(db);
            var groups = GroupsFactory.SeedMany(db, department.Id, 1);

            In.DepartmentId = 1;
            /*In.PageNo = 1;
            In.PageSize = 10;
            In.SortColumn = "Name";
            In.SortOrder = "ASC";
            In.CategoryId = 0;*/

            foreach (var group in groups)
            {
                GroupsByDepartmentIdOut obj = new()
                {
                    Id = group.Id,
                    Name = group.Name
                };
                GroupsByDepartmentIdOut.Add(obj);
            }
        }

        public GetGroupsByDepartmentIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}



