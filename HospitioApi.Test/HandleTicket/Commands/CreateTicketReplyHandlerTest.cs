using HospitioApi.Core.HandleTicket.Commands.CreateTicketReply;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Commands.CreateTicketReplyHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Commands;

public class CreateTicketReplyHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateTicketReplyHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, UserTypeEnum.Customer,null), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create ticket reply successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.TicketId;
        _fix.In.TicketId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, UserTypeEnum.Customer,null), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Ticket with Id {_fix.In.TicketId} could not be found.");

        _fix.In.TicketId = actualId;
    }
}
public class CreateTicketReplyHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateTicketReplyIn In { get; set; } = new CreateTicketReplyIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var ticket = TicketFactory.SeedSingle(db);

        In.Reply = "Test Reply";
        In.TicketId = ticket.Id;
    }

    public CreateTicketReplyHandler BuildHandler(ApplicationDbContext db) =>
        new(db,null,null, Response);
}
