using Bogus;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQaCategories.Queries.GetQaCategoriesHandlerTestFixture;

namespace HospitioApi.Test.HandleQaCategories.Queries
{
    public class GetQaCategoriesHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetQaCategoriesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<QaCategoriesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.QaCategoriesOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get qa categories successful.");

            var QaCategoriesOut = (GetQaCategoriesOut)result.Response;
            Assert.NotNull(QaCategoriesOut);
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

    public class GetQaCategoriesHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<QaCategoriesOut> QaCategoriesOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var qaCategories = QuestionAnswerCategoryFactory.SeedMany(db, 1);


            foreach (var category in qaCategories)
            {
                QaCategoriesOut obj = new()
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                QaCategoriesOut.Add(obj);
            }
        }

        public GetQaCategoriesHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper,Response);
    }
}


