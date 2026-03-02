using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServices;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyService.Queries.GetCustomerPropertyServicesHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyService.Queries;

public class GetCustomerPropertyServicesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerPropertyServicesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerPropertyServicesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerPropertyServicesOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer property services successful.");

        var customerPropertyServicesOut = (GetCustomerPropertyServicesOut)result.Response;
        Assert.NotNull(customerPropertyServicesOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.customerPropertyServicesOuts[0].Id;
        _fix.customerPropertyServicesOuts[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.customerPropertyServicesOuts[0].Id = actualId;
    }
}
public class GetCustomerPropertyServicesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerPropertyServicesOut> customerPropertyServicesOuts { get; set; } = new();
    public GetCustomerPropertyServicesIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        var propertyServices = CustomerPropertyServiceFactory.SeedMany(db, 1);

        In.CustomerPropertyInformationId = customerProperty.Id;

        foreach (var propertyService in propertyServices)
        {
            CustomerPropertyServicesOut obj = new()
            {
                Id = propertyService.Id
            };
            customerPropertyServicesOuts.Add(obj);
        }
    }

    public GetCustomerPropertyServicesHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}