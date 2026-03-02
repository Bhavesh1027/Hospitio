using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneys;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomersGuestJourneys.Queries;

public class GetCustomersGuestJourneysHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersGuestJourneysHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<CustomersGuestJourneysOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomersGuestJourneysOuts);

        var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get guest journey successful.");

        var departmentOut = (GetCustomersGuestJourneysOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomersGuestJourneysHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomersGuestJourneysIn In  { get; set; } = new();
    public string CustomerId { get; set; } = string.Empty;
    public List<CustomersGuestJourneysOut> CustomersGuestJourneysOuts { get; set; } = new();

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var messageTemplete = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);
        //In.CustomerId = customer.Id;
        var digitalAssistants = CustomerGuestJourneyFactory.SeedMany(db,messageTemplete.Id,customer.Id, 1);
        CustomerId = customer.Id.ToString();
        foreach (var item in digitalAssistants)
        {
            CustomersGuestJourneysOut obj = new()
            {
               Id = item.Id,
               Name = item.Name,
               JourneyStep = item.JourneyStep,
               SendType = item.SendType,
               TimingOption1 = item.TimingOption1,
               TimingOption2 = item.TimingOption2,
               TempletMessage = item.TempletMessage,
               TimingOption3 = item.TimingOption3,
               Timing = item.Timing,
               NotificationTime = item.NotificationTime,
               NotificationDays = item.NotificationDays,
               GuestJourneyMessagesTemplateId = messageTemplete.Id,
               IsActive = item.IsActive
            };
            CustomersGuestJourneysOuts.Add(obj);
        }
    }
    public GetCustomersGuestJourneysHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}
