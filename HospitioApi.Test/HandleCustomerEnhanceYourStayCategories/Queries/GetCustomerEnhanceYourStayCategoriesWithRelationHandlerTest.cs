using FakeItEasy;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoriesWithRelation;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries;

public class GetCustomerEnhanceYourStayCategoriesWithRelationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayCategoriesWithRelationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerEnhanceYourStayCategoriesWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerGuestsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay categories successful.");

        var CustomerGuestsOut = (GetCustomerEnhanceYourStayCategoriesWithRelationOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
}
public class GetCustomerEnhanceYourStayCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayCategoriesWithRelationOut> CustomerGuestsOut { get; set; } = new();
    public GetCustomerEnhanceYourStayCategoriesWithRelationIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuests = customerEnhanceYourStayCategoryFactory.SeedMany(db, 1);

        In.PageNo = 1; In.PageSize = 1;
        In.CustomerId = customer.Id;

        foreach (var customerGuest in customerGuests)
        {
            CustomerEnhanceYourStayCategoriesWithRelationOut obj = new()
            {
                Id = customerGuest.Id,
                CustomerId = customerGuest.CustomerId,
                CustomerGuestAppBuilderId = customerGuest.CustomerGuestAppBuilderId,
                CategoryName = customerGuest.CategoryName
            };
            CustomerGuestsOut.Add(obj);
        }
    }

    public GetCustomerEnhanceYourStayCategoriesWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}