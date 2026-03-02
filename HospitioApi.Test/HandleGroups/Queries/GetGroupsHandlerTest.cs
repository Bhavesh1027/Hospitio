using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleGroups.Commands.CreateGroup;
using HospitioApi.Core.HandleGroups.Queries.GetGroups;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Queries.GetGroupsHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Queries
{
    public class GetGroupsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetGroupsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<GroupsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GroupsOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get groups successful.");

            var groupsOut = (GetGroupsOut)result.Response;
            Assert.NotNull(groupsOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetGroupsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<GroupsOut> GroupsOut { get; set; } = new();
        public GetGroupsIn In { get; set; } = new GetGroupsIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = DepartmentFactory.SeedSingle(db);
            var groups = GroupsFactory.SeedMany(db, department.Id, 1);


            foreach (var group in groups)
            {
                GroupsOut obj = new()
                {
                    Id = group.Id,
                    Name = group.Name,
                    DepartmentId = group.DepartmentId,
                    IsActive = group.IsActive
                };
                GroupsOut.Add(obj);
            }
        }

        public GetGroupsHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}



