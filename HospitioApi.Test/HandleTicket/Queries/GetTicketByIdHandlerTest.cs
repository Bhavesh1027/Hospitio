using FakeItEasy;
using HospitioApi.Core.HandleTicket.Queries.GetTicketById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Queries.GetTicketByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Queries;

public class GetTicketByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetTicketByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<GetTicketByIdResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.TicketByIdResponseOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.TicketByIdResponseOut[0].Id), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get ticket successful.");

        var ticketByIdOut = (GetTicketByIdOut)result.Response;
        Assert.NotNull(ticketByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetTicketByIdResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<GetTicketByIdResponseOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.TicketByIdResponseOut[0].Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Tickets not found.");
    }
}

public class GetTicketByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GetTicketByIdResponseOut> TicketByIdResponseOut { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var tickets = TicketFactory.SeedMany(db, 1);

        foreach (var ticket in tickets)
        {
            GetTicketByIdResponseOut obj = new()
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
                CreatedAt = DateTime.UtcNow,
                CSAgentName = ticket.Csagent?.FirstName,
                CustomerName = ticket.Customer?.Cname,
            };
            TicketByIdResponseOut.Add(obj);
        }
    }

    public GetTicketByIdHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
