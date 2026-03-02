using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;
using HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestRequest.Commands.CreateGuestRequestHandlerTestFixture;


namespace HospitioApi.Test.HandleGuestRequest.Commands;

public class CreateGuestRequestHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateGuestRequestHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _chatService = A.Fake<IChatService>();
        var _hubContext = A.Fake<IHubContext<ChatHub>>();
        //var actualName = _fix.In.Name;
        //_fix.In.Name = "Test2";
        var result = await _fix.BuildHandler(db, _chatService, _hubContext).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create guest request successful.");
    }
    [Fact]
    public async Task Customer_Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _chatService = A.Fake<IChatService>();
        var _hubContext = A.Fake<IHubContext<ChatHub>>();
        int actualId = 0;
        foreach (var item in _fix.In.GuestRequests)
        {
            actualId = item.CustomerId;
            item.CustomerId = 0;
        }       
        var result = await _fix.BuildHandler(db, _chatService, _hubContext).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer not found.");
        foreach (var item in _fix.In.GuestRequests)
        {
            item.CustomerId = actualId;
        }
        
    }
    [Fact]
    public async Task Guest_Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _chatService = A.Fake<IChatService>();
        var _hubContext = A.Fake<IHubContext<ChatHub>>();
        int actualId = 0;
        foreach (var item in _fix.In.GuestRequests)
        {
            actualId = item.GuestId;
            item.GuestId = 0;
        }
        var result = await _fix.BuildHandler(db, _chatService, _hubContext).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer guest not found.");
        foreach (var item in _fix.In.GuestRequests)
        {
            item.GuestId = actualId;
        }
    }
}
public class CreateGuestRequestHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateGuestRequestIn In { get; set; } = new();
   

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appbuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db,customer.Id);
        var guest = CustomerGuestFactory.SeedSingle(db,customerReservation.Id);
        var enhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db,appbuilder.Id,customer.Id);

        List<GuestRequestIn> guestRequestIns = new List<GuestRequestIn>();
        GuestRequestIn guestRequestIn = new GuestRequestIn();
        guestRequestIn.CustomerId = customer.Id;
        //guestRequestIn.CustomerGuestAppEnhanceYourStayItemId = enhanceYourStayCategory.Id;
        guestRequestIn.GuestId = guest.Id;
        guestRequestIns.Add(guestRequestIn);
        In.GuestRequests = guestRequestIns;
    }
    public CreateGuestRequestHandler BuildHandler(ApplicationDbContext db,IChatService chatService,IHubContext<ChatHub> hubContext) =>
       new(db, chatService, hubContext,Response);
}
