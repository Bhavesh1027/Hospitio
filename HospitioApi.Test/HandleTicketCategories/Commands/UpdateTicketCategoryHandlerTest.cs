using Azure.Core;
using HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicketCategories.Commands.UpdateTicketCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleTicketCategories.Commands;

public class UpdateTicketCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateTicketCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update ticket category successful.");        
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The ticket category {_fix.In.CategoryName} already exists.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualName = _fix.In.CategoryName;
        _fix.In.Id = 0;
        _fix.In.CategoryName = "Demo";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket category with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
        _fix.In.CategoryName = actualName;
    }
}

public class UpdateTicketCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateTicketCategoryIn In { get; set; } = new UpdateTicketCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticketCategory = TicketCategoryFactory.SeedSingle(db);

        In.CategoryName = ticketCategory.CategoryName;
        In.IsActive = true;
        In.Id = ticketCategory.Id;
    }

    public UpdateTicketCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(Response, db);
}
