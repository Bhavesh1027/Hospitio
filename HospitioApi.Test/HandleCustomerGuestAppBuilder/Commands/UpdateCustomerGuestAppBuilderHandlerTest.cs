using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerAppBuilder;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerGuestAppBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands;

public class UpdateCustomerGuestAppBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerGuestAppBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer guest app builder successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.CustomerRoomNameId;
        _fix.In.CustomerRoomNameId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer room not found.");

        _fix.In.CustomerRoomNameId = actualId;
    }

    [Fact]
    public async Task AlreadyExists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer guest app builder already exists.");

        _fix.In.Id = actualId;
    }

    [Fact]
    public async Task Id_NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 1;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer guest app builder already exists.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerGuestAppBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerGuestAppBuilderIn In { get; set; } = new UpdateCustomerGuestAppBuilderIn();
    private IHubContext<ChatHub> _hubContext { get; set; } = A.Fake<IHubContext<ChatHub>>();
    private IChatService _chatService { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);
        var screenDisplayOrderAndStatus = ScreenDisplayOrderAndStatusesFactory.SeedSingle(db, customerGuestAppBuilder.Id, 2);

        CustomerId = customer.Id.ToString();
        In.Id = customerGuestAppBuilder.Id;
        In.CustomerRoomNameId = customerRoomName.Id;
        In.Concierge = true;
        In.Ekey = true;
        In.EnhanceYourStay = true;
        In.Housekeeping = true;
        In.LocalExperience = true;
        In.Message = "Test Message";
        In.PropertyInfo = true;
        In.Reception = true;
        In.RoomService = true;
        In.SecondaryMessage = "Test SecondaryMessage";
        In.TransferServices = true;
        In.IsActive = true;
        In.JsonData = "Test json";
    }

    public UpdateCustomerGuestAppBuilderHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response, _hubContext, _chatService);
}
