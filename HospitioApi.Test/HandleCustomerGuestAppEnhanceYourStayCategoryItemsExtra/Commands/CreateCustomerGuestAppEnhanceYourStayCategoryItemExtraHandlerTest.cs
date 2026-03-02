using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands;

public class CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer enhance your stay category item extra successful.");
    }

}
public class CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn In { get; set; } = new CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn();
    public ICommonDataBaseOprationService _common { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);
        var enhanceStayItem = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id, null);

        In.CustomerGuestAppEnhanceYourStayItemId = enhanceStayItem.Id;
        In.createCustomerGuestAppEnhanceYourStayCategoryItems = new List<CreateCustomerGuestAppEnhanceYourStayCategoryItem>();
    }

    public CustomerGuestAppEnhanceYourStayCategoryItemExtraHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response, _common);
}