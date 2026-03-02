using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItemHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands;

public class CreateCustomerEnhanceYourStayItemHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerEnhanceYourStayItemHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        var result = await _fix.BuildHandler(db,_commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer enhance your stay item successful.");
    }
}
public class CreateCustomerEnhanceYourStayItemHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerEnhanceYourStayItemIn In { get; set; } = new CreateCustomerEnhanceYourStayItemIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuestAppEnhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);

        var customerGuest = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppEnhanceYourStayCategory.CustomerGuestAppBuilderId, customerGuestAppEnhanceYourStayCategory.CustomerId, customerGuestAppEnhanceYourStayCategory.Id);

        In.CustomerId = customerGuest.CustomerId;
        In.CustomerGuestAppBuilderId = customerGuest.CustomerGuestAppBuilderId;
        In.CustomerGuestAppBuilderCategoryId = customerGuest.CustomerGuestAppBuilderCategoryId;
        In.Badge = 1;
        In.ShortDescription = "Test Short Description";
        In.LongDescription = "Test Long Description";
        In.ButtonType = 1;
        In.ButtonText = "Test";
        In.ChargeType = 1;
        In.Discount = 10;
        In.Price = 2;
        In.Currency = "BTC";
    }

    public CreateCustomerEnhanceYourStayItemHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService _commonRepository) =>
        new(db, Response, _commonRepository);
}