using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItemById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerGuestAppEnhannceYourStayItemByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Queries;

public class GetCustomerEnhanceYourStayItemByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayItemByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerEnhanceYourStayItemByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerEnhanceYourStayItemByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerEnhanceYourStayItemByIdOut[0].Id },null), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay item successful.");

        var customerEnhanceYourStayItemByIdOut = (GetCustomerEnhanceYourStayItemByIdOut)result.Response;
        Assert.NotNull(customerEnhanceYourStayItemByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerEnhanceYourStayItemByIdOut[0].Id;
        _fix.CustomerEnhanceYourStayItemByIdOut[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = 0 },null), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay item could not be found");

        _fix.CustomerEnhanceYourStayItemByIdOut[0].Id = actualId;
    }
}
public class GetCustomerGuestAppEnhannceYourStayItemByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayItemByIdOut> CustomerEnhanceYourStayItemByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuestAppEnhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);
        var customerGuestAppEnhanceYourStayItem = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppEnhanceYourStayCategory.CustomerGuestAppBuilderId, customerGuestAppEnhanceYourStayCategory.CustomerId, customerGuestAppEnhanceYourStayCategory.Id);

        CustomerEnhanceYourStayItemByIdOut CustomerEnhanceYourStayItem = new()
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

        CustomerEnhanceYourStayItemByIdOut.Add(CustomerEnhanceYourStayItem);
    }

    public GetCustomerEnhanceYourStayItemByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}