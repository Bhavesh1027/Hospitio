using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Commands.CreateQuestionAnswerHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Commands
{
    public class CreateQuestionAnswerHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateQuestionAnswerHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Create_Question_Answer_Category_Not_Exists_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _chatService = A.Fake<IChatService>();
            var _hubContext = A.Fake<IHubContext<ChatHub>>();

            var actualId = _fix.In.QuestionAnswerCategoryId;
            _fix.In.QuestionAnswerCategoryId = 0;

            var result = await _fix.BuildHandler(db,_hubContext,_chatService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Given question answers category not exists.");

            _fix.In.QuestionAnswerCategoryId = actualId;
        }

        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _chatService = A.Fake<IChatService>();
            var _hubContext = A.Fake<IHubContext<ChatHub>>();

            var result = await _fix.BuildHandler(db, _hubContext, _chatService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.Equal("Create question answer successful.", result.Response!.Message);
        }

    }

    public class CreateQuestionAnswerHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateQuestionAnswerIn In { get; set; } = new CreateQuestionAnswerIn();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var QuestionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
            var QuestionAnswer = QuestionAnswerFactory.SeedSingle(db, QuestionCategory.Id);

            In.Id = QuestionAnswer.Id;
            In.QuestionAnswerCategoryId = QuestionCategory.Id;
            In.Name = "Test";
            In.Description = "Test Description";
            In.Icon = "Test Icon";
            In.IsActive = true;
            In.IsPublish = true;
        }

        public CreateQuestionAnswerHandler BuildHandler(ApplicationDbContext db,IHubContext<ChatHub> hubContext, IChatService _chatService) =>
            new(db, hubContext, _chatService, Response);
    }
}



