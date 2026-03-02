using FakeItEasy;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeepingHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerHouseKeeping.Commands;

public class UpdateCustomerHouseKeepingHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerHouseKeepingHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer house keeping successful.");
    }

}
public class UpdateCustomerHouseKeepingHandlerTestFixture : DbFixture
{
    public int CustomerId { get; set; }
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerHouseKeepingIn In { get; set; } = new UpdateCustomerHouseKeepingIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerGuestsCheckInFormBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var housekeepingCategory = housekeepingCategoryFactory.SeedSingle(db, customerGuestsCheckInFormBuilder.Id, customer.Id);
        CustomerId = customer.Id;
        UpdateCustomerHouseKeepingCategoryIn updateCustomerHouseKeepingCategoryIns = new UpdateCustomerHouseKeepingCategoryIn();
        UpdateCustomerHouseKeepingCategoryIn customerHouseKeepingCategoryIn = new UpdateCustomerHouseKeepingCategoryIn();
        customerHouseKeepingCategoryIn.Id = housekeepingCategory.Id;
        customerHouseKeepingCategoryIn.CustomerGuestAppBuilderId = customerGuestsCheckInFormBuilder.Id;
        //updateCustomerHouseKeepingCategoryIns.Add(customerHouseKeepingCategoryIn);
        In.CustomerHouseKeepingCategories = customerHouseKeepingCategoryIn;

    }

    public UpdateCustomerHouseKeepingHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}