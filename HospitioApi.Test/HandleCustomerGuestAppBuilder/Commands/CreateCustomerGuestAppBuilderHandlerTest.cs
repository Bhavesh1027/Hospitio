using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands.CreateCustomerGuestAppBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands;

public class CreateCustomerGuestAppBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerGuestAppBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer guest app builder successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actalRoomId = _fix.In.CustomerRoomNameId;
        _fix.In.CustomerRoomNameId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer room not found.");

        _fix.In.CustomerRoomNameId = actalRoomId;
    }
    [Fact]
    public async Task GuestAppBuilder_Alreadt_exist_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actalRoomId = _fix.In.CustomerRoomNameId;
        _fix.In.CustomerRoomNameId = _fix.CustomerRoomId;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer guest app builder alreday exists.");

        _fix.In.CustomerRoomNameId = actalRoomId;
    }
}

public class CreateCustomerGuestAppBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestAppBuilderIn In { get; set; } = new CreateCustomerGuestAppBuilderIn();
    public int CustomerRoomId { get; set; }
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);

        //Multiple Guest AppBuilder for same Customer
        var customerRoomNames = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder1 = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomNames.Id);
        var customerGuestAppBuilder2 = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomNames.Id);
        CustomerRoomId = customerRoomNames.Id;

        In.CustomerId = customer.Id;
        In.CustomerRoomNameId = customerRoomName.Id;
        In.Concierge =true;
        In.Ekey = true;
        In.EnhanceYourStay = true;
        In.Housekeeping = true;
        In.LocalExperience = true;
        In.Message = "Test Message";
        In.PropertyInfo = true;
        In.Reception = true;
        In.RoomService = true;
        In.SecondaryMessage = "Test Secondary Message";
        In.TransferServices = true;
        In.IsActive = true;
    }

    public CreateCustomerGuestAppBuilderHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
