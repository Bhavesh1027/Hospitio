using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleProduct.Queries.GetProducts;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProduct.Queries.GetProductsHandlerTestFixture;

namespace HospitioApi.Test.HandleProduct.Queries;

public class GetProductsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetProductsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<GetProductsResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.getProductsResponses);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get products successful.");

        var productOut = (GetProductsOut)result.Response;
        Assert.NotNull(productOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");
    }
}

public class GetProductsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetProductsIn In { get; set; } = new();
    public List<GetProductsResponseOut> getProductsResponses { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var products = ProductFactory.SeedMany(db, 1);

        In.SearchValue = "test";

        foreach (var product in products)
        {
            GetProductsResponseOut obj = new()
            {
                Id = product.Id,
                Name = product.Name,
                IsActive = product.IsActive
            };
            getProductsResponses.Add(obj);
        }
    }

    public GetProductsHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
