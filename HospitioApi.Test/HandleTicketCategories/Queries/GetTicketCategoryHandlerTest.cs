using Azure.Core;
using HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicketCategories.Queries.GetTicketCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleTicketCategories.Queries;

public class GetTicketCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetTicketCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get ticket category successful.");
    }

    [Fact]
    public async Task Not_found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket category with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class GetTicketCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetTicketCategoryIn In { get; set; } = new GetTicketCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticketCategory = TicketCategoryFactory.SeedMany(db, 1);
        In.Id = ticketCategory[0].Id;
    }

    public GetTicketCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
