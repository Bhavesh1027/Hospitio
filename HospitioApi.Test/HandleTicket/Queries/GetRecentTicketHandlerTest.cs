using FakeItEasy;
using HospitioApi.Core.HandleTicket.Queries.GetRecentTickets;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Queries.GetRecentTicketHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Queries;

public class GetRecentTicketHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetRecentTicketHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAll<GetRecentTicketsResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.RecentTicketsResponseOuts);

        var result = await _fix.BuildHandler(db,_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get recent tickets successful.");

        var recentTicketOut = (GetRecentTicketOut)result.Response;
        Assert.NotNull(recentTicketOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAll<GetRecentTicketsResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<GetRecentTicketsResponseOut>());

        var result = await _fix.BuildHandler(db,_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetRecentTicketHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetRecentTicketIn In { get; set; } = new GetRecentTicketIn();
    public List<GetRecentTicketsResponseOut> RecentTicketsResponseOuts { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var tickets = TicketFactory.SeedMany(db, 1);

        foreach(var ticket in tickets)
        {
            GetRecentTicketsResponseOut obj = new()
            {
                Id = ticket.Id,
                CustomerId = ticket.CustomerId,
                Title = ticket.Title,
                Details = ticket.Details,
                Priority = ticket.Priority,
                Duedate = ticket.Duedate,
                TicketCategoryId = ticket.TicketCategoryId,
                CSAgentId = ticket.CSAgentId,
                Status = ticket.Status,
                CloseDate = ticket.CloseDate,
                CreatedFrom = ticket.CreatedFrom,
                CreatedAt = ticket.CreatedAt
            };
            RecentTicketsResponseOuts.Add(obj);
        }
    }

    public GetRecentTicketHandler BuildHandler(ApplicationDbContext db, IDapperRepository dapper) =>
        new(db, Response, dapper);
}
