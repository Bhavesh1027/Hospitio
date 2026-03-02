using Azure.Core;
using HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicketCategories.Commands.DeleteTicketCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleTicketCategories.Commands;

public class DeleteTicketCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteTicketCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete ticket category successful.");
    }

    [Fact]
    public async Task Already_Exists_Error()
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

public class DeleteTicketCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteTicketCategoryIn In { get; set; } = new DeleteTicketCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticketCategory = TicketCategoryFactory.SeedSingle(db);

        In.Id = ticketCategory.Id;
    }

    public DeleteTicketCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
