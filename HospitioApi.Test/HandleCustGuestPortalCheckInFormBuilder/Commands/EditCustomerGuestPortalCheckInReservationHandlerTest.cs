using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservation;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands;

public class EditCustomerGuestPortalCheckInReservationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public EditCustomerGuestPortalCheckInReservationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer reservation successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customer reservation could not be found.");

        _fix.In.Id = actualId;
    }
}

public class EditCustomerGuestPortalCheckInReservationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditCustomerGuestPortalCheckInReservationIn In { get; set; } = new EditCustomerGuestPortalCheckInReservationIn();
    public IChatService chatService;
    public IHubContext<ChatHub> hubContext;
    public IVonageService vonage;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);

        In.Id = customerReservation.Id;
    }

    public EditCustomerGuestPortalCheckInReservationHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response , chatService , hubContext ,vonage);
}
