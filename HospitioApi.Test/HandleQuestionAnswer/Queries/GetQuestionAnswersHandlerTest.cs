using Bogus;
using FakeItEasy;
using Humanizer;
using Moq;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Queries.GetQuestionAnswersHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Queries
{
    public class GetQuestionAnswersHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetQuestionAnswersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<QuestionAnswersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.QuestionAnswersOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get question answer successful.");

            var QaCategoriesOut = (GetQuestionAnswersOut)result.Response;
            Assert.NotNull(QaCategoriesOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }

    public class GetQuestionAnswersHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<QuestionAnswersOut> QuestionAnswersOut { get; set; } = new();
        public GetQuestionAnswersIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var questionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
            var questionAnswers = QuestionAnswerFactory.SeedMany(db, questionCategory.Id, 1);

            In.SearchValue = "test";
            /*In.PageNo = 1;
            In.PageSize = 10;
            In.SortColumn = "Name";
            In.SortOrder = "ASC";
            In.CategoryId = 0;*/

            foreach (var questionAnswer in questionAnswers)
            {
                QuestionAnswersOut obj = new()
                {
                    Id = questionAnswer.Id,
                    QuestionAnswerCategoryId = questionAnswer.QuestionAnswerCategoryId,
                    Name = questionAnswer.Name,
                    Description = questionAnswer.Description,
                    Icon = questionAnswer.Icon,
                    IsActive = questionAnswer.IsActive,
                    IsPublish = questionAnswer.IsPublish
                };
                QuestionAnswersOut.Add(obj);
            }
        }

        public GetQuestionAnswersHandler BuildHandler(IDapperRepository _dapper) =>
            new( _dapper, Response);
    }
}


