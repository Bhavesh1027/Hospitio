using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleTicket.Commands.CreateTicket;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Commands.CreateTicketHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Commands;

public class CreateTicketHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateTicketHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, UserTypeEnum.Customer,null), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create ticket successful.");
    }
}

public class CreateTicketHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateTicketIn In { get; set; } = new CreateTicketIn();
    public IHubContext<ChatHub> hubContext { get; set; } = A.Fake<IHubContext<ChatHub>>();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var user = UserFactory.SeedSingle(db);
        In.CSAgentId = user.Id;
        In.CustomerId = customer.Id;
        In.Details = "Test Detail";
        In.Duedate = DateTime.UtcNow;
        In.Priority = 1;
        In.Status = 1;
        In.Title = "Test Title";
    }

    public CreateTicketHandler BuildHandler(ApplicationDbContext db) =>
        new(db, hubContext, Response);
}

