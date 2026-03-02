using Bogus;
using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Queries.GetQuestionAnswerByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Queries
{
    public class GetQuestionAnswerByIdHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetQuestionAnswerByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAllJsonData<QuestionAnswerByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.QuestionAnswerByIdOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.QuestionAnswerByIdOut[0].Id }), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get question answer info successful.");

            var questionAnswerByIdOut = (GetQuestionAnswerByIdOut)result.Response;
            Assert.NotNull(questionAnswerByIdOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var actualId = _fix.QuestionAnswerByIdOut[0].Id;
            _fix.QuestionAnswerByIdOut[0].Id = 0;

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = 0 }), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Question answer could not be found");

            _fix.QuestionAnswerByIdOut[0].Id = actualId;
        }
    }

    public class GetQuestionAnswerByIdHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<QuestionAnswerByIdOut> QuestionAnswerByIdOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var questionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
            var questionAnswer = QuestionAnswerFactory.SeedSingle(db, questionCategory.Id);

            QuestionAnswerByIdOut QuestionAnswer = new()
            {
                Id = questionAnswer.Id,
                QuestionAnswerCategoryId = questionCategory.Id,
                Name = questionAnswer.Name,
                Description = questionAnswer.Description,
                Icon = questionAnswer.Icon,
                IsActive = questionAnswer.IsActive,
                IsPublish = questionAnswer.IsPublish
            };

            QuestionAnswerByIdOut.Add(QuestionAnswer);
        }

        public GetQuestionAnswerByIdHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper,Response);
    }
}

