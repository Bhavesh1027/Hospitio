using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStay.Queries;

public class GetCustomerEnhanceYourStayHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerEnhanceYourStayOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerEnhanceYourStayOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In,null), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay categories successful.");

        var CustomerGuestsOut = (GetCustomerEnhanceYourStayOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.customerEnhanceYourStayOuts[0].Id;
        _fix.customerEnhanceYourStayOuts[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In,null), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.customerEnhanceYourStayOuts[0].Id = actualId;
    }
}
public class GetCustomerEnhanceYourStayHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayOut> customerEnhanceYourStayOuts { get; set; } = new();
    public GetCustomerEnhanceYourStayIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerGuests = customerEnhanceYourStayCategoryFactory.SeedMany(db, 1);

        In.BuilderId = customerGuestApp.Id;

        foreach (var customerGuest in customerGuests)
        {
            CustomerEnhanceYourStayOut obj = new()
            {
                Id = customerGuest.Id
            };
            customerEnhanceYourStayOuts.Add(obj);
        }
    }

    public GetCustomerEnhanceYourStayHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}