using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleGroups.Queries.GetGroup;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGroups.Queries.GetGroupHandlerTestFixture;

namespace HospitioApi.Test.HandleGroups.Queries
{
    public class GetGroupHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetGroupHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GroupOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.GroupOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.GroupOut.Id }), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get group successful.");

            var groupOut = (GetGroupOut)result.Response;
            Assert.NotNull(groupOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.GroupOut.Id;
            _fix.GroupOut.Id = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.GroupOut.Id }), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

            _fix.GroupOut.Id = actualId;
        }
    }

    public class GetGroupHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GroupOut GroupOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var group = GroupsFactory.SeedSingle(db);

            GroupOut = new()
            {
                Id = group.Id,
                Name = group.Name,
                DepartmentId = group.DepartmentId,
                IsActive = group.IsActive
            };
        }

        public GetGroupHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper,Response);
    }
}

