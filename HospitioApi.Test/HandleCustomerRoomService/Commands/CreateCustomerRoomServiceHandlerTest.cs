using FakeItEasy;
using HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerRoomService.Commands.CreateCustomerRoomServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerRoomService.Commands;

public class CreateCustomerRoomServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerRoomServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer room service successful.");
    }
    [Fact]
    public async Task Unable_To_Add()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualData = _fix.In.CustomerRoomServiceCategories;
        _fix.In.CustomerRoomServiceCategories = null;

        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Unable to add room service request successfully.");

        _fix.In.CustomerRoomServiceCategories = actualData;
    }
}
public class CreateCustomerRoomServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerRoomServiceIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appbuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);

        List<CreateCustomerRoomServiceCategoryIn> createCustomerRoomServiceCategoryIns = new List<CreateCustomerRoomServiceCategoryIn>();
        CreateCustomerRoomServiceCategoryIn customerRoomServiceCategoryIn = new CreateCustomerRoomServiceCategoryIn();
        customerRoomServiceCategoryIn.CustomerId = customer.Id;
        customerRoomServiceCategoryIn.CustomerGuestAppBuilderId = appbuilder.Id;
        createCustomerRoomServiceCategoryIns.Add(customerRoomServiceCategoryIn);
        In.CustomerRoomServiceCategories = createCustomerRoomServiceCategoryIns;

    }
    public CreateCustomerRoomServiceHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
       new(db, Response, commonRepository);
}
