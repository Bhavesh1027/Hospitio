using FakeItEasy;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategories;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoriesHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries;

public class GetCustomerEnhanceYourStayCategoriesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayCategoriesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerEnhanceYourStayCategoriesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerEnhanceYourStayCategories);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay categories successful.");

        var CustomerGuestsOut = (GetCustomerEnhanceYourStayCategoriesOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
}
public class GetCustomerEnhanceYourStayCategoriesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayCategoriesOut> customerEnhanceYourStayCategories { get; set; } = new();
    public GetCustomerEnhanceYourStayCategoriesIn In { get; set; } = new();
    public int CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuestAppEnhanceYourStayCategories = customerEnhanceYourStayCategoryFactory.SeedMany(db, 1);

        In.CustomerId = customer.Id;
        In.PageNo = 1;
        In.PageSize = 10;

        foreach (var customerGuest in customerGuestAppEnhanceYourStayCategories)
        {
            CustomerEnhanceYourStayCategoriesOut obj = new()
            {
                Id = customerGuest.Id,
                CustomerId = customerGuest.CustomerId,
                CustomerGuestAppBuilderId = customerGuest.CustomerGuestAppBuilderId,
                CategoryName = customerGuest.CategoryName
            };
            customerEnhanceYourStayCategories.Add(obj);
        }
    }

    public GetCustomerEnhanceYourStayCategoriesHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}