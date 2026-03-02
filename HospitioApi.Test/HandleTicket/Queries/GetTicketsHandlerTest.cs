using FakeItEasy;
using HospitioApi.Core.HandleTicket.Queries.GetTicketById;
using HospitioApi.Core.HandleTicket.Queries.GetTickets;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Queries.GetTicketsHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Queries;

public class GetTicketsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetTicketsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetTicketsResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GetTicketsResponseOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.UserId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get tickets successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetTicketByIdResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<GetTicketByIdResponseOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.UserId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Tickets not found.");
    }
}

public class GetTicketsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GetTicketsResponseOut> GetTicketsResponseOut { get; set; } = new();
    public string? UserId { get; set; } 
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var tickets = TicketFactory.SeedMany(db, 1);

        foreach (var ticket in tickets)
        {
            GetTicketsResponseOut obj = new()
            {
                Id = ticket.Id,
                CustomerId = ticket.CustomerId,
                Title = ticket.Title,
                Details = ticket.Details,
                Priority = ticket.Priority,
                Duedate = ticket.Duedate,
                CSAgentId = ticket.CSAgentId,
                Status = ticket.Status,
                CloseDate = ticket.CloseDate,
                CreatedFrom = ticket.CreatedFrom,
                CSAgentName = ticket.Csagent?.FirstName,
                CustomerName = ticket.Customer?.Cname,
            };
            GetTicketsResponseOut.Add(obj);
        }
    }

        public GetTicketsHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper,Response);
}
