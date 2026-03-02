using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplates;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplatesHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestJourneyMessagesTemplates.Commands;

public class DeleteGuestJourneyMessagesTemplatesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteGuestJourneyMessagesTemplatesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete GuestJourneyMessagesTemplate successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"GuestJourneyMessagesTemplate with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteGuestJourneyMessagesTemplatesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteGuestJourneyMessagesTemplatesIn In { get; set; } = new();
    public IVonageService _vonageService { get; set; } = A.Fake<IVonageService>();
    public IOptions<VonageSettingsOptions> FakeVonageSettingsOptions { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var guestJourneyMessages = GuestJourneyMessagesTemplatesFactory.SeedSingle(db);

        In.Id = guestJourneyMessages.Id;
    }

    public DeleteGuestJourneyMessagesTemplatesHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response, _vonageService, FakeVonageSettingsOptions);
}
