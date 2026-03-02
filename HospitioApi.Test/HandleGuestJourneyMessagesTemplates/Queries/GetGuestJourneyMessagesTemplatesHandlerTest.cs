using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplatesHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries;

public class GetGuestJourneyMessagesTemplatesHandlerTest : IClassFixture<ThisTestFixture>
{ 
    private readonly ThisTestFixture _fix;
    public GetGuestJourneyMessagesTemplatesHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<GuestJourneyMessagesTemplatesOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GuestJourneyMessagesTemplatesOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get guest journey messages templates successful.");

        var departmentOut = (GetGuestJourneyMessagesTemplatesOut)result.Response;
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

public class GetGuestJourneyMessagesTemplatesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GuestJourneyMessagesTemplatesOut> GuestJourneyMessagesTemplatesOut { get; set; } = new();

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        //In.CustomerId = customer.Id;
        var digitalAssistants = GuestJourneyMessagesTemplatesFactory.SeedMany(db, 1);

        foreach (var item in digitalAssistants)
        {
            GuestJourneyMessagesTemplatesOut obj = new()
            {
                Id = item.Id,
                Name = item.Name,
                TempleteType = item.TempleteType,
                TempletMessage = item.TempletMessage,
                IsActive = item.IsActive,
            };
            GuestJourneyMessagesTemplatesOut.Add(obj);
        }
    }
    public GetGuestJourneyMessagesTemplatesHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}
