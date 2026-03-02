using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersGuestJourneys.Queries;

public class GetCustomersGuestJourneysByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersGuestJourneysByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomersGuestJourneysByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomersGuestJourneysByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customers guest journey successful.");

        var departmentOut = (GetCustomersGuestJourneysByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.CustomersGuestJourneysByIdOut.Id;
        _fix.CustomersGuestJourneysByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customers guest journey could not be found");

        _fix.CustomersGuestJourneysByIdOut.Id = actualId;
    }
}
public class GetCustomersGuestJourneysByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomersGuestJourneysByIdIn In { get; set; } = new();
    public CustomersGuestJourneysByIdOut CustomersGuestJourneysByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestJourney = CustomerGuestJourneyFactory.SeedSingle(db, customer.Id);

        CustomersGuestJourneysByIdOut = new()
        {
            Id = guestJourney.Id,
            CutomerId = customer.Id,
            JourneyStep = guestJourney.JourneyStep,
            Name = guestJourney.Name,
            SendType = guestJourney.SendType,
            TimingOption1 = guestJourney.TimingOption1,
            TimingOption2 = guestJourney.TimingOption2,
            TimingOption3 = guestJourney.TimingOption2,
            Timing = guestJourney.Timing,
            NotificationDays = guestJourney.NotificationDays,
            NotificationTime    = guestJourney.NotificationTime,
            GuestJourneyMessagesTemplateId = guestJourney.GuestJourneyMessagesTemplateId,
            TempletMessage = guestJourney.TempletMessage,
        };
    }

    public GetCustomersGuestJourneysByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
