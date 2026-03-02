using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries;

public class GetCustomerGuestAppBuilderByCustomerRoomIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerGuestAppBuilderByCustomerRoomIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<ModuleServiceOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.ModuleServiceOuts);
        A.CallTo(() => _dapper.GetSingle<CustomerGuestAppBuilderByCustomerRoomIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestAppBuilderByCustomerRoomIdOut);

        var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest app builder successful.");
    }

    [Fact]
    public async Task Room_Notfound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        var actualId = _fix.In.RoomId;
        _fix.In.RoomId = 0;

        var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The customer room not found.");

        _fix.In.RoomId = actualId;
    }
}

public class GetCustomerGuestAppBuilderByCustomerRoomIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerGuestAppBuilderByCustomerRoomIdIn In { get; set; } = new GetCustomerGuestAppBuilderByCustomerRoomIdIn();
    public List<ModuleServiceOut> ModuleServiceOuts { get; set; } = new();
    public CustomerGuestAppBuilderByCustomerRoomIdOut CustomerGuestAppBuilderByCustomerRoomIdOut { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var product = ProductFactory.SeedSingle(db);
        var customer = CustomerFactory.SeedSingle(db,product.Id);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);
        var screenDisplayOrderAndStatus = ScreenDisplayOrderAndStatusesFactory.SeedSingle(db, customerGuestAppBuilder.Id, 2);
        var customerAppBuilders = CustomerGuestAppBuilderFactory.SeedMany(db, 1);


        In.CustomerId = customer.Id;
        In.RoomId = customerRoomName.Id;

        foreach (var obj in customerAppBuilders)
        {
            ModuleServiceOut module = new()
            {
                name = "Test Name",
                items = 1,
                isDisable = false,
                displayOrder = 1,
                customerAppBuliderId = customerGuestAppBuilder.Id,
                image = "test image",
                categories = 1,
            };

            ModuleServiceOuts.Add(module);
        }

        CustomerGuestAppBuilderByCustomerRoomIdOut.Id = customerGuestAppBuilder.Id;
        CustomerGuestAppBuilderByCustomerRoomIdOut.Message = customerGuestAppBuilder.Message;
        CustomerGuestAppBuilderByCustomerRoomIdOut.SecondaryMessage = customerGuestAppBuilder.SecondaryMessage;
        CustomerGuestAppBuilderByCustomerRoomIdOut.LocalExperience = customerGuestAppBuilder.LocalExperience;
        CustomerGuestAppBuilderByCustomerRoomIdOut.Ekey = customerGuestAppBuilder.Ekey;
        CustomerGuestAppBuilderByCustomerRoomIdOut.PropertyInfo = customerGuestAppBuilder.PropertyInfo;
        CustomerGuestAppBuilderByCustomerRoomIdOut.EnhanceYourStay = customerGuestAppBuilder.EnhanceYourStay;
        CustomerGuestAppBuilderByCustomerRoomIdOut.Reception = customerGuestAppBuilder.Reception;
        CustomerGuestAppBuilderByCustomerRoomIdOut.Housekeeping = customerGuestAppBuilder.Housekeeping;
        CustomerGuestAppBuilderByCustomerRoomIdOut.RoomService = customerGuestAppBuilder.RoomService;
        CustomerGuestAppBuilderByCustomerRoomIdOut.Concierge = customerGuestAppBuilder.Concierge;
        CustomerGuestAppBuilderByCustomerRoomIdOut.TransferServices = customerGuestAppBuilder.TransferServices;
        CustomerGuestAppBuilderByCustomerRoomIdOut.IsActive = customerGuestAppBuilder.IsActive;
    }

    public GetCustomerGuestAppBuilderByCustomerRoomIdHandler BuildHandler(IDapperRepository dapper, ApplicationDbContext db) =>
        new(dapper, Response, db);
}
