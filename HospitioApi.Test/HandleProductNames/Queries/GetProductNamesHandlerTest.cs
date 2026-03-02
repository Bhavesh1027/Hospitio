using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleProductNames.Queries.GetProductNames;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProductNames.Queries.GetProductNamesHandlerTestFixture;

namespace HospitioApi.Test.HandleProductNames.Queries;

public class GetProductNamesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductNamesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<ProductNamesOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.ProductNames);

        var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get product names successful.");

        var ProductNamesOut = (GetProductNamesOut)result.Response;
        Assert.NotNull(ProductNamesOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetProductNamesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<ProductNamesOut> ProductNames { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var products = ProductFactory.SeedMany(db, 1);

        foreach (var product in products)
        {
            ProductNamesOut productNamesOut = new()
            {
                Id = product.Id,
                Name = product.Name
            };
            ProductNames.Add(productNamesOut);
        }
    }

    public GetProductNamesHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
