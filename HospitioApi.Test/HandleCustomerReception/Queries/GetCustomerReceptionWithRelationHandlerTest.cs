using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
using HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReception.Queries.GetCustomerReceptionWithRelationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReception.Queries;

public class GetCustomerReceptionWithRelationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerReceptionWithRelationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerReceptionWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerReceptionWithRelationOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer reception successful.");

        var CustomerGuestsOut = (GetCustomerReceptionWithRelationOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerReceptionWithRelationOut[0].Id;
        _fix.CustomerReceptionWithRelationOut[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.CustomerReceptionWithRelationOut[0].Id = actualId;
    }
}
public class GetCustomerReceptionWithRelationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerReceptionWithRelationOut> CustomerReceptionWithRelationOut { get; set; } = new();
    public GetCustomerReceptionWithRelationIn In { get; set; } = new();
    public int CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id;
        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerGuestAppReceptionCategories = CustomerReceptionCategoryFactory.SeedMany(db, 1);

        In.AppBuilderId = customerGuestApp.Id;

        foreach (var receptionCategory in customerGuestAppReceptionCategories)
        {
            CustomerReceptionWithRelationOut obj = new()
            {
                Id = receptionCategory.Id
            };
            CustomerReceptionWithRelationOut.Add(obj);
        }
    }

    public GetCustomerReceptionWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
