using HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategories;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicketCategories.Queries.GetTicketCategoriesHandlerTestFixture;

namespace HospitioApi.Test.HandleTicketCategories.Queries;

public class GetTicketCategoriesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetTicketCategoriesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get ticket categories successful.");
    }
}

public class GetTicketCategoriesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetTicketCategoriesIn In { get; set; } = new GetTicketCategoriesIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        TicketCategoryFactory.SeedMany(db, 1);
    }

    public GetTicketCategoriesHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
