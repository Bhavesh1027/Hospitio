using Azure.Core;
using HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleQaCategories.Commands.UpdateQaCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleQaCategories.Commands;

public class UpdateQaCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public UpdateQaCategoryHandlerTest(ThisTestFixture fixture, ITestOutputHelper output) {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update qa Category successful.");
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new UpdateQaCategoryRequest(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The QA category {_fix.In.Name} already exists.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualName = _fix.In.Name;
        _fix.In.Id = 0;
        _fix.In.Name = "Test";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"QA category with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
        _fix.In.Name = actualName;
    }

    
}

public class UpdateQaCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateQaCategoryIn In { get; set; } = new UpdateQaCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var QaCategory = QuestionAnswerCategoryFactory.SeedSingle(db);

        In.Id = QaCategory.Id;
        In.Name = QaCategory.Name;
        
    }

    public UpdateQaCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
