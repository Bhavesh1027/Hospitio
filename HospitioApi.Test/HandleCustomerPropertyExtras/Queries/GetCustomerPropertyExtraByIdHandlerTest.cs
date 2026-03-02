using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Queries;

public class GetCustomerPropertyExtraByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerPropertyExtraByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerPropertyExtraByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.customerPropertyExtraByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.customerPropertyExtraByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customers digital assistant successful.");

        var customerPropertyExtraByIdOut = (GetCustomerPropertyExtraByIdOut)result.Response;
        Assert.NotNull(customerPropertyExtraByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.customerPropertyExtraByIdOut.Id;
        _fix.customerPropertyExtraByIdOut.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.customerPropertyExtraByIdOut.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customers digital assistant could not be found");

        _fix.customerPropertyExtraByIdOut.Id = actualId;
    }
}
public class GetCustomerPropertyExtraByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CustomerPropertyExtraByIdOut customerPropertyExtraByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerPropertyExtra = propertyExtraFactory.SeedSingle(db);
        var customerPropertyInformation = customerProperyInformationFactory.SeedSingle(db);

        customerPropertyExtraByIdOut = new()
        {
            Id = customerPropertyExtra.Id,
            CustomerPropertyInformationId = customerPropertyInformation.Id
        };
    }

    public GetCustomerPropertyExtraByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}