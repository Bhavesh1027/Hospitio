using FakeItEasy;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerGuestAppEnhanceYourStayItemHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands;

public class UpdateCustomerEnhanceYourStayItemHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerEnhanceYourStayItemHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customers enhance your stay category item successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        var actualId = _fix.In.Id;
        var actualCustomerGuestAppBuilderId = _fix.In.CustomerGuestAppBuilderId;
        var actualCustomerId = _fix.In.CustomerId;
        var actualCustomerGuestAppBuilderCategoryId = _fix.In.CustomerGuestAppBuilderCategoryId;
        _fix.In.CustomerGuestAppBuilderId = 0;
        _fix.In.Id = 0;
        _fix.In.CustomerGuestAppBuilderCategoryId = 0;
        _fix.In.CustomerId = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay category Item with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
        _fix.In.CustomerGuestAppBuilderId = actualCustomerGuestAppBuilderId;
        _fix.In.CustomerId = actualCustomerId;
        _fix.In.CustomerGuestAppBuilderCategoryId = actualCustomerGuestAppBuilderCategoryId;
    }
}
public class UpdateCustomerGuestAppEnhanceYourStayItemHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerEnhanceYourStayItemIn In { get; set; } = new UpdateCustomerEnhanceYourStayItemIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = CustomerFactory.SeedSingle(db);
        var customerGuestAppEnhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);
        var customerGuestAppEnhanceYourStayItem = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppEnhanceYourStayCategory.CustomerGuestAppBuilderId, customerGuestAppEnhanceYourStayCategory.CustomerId, customerGuestAppEnhanceYourStayCategory.Id);

        In.Id = customerGuestAppEnhanceYourStayItem.Id;
        In.CustomerId = customerGuestAppEnhanceYourStayItem.CustomerId;
        In.CustomerGuestAppBuilderId = customerGuestAppEnhanceYourStayItem.CustomerGuestAppBuilderId;
        In.CustomerGuestAppBuilderCategoryId = customerGuestAppEnhanceYourStayItem.CustomerGuestAppBuilderCategoryId;
        In.Badge = 1;
        In.ShortDescription = "Test";
        In.LongDescription = "Long Test";
        In.ButtonType = 1;
        In.ButtonText = "Test";
        In.ChargeType = 1;
        In.Discount = 10;
        In.Price = 1;
        In.Currency = "ETH";
    }

    public UpdateCustomerEnhanceYourStayItemHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService _commonRepository) =>
        new(db, Response, _commonRepository);
}