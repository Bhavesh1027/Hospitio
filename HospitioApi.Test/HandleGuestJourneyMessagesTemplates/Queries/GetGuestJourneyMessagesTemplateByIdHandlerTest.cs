using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Queries;

public class GetGuestJourneyMessagesTemplateByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetGuestJourneyMessagesTemplateByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<GuestJourneyMessagesTemplateByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.GuestJourneyMessagesTemplateByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get GuestJourneyMessagesTemplate successful.");

        var departmentOut = (GetGuestJourneyMessagesTemplateByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.GuestJourneyMessagesTemplateByIdOut.Id;
        _fix.GuestJourneyMessagesTemplateByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.GuestJourneyMessagesTemplateByIdOut.Id = actualId;
    }
}

public class GetGuestJourneyMessagesTemplateByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetGuestJourneyMessagesTemplateByIdIn In { get; set; } = new();
    public GuestJourneyMessagesTemplateByIdOut GuestJourneyMessagesTemplateByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var digitalAssistant = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);
        In.Id = digitalAssistant.Id;
        GuestJourneyMessagesTemplateByIdOut = new()
        {
            Id = digitalAssistant.Id,
            Name = digitalAssistant.Name,
            TempleteType = digitalAssistant.TempleteType,
            TempletMessage = digitalAssistant.TempletMessage,
            IsActive = digitalAssistant.IsActive,
        };
    }
    public GetGuestJourneyMessagesTemplateByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}
