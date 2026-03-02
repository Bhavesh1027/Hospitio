using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Queries;

public class GetCustomerEnhanceYourStayCategoryByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayCategoryByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerEnhanceYourStayCategoryByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.CustomerGuestByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay category successful.");

        var customerGuestByIdOut = (GetCustomerEnhanceYourStayCategoryByIdOut)result.Response;
        Assert.NotNull(customerGuestByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerGuestByIdOut.Id;
        _fix.CustomerGuestByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.CustomerGuestByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customers enhance your stay category could not be found");

        _fix.CustomerGuestByIdOut.Id = actualId;
    }
}
public class GetCustomerEnhanceYourStayCategoryByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerEnhanceYourStayCategoryByIdOut CustomerGuestByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuest = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);

        CustomerGuestByIdOut = new()
        {
            Id = customerGuest.Id,
            CustomerId = customerGuest.CustomerId,
            CustomerGuestAppBuilderId = customerGuest.CustomerGuestAppBuilderId,
            CategoryName = customerGuest.CategoryName
        };
    }

    public GetCustomerEnhanceYourStayCategoryWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}