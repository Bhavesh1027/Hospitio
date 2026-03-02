using Azure.Core;
using FakeItEasy;
using HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;
using HospitioApi.Core.HandleQuestionAnswer.Commands.DeleteQuestionAnswer;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQuestionAnswer.Commands.DeleteQuestionAnswerHandlerTestFixture;

namespace HospitioApi.Test.HandleQuestionAnswer.Commands;

public class DeleteQuestionAnswerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteQuestionAnswerHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _userFilesService = A.Fake<IUserFilesService>();

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete article successfully.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _userFilesService = A.Fake<IUserFilesService>();

        var actualId = _fix.In.QuestionAnswerId;
        _fix.In.QuestionAnswerId = 0;

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Question answer article with id {_fix.In.QuestionAnswerId} not found or user doesn't have the rights to delete it");

        _fix.In.QuestionAnswerId = actualId;
    }
}

public class DeleteQuestionAnswerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteQuestionAnswerIn In { get; set; } = new DeleteQuestionAnswerIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var QuestionCategory = QuestionAnswerCategoryFactory.SeedSingle(db);
        var QuestionAnswer = QuestionAnswerFactory.SeedSingle(db, QuestionCategory.Id);

        In.QuestionAnswerId = QuestionAnswer.Id;
    }

    public DeleteQuestionAnswerHandler BuildHandler(ApplicationDbContext db, IUserFilesService _userFilesService) =>
        new(db, Response, _userFilesService);
}


