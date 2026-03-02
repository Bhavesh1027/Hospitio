using HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatus;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatusHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Commands;

public class UpdateQuestionAnswerStatusHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateQuestionAnswerStatusHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update request status successfully.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "QA request not found.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Unable_To_Publish_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var questionanswer = new QuestionAnswer
        {
            Name = "Test",
            Description = "Test",
            Icon = "Test",
            IsActive = false,
            IsPublish = true,
        };
        db.QuestionAnswers.Add(questionanswer);
        db.SaveChanges();
        _fix.In.Id = questionanswer.Id;
        

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Request is inactive unable to publish this request.");

    }
}

public class UpdateQuestionAnswerStatusHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateQuestionAnswerStatusIn In { get; set; } = new UpdateQuestionAnswerStatusIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var questionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
        var questionAnswer = QuestionAnswerFactory.SeedSingle(db, questionCategory.Id,true);

        In.Id = questionAnswer.Id;
        In.IsPublish = true;

    }

    public UpdateQuestionAnswerStatusHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}


