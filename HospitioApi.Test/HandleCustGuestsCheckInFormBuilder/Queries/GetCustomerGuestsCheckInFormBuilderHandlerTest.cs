using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Queries;

public class GetCustomerGuestsCheckInFormBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerGuestsCheckInFormBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<GetCustomerGuestsCheckInFormBuilderResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerGuestsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guests successful.");

        var CustomerGuestsOut = (GetCustomerGuestsCheckInFormBuilderOut)result.Response;
        Assert.NotNull(CustomerGuestsOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomerGuestsOut[0].Id;
        _fix.CustomerGuestsOut[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomerGuestsOut[0].Id = actualId;
    }
}
public class GetCustomerGuestsCheckInFormBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<GetCustomerGuestsCheckInFormBuilderResponseOut> CustomerGuestsOut { get; set; } = new();
    public GetCustomerGuestsCheckInFormBuilderIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerGuests = CustomerGuestsCheckInFormBuildersFactory.SeedMany(db, 1);

        In.CustomerId = customer.Id;

        foreach (var customerGuest in customerGuests)
        {
            GetCustomerGuestsCheckInFormBuilderResponseOut obj = new()
            {
                Id = customerGuest.Id
            };
            CustomerGuestsOut.Add(obj);
        }
    }

    public GetCustomerGuestsCheckInFormBuilderHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}