using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands;

public class DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _common = A.Fake<ICommonDataBaseOprationService>();

        var result = await _fix.BuildHandler(db, _common).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete enhance your stay category item extra successfully.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _common = A.Fake<ICommonDataBaseOprationService>();

        var actualId = _fix.In.CustomerGuestAppEnhanceYourStayItemId;
        _fix.In.CustomerGuestAppEnhanceYourStayItemId = 0;

        var result = await _fix.BuildHandler(db, _common).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Enhance your stay category item extra with CustomerGuestAppEnhanceYourStayItemId {_fix.In.CustomerGuestAppEnhanceYourStayItemId} not found or user doesn't have the rights to delete it");

        _fix.In.CustomerGuestAppEnhanceYourStayItemId = actualId;
    }
}
public class DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn In { get; set; } = new DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);
        var enhanceStayItem = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id, null);
        var enhanceYourStayCategoryItemsExtra = CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory.SeedSingle(db, enhanceStayItem.Id);

        In.CustomerGuestAppEnhanceYourStayItemId = enhanceStayItem.Id;
    }

    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonDataBaseOprationService) =>
        new(db, Response, commonDataBaseOprationService);
}