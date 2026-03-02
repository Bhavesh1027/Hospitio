using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtras;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtrasHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Queries;

public class GetCustomerPropertyExtrasHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerPropertyExtrasHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerPropertyExtraOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.customerPropertyExtraOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customers property extra successful.");

        var customerPropertyExtrasOut = (GetCustomerPropertyExtrasOut)result.Response;
        Assert.NotNull(customerPropertyExtrasOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.customerPropertyExtraOuts[0].Id;
        _fix.customerPropertyExtraOuts[0].Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers property extra could not be found");

        _fix.customerPropertyExtraOuts[0].Id = actualId;
    }
}
public class GetCustomerPropertyExtrasHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomerPropertyExtraOut> customerPropertyExtraOuts { get; set; } = new();
    public GetCustomerPropertyExtrasIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        var propertyExtras = propertyExtraFactory.SeedMany(db, 1);

        In.CustomerPropertyInformationId = customerProperty.Id;

        foreach (var propertyExtra in propertyExtras)
        {
            CustomerPropertyExtraOut obj = new()
            {
                Id = propertyExtra.Id
            };
            customerPropertyExtraOuts.Add(obj);
        }
    }

    public GetCustomerPropertyExtrasHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}