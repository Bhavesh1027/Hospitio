using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;
using HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersConcierge.Queries;

public class GetCustomerConciergeWithRelationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerConciergeWithRelationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<CustomerConciergeWithRelationOut>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerConciergeWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerConciergeWithRelationOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.CustomerId.ToString(),UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer concierge successful.");

        var CustomerGuestsOut = (GetCustomerConciergeWithRelationOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        var isdelete = _fix.CustomerConciergeWithRelationOut[0].IsDeleted;
        _fix.CustomerConciergeWithRelationOut[0].IsDeleted = true;
        var _commonRepository = A.Fake<CustomerConciergeWithRelationOut>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerConciergeWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerConciergeWithRelationOut);

        
        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
        _fix.CustomerConciergeWithRelationOut[0].IsDeleted = isdelete;
    }
}
public class GetCustomerConciergeWithRelationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerConciergeWithRelationOut> CustomerConciergeWithRelationOut { get; set; } = new();
    public GetCustomerConciergeWithRelationIn In { get; set; } = new();
    public int CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id;
        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerGuestAppConciergeCategories = CustomerConciergeCategoryFactory.SeedMany(db, 1);

        In.AppBuilderId = customerGuestApp.Id;

        foreach (var conciergeCategory in customerGuestAppConciergeCategories)
        {
            CustomerConciergeWithRelationOut obj = new()
            {
                Id = conciergeCategory.Id,
                IsDeleted = false
            };
            CustomerConciergeWithRelationOut.Add(obj);
        }
    }

    public GetCustomerConciergeWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response, null);
}