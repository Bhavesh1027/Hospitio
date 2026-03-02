using Azure.Core;
using HospitioApi.Core.HandleTicket.Commands.ForwardTicket;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Commands.ForwardTicketHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Commands;

public class ForwardTicketHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public ForwardTicketHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Ticket forward successful.");
    }

    [Fact]
    public async Task TicketNot_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualUserId = _fix.In.UserId;
        _fix.In.UserId = 0;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket with Id {_fix.In.Id} could not be found.");

        _fix.In.UserId = actualUserId;
        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task UserNot_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class ForwardTicketHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public ForwardTicketIn In { get; set; } = new ForwardTicketIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticket = TicketFactory.SeedSingle(db);
        var user = UserFactory.SeedSingle(db);
        var group = GroupsFactory.SeedSingle(db);

        In.GroupId = group.Id;
        In.UserId = user.Id;
        In.Id = ticket.Id;
    }

    public ForwardTicketHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
