using FakeItEasy;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItems;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnahnceYourStayItemsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Queries;

public class GetCustomerEnhanceYourStayItemsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayItemsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerEnhanceYourStayItemsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerEnhanceYourStayItemsOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay items successful.");

        var customerEnhanceYourStayItemsOut = (GetCustomerEnhanceYourStayItemsOut)result.Response;
        Assert.NotNull(customerEnhanceYourStayItemsOut);
    }
}
public class GetCustomerEnahnceYourStayItemsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayItemsOut> customerEnhanceYourStayItemsOuts { get; set; } = new();
    public GetCustomerEnhanceYourStayItemsIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerReservation = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);
        var customerGuestAppEnhanceYourStayItems = CustomerGuestAppEnhanceYourStayItemFactory.SeedMany(db, 1);

        In.CustomerId = customer.Id;
        In.PageNo = 1;
        In.PageSize = 10;

        foreach (var customerGuestAppEnhanceYourStayItem in customerGuestAppEnhanceYourStayItems)
        {
            CustomerEnhanceYourStayItemsOut obj = new()
            {
                Id = customerGuestAppEnhanceYourStayItem.Id,
                CustomerId = customerGuestAppEnhanceYourStayItem.CustomerId,
                CustomerGuestAppBuilderId = customerGuestAppEnhanceYourStayItem.CustomerGuestAppBuilderId,
                CustomerGuestAppBuilderCategoryId = customerGuestAppEnhanceYourStayItem.CustomerGuestAppBuilderCategoryId,
                Badge = customerGuestAppEnhanceYourStayItem.Badge,
                ShortDescription = customerGuestAppEnhanceYourStayItem.ShortDescription,
                LongDescription = customerGuestAppEnhanceYourStayItem.LongDescription,
                ButtonType = customerGuestAppEnhanceYourStayItem.ButtonType,
                ButtonText = customerGuestAppEnhanceYourStayItem.ButtonText,
                ChargeType = customerGuestAppEnhanceYourStayItem.ChargeType,
                Discount = customerGuestAppEnhanceYourStayItem.Discount,
                Price = customerGuestAppEnhanceYourStayItem.Price,
                Currency = customerGuestAppEnhanceYourStayItem.Currency
            };
            customerEnhanceYourStayItemsOuts.Add(obj);
        }
    }

    public GetCustomerEnhanceYourStayItemsHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}