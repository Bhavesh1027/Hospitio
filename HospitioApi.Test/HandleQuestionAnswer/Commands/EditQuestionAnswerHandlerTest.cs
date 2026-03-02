using FakeItEasy;
using HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Commands.EditQuestionAnswerHandlerTestFixture;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.SignalR.Hubs;

namespace HospitioApi.Test.HandleQuestionAnswer.Commands;

public class EditQuestionAnswerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;
    private readonly IHubContext<ChatHub> _hubContext;

    public EditQuestionAnswerHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _userFilesService = A.Fake<IUserFilesService>();

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Article edited successfully.");
    }

    [Fact]
    public async Task Not_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _userFilesService = A.Fake<IUserFilesService>();

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new EditQuestionAnswerRequest(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Given article not exists.");

        _fix.In.Id = actualId;
    }
}

public class EditQuestionAnswerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditQuestionAnswerIn In { get; set; } = new EditQuestionAnswerIn();
    public IHubContext<ChatHub> hubContexts { get; set; } = A.Fake<IHubContext<ChatHub>>();
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

    public EditQuestionAnswerHandler BuildHandler(ApplicationDbContext db, IUserFilesService _userFilesService) =>
        new(db, Response, _userFilesService, hubContexts);
}

