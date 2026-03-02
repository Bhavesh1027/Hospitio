using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Data;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelationHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerRoomService.Queries;

public class GetCustomerRoomServiceWithRelationHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerRoomServiceWithRelationHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerRoomServiceWithRelationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerRoomServiceWithRelationOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer room service successful.");

        var CustomerGuestsOut = (GetCustomerRoomServiceWithRelationOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerRoomServiceWithRelationOut[0].Id;
        _fix.CustomerRoomServiceWithRelationOut[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In, _fix.CustomerId.ToString(), UserTypeEnum.Customer.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");

        _fix.CustomerRoomServiceWithRelationOut[0].Id = actualId;
    }
}
public class GetCustomerRoomServiceWithRelationHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerRoomServiceWithRelationOut> CustomerRoomServiceWithRelationOut { get; set; } = new();
    public GetCustomerRoomServiceWithRelationIn In { get; set; } = new();
    public int CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id;
        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerGuestAppRoomServiceCategories = CustomerRoomServiceCategoryFactory.SeedMany(db, 1);

        In.AppBuilderId = customerGuestApp.Id;

        foreach (var roomServiceCategory in customerGuestAppRoomServiceCategories)
        {
            CustomerRoomServiceWithRelationOut obj = new()
            {
                Id = roomServiceCategory.Id
            };
            CustomerRoomServiceWithRelationOut.Add(obj);
        }
    }

    public GetCustomerRoomServiceWithRelationHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
