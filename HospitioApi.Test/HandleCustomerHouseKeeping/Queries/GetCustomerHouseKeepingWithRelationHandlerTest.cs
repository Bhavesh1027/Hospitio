using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerHouseKeeping.Queries;

public class GetCustomerHouseKeepingWithRelationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerHouseKeepingWithRelationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerHouseKeepingWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerHouseKeepingWithRelationOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer house keeping successful.");

        var CustomerGuestsOut = (GetCustomerHouseKeepingWithRelationOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerHouseKeepingWithRelationOuts[0].Id;
        _fix.CustomerHouseKeepingWithRelationOuts[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In, _fix.CustomerId.ToString(),UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerHouseKeepingWithRelationOuts[0].Id = actualId;
    }
}
public class GetCustomerHouseKeepingWithRelationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerHouseKeepingWithRelationOut> CustomerHouseKeepingWithRelationOuts { get; set; } = new();
    public GetCustomerHouseKeepingWithRelationIn In { get; set; } = new();
    public int CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id;
        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var housekeepingCategories = housekeepingCategoryFactory.SeedMany(db, 1);

        In.AppBuilderId = customerGuestApp.Id;

        foreach (var housekeepingCategory in housekeepingCategories)
        {
            CustomerHouseKeepingWithRelationOut obj = new()
            {
                Id = housekeepingCategory.Id
            };
            CustomerHouseKeepingWithRelationOuts.Add(obj);
        }
    }

    public GetCustomerHouseKeepingWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}