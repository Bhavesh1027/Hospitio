using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQaCategories.Queries.GetQaCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleQaCategories.Queries
{
    public class GetQaCategoryHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetQaCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<QaCategoryOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.QaCategoryOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.QaCategoryOut.Id }), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get qa category successful.");

            var qaCategoryOut = (GetQaCategoryOut)result.Response;
            Assert.NotNull(qaCategoryOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.QaCategoryOut.Id;
            _fix.QaCategoryOut.Id = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.QaCategoryOut.Id }), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");

            _fix.QaCategoryOut.Id = actualId;
        }
    }

    public class GetQaCategoryHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public QaCategoryOut QaCategoryOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var qaCategory = QuestionAnswerCategoryFactory.SeedSingle(db);

            QaCategoryOut = new()
            {
                Id = qaCategory.Id,
                Name = qaCategory.Name,
            };
        }

        public GetQaCategoryHandler BuildHandler(IDapperRepository _dapper) =>
            new(Response,_dapper);
    }
}
