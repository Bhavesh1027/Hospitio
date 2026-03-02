using Azure.Core;
using HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
using HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswer;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Commands.UpdateQuestionAnswerHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Commands;

public class UpdateQuestionAnswerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateQuestionAnswerHandlerTest(ThisTestFixture fixture) => _fix = fixture;

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


}

public class UpdateQuestionAnswerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateQuestionAnswerIn In { get; set; } = new UpdateQuestionAnswerIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var questionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
        var questionAnswer = QuestionAnswerFactory.SeedSingle(db, questionCategory.Id);

        In.Id = questionAnswer.Id;
        In.IsActive = true;

    }

    public UpdateQuestionAnswerHandler BuildHandler(ApplicationDbContext db) =>
        new(db,null, Response);
}

