using Azure.Core;
using HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleQaCategories.Commands.DeleteQaCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleQaCategories.Commands;

public class DeleteQaCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteQaCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete qa category successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.QaCategoryId;
        /*_fix.In.QaCategoryId = 0;*/

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Qa category with Id {actualId} could not be found.");

        _fix.In.QaCategoryId = actualId;
    }
}

public class DeleteQaCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteQaCategoryIn In { get; set; } = new DeleteQaCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var QaCategory = QuestionAnswerCategoryFactory.SeedSingle(db);

        In.QaCategoryId = QaCategory.Id;
    }

    public DeleteQaCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}

