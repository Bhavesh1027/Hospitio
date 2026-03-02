using FakeItEasy;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneys;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneysHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersGuestJourneys.Commands;

public class DeleteCustomersGuestJourneysHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomersGuestJourneysHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers guest journey successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers guest journey with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomersGuestJourneysHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomersGuestJourneysIn In { get; set; } = new();
    public IVonageService _vonageService { get; set; } = A.Fake<IVonageService>();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestJourny = CustomerGuestJourneyFactory.SeedSingle(db, customer.Id);

        In.Id = guestJourny.Id;
    }

    public DeleteCustomersGuestJourneysHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response, _vonageService);
}
