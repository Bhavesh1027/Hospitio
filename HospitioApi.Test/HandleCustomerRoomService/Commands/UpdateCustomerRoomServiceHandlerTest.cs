using FakeItEasy;
using HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerRoomService.Commands.UpdateCustomerRoomServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerRoomService.Commands;

public class UpdateCustomerRoomServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerRoomServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer room service categories successful.");
    }
}
public class UpdateCustomerRoomServiceHandlerTestFixture : DbFixture
{
    public int CustomerId { get; set; }
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerRoomServiceIn In { get; set; } = new UpdateCustomerRoomServiceIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var receptionCategory = CustomerRoomServiceCategoryFactory.SeedSingle(db, customer.Id, appBuilder.Id);

        CustomerId = customer.Id;
        //List<UpdateCustomerRoomServiceCategoryIn> updateCustomerRoomServiceCategoryIns = new List<UpdateCustomerRoomServiceCategoryIn>();
        UpdateCustomerRoomServiceCategoryIn updateCustomerRoomServiceCategory = new UpdateCustomerRoomServiceCategoryIn();
        updateCustomerRoomServiceCategory.Id = receptionCategory.Id;
        updateCustomerRoomServiceCategory.CustomerGuestAppBuilderId = appBuilder.Id;
        //updateCustomerRoomServiceCategoryIns.Add(updateCustomerRoomServiceCategory);
        In.UpdateCustomerRoomServiceCategoryIn = updateCustomerRoomServiceCategory;

    }

    public UpdateCustomerRoomServiceHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}
