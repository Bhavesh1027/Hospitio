using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayByCategory;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayByCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStay.Queries;

public class GetCustomerEnhanceYourStayByCategoryHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerEnhanceYourStayByCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerEnhanceYourStayByCategoryOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerEnhanceYourStayByCategoryOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer enhance your stay categories successful.");

        var CustomerGuestsOut = (GetCustomerEnhanceYourStayByCategoryOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.customerEnhanceYourStayByCategoryOuts[0].Id;
        _fix.customerEnhanceYourStayByCategoryOuts[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.customerEnhanceYourStayByCategoryOuts[0].Id = actualId;
    }
}
public class GetCustomerEnhanceYourStayByCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerEnhanceYourStayByCategoryOut> customerEnhanceYourStayByCategoryOuts { get; set; } = new();
    public GetCustomerEnhanceYourStayByCategoryIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuests = customerEnhanceYourStayCategoryFactory.SeedMany(db, 1);

        In.CategoryId = customerGuests[0].Id;

        foreach (var customerGuest in customerGuests)
        {
            CustomerEnhanceYourStayByCategoryOut obj = new()
            {
                Id = customerGuest.Id
            };
            customerEnhanceYourStayByCategoryOuts.Add(obj);
        }
    }

    public GetCustomerEnhanceYourStayByCategoryHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}