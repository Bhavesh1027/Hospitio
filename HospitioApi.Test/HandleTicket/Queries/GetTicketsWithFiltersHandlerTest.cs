using FakeItEasy;
using HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;
using HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFiltersWithFilters;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleTicket.Queries.GetTicketsWithFiltersHandlerTestFixture;

namespace HospitioApi.Test.HandleTicket.Queries;

public class GetTicketsWithFiltersHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetTicketsWithFiltersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetTicketsWithFiltersResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Tickets);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.user.Id.ToString(),Shared.Enums.UserTypeEnum.Hospitio), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get tickets successful.");

        var ticketsOut = (GetTicketsWithFiltersOut)result.Response;
        Assert.NotNull(ticketsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        A.CallTo(() => _dapper.GetAll<GetTicketsWithFiltersResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<GetTicketsWithFiltersResponseOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In,_fix.user.Id.ToString(),Shared.Enums.UserTypeEnum.Hospitio), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Tickets not found.");
    }
}

public class GetTicketsWithFiltersHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetTicketsWithFiltersIn In { get; set; } = new GetTicketsWithFiltersIn();
    public List<GetTicketsWithFiltersResponseOut> Tickets { get; set; } = new();
    public User user { get; set; }
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var tickets = TicketFactory.SeedMany(db, 1);
        user = UserFactory.SeedSingle(db);

        In.Status = 1;
        In.Priority = 1;
        In.ApplyPagination = true;

        foreach (var ticket in tickets)
        {
            GetTicketsWithFiltersResponseOut obj = new()
            {
                Id = ticket.Id,
                CustomerId = ticket.CustomerId,
                Title = ticket.Title,
                Details = ticket.Details,
                Priority = ticket.Priority,
                Duedate = ticket.Duedate,
                Status = ticket.Status,
                CloseDate = ticket.CloseDate,
                CreatedFrom = ticket.CreatedFrom,
                CreatedAt = DateTime.UtcNow,
                CSAgentName = ticket.Csagent?.FirstName,
                CustomerName = ticket.Customer?.Cname,
                BusinessName = ticket.Customer?.BusinessName,
                Email = ticket.Customer?.Email,
                IsActive = ticket.IsActive
            };
            Tickets.Add(obj);
        }
    }

    public GetTicketsWithFiltersHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
