using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;
using HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequestById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestRequest.Queries.GetGuestRequestByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestRequest.Queries;

public class GetGuestRequestByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetGuestRequestByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<GuestRequestByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.GuestRequestByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get guest request by id successful.");

        var departmentOut = (GetGuestRequestByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.GuestRequestByIdOut.Id;
        _fix.GuestRequestByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.GuestRequestByIdOut.Id = actualId;
    }
}
public class GetGuestRequestByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetGuestRequestByIdIn In { get; set; } = new();
    public GuestRequestByIdOut GuestRequestByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestJourney = GuestRequestFactory.SeedSingle(db, customer.Id);

        GuestRequestByIdOut = new()
        {
            Id = guestJourney.Id,
            RequestType = guestJourney.RequestType,
            MinuteValue = guestJourney.MinuteValue,
            DayValue = guestJourney.DayValue,
            MonthValue = guestJourney.MonthValue,
            YearValue = guestJourney.YearValue,
            HourValue = guestJourney.HourValue
        };
    }

    public GetGuestRequestByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
