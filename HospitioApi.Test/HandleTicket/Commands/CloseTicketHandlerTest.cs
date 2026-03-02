using HospitioApi.Core.HandleTicket.Commands.CloseTicket;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Commands.CloseTicketHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Commands;

public class CloseTicketHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CloseTicketHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Ticket close successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.TicketId;
        _fix.In.TicketId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket with Id {_fix.In.TicketId} could not be found.");

        _fix.In.TicketId = actualId;
    }
}

public class CloseTicketHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CloseTicketIn In { get; set; } = new CloseTicketIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticket = TicketFactory.SeedSingle(db);
        In.TicketId = ticket.Id;
    }

    public CloseTicketHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
