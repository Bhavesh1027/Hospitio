using HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicketCategories.Commands.CreateTicketCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleTicketCategories.Commands;

public class CreateTicketCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateTicketCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.Name;
        _fix.In.Name = "demo";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create ticket category successful.");

        _fix.In.Name = actualName;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The ticket category {_fix.In.Name} already exists.");
    }
}

public class CreateTicketCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateTicketCategoryIn In { get; set; } = new CreateTicketCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticketCategory = TicketCategoryFactory.SeedSingle(db);

        In.Name = ticketCategory.CategoryName;
        In.IsActive = true;
    }

    public CreateTicketCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
