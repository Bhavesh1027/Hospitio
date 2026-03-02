using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplatesForCustomerHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries;

public class GetGuestJourneyMessagesTemplatesForCustomerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetGuestJourneyMessagesTemplatesForCustomerHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<GuestJourneyMessagesTemplatesForCustomerOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GuestJourneyMessagesTemplatesForCustomerOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get guest journey messages templates successful.");

        var departmentOut = (GetGuestJourneyMessagesTemplatesForCustomerOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}
public class GetGuestJourneyMessagesTemplatesForCustomerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GuestJourneyMessagesTemplatesForCustomerOut> GuestJourneyMessagesTemplatesForCustomerOut { get; set; } = new();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        //In.CustomerId = customer.Id;
        var digitalAssistants = GuestJourneyMessagesTemplatesFactory.SeedMany(db, 1);
        CustomerId = customer.Id.ToString();
        foreach (var item in digitalAssistants)
        {
            GuestJourneyMessagesTemplatesForCustomerOut obj = new()
            {
                Id = item.Id,
                Name = item.Name,
                TempleteType = item.TempleteType,
                TempletMessage = item.TempletMessage,
                IsActive = item.IsActive,
            };
            GuestJourneyMessagesTemplatesForCustomerOut.Add(obj);
        }
    }
    public GetGuestJourneyMessagesTemplatesForCustomerHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}