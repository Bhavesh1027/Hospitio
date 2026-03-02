using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneys;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneysHandlerTestFixture;
using Xunit;
using Azure.Core;

namespace HospitioApi.Test.HandleCustomersGuestJourneys.Commands;

public class UpdateIsActiveCustomersGuestJourneysHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateIsActiveCustomersGuestJourneysHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == $"Guest journey with {_fix.In.Id} has been updated. IsActive has been set to {_fix.In.IsActive}.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers guest journeys with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class UpdateIsActiveCustomersGuestJourneysHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public UpdateIsActiveCustomersGuestJourneyIn In { get; set; } = new();
    public CustomerGuestJourny CustomerGuestJourny { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var guestJourney = CustomerGuestJourneyFactory.SeedSingle(db, customer.Id);
        CustomerGuestJourny = guestJourney;

        In.Id = guestJourney.Id;
        In.IsActive = true;
    }
    public UpdateIsActiveCustomersGuestJourneysHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}
