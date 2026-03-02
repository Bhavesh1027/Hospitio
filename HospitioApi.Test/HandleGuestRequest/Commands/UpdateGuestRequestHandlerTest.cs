using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequest;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestRequest.Commands.UpdateGuestRequestHandlerTestFixture;
using Azure.Core;

namespace HospitioApi.Test.HandleGuestRequest.Commands;

public class UpdateGuestRequestHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateGuestRequestHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update guest request successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.In.Id;       
        _fix.In.Id = 0;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Guest request with Id {_fix.In.Id} could not be found.");
        _fix.In.Id = actualId;
    }
}
public class UpdateGuestRequestHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public UpdateGuestRequestIn In { get; set; } = new();
    public GuestRequest GuestRequest { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var guestJourny = GuestRequestFactory.SeedSingle(db, customer.Id);
        GuestRequest = guestJourny;

        In.Id = GuestRequest.Id;
        In.Status = Shared.Enums.GuestRequestStatusEnum.Completed;
        In.IsActive = true;
     }
    public UpdateGuestRequestHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}
