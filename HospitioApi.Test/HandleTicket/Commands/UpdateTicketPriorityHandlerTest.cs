using HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Commands.UpdateTicketPriorityHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Commands;

public class UpdateTicketPriorityHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateTicketPriorityHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update ticket status successful.");
    }

    [Fact]
    public async Task Not_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Given ticket not exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateTicketPriorityHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateTicketPriorityIn In { get; set; } = new UpdateTicketPriorityIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticket = TicketFactory.SeedSingle(db);
        In.Id = ticket.Id;
    }

    public UpdateTicketPriorityHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
