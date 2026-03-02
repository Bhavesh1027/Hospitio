using HospitioApi.Core.HandleProduct.Commands.EditProduct;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleProduct.Commands.EditProductHandlerTestFixture;

namespace HospitioApi.Test.HandleProduct.Commands;

public class EditProductHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public EditProductHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.Name;
        _fix.In.Name = "New Product";

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Edit product successful.");

        _fix.ProductFactory.Update(db, new() { Name = actualName});
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Product not found.");

        _fix.Id = actualId;
    }

    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In, _fix.Id), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "The product name already exists in the system.");
    }
}

public class EditProductHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditProductIn In { get; set; } = new();
    public int Id { get; set; }
    public Product Product { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var productObj = ProductFactory.SeedSingle(db);
        Product = productObj;
        In.Name = "Test";
        Id = productObj.Id;
    }

    public EditProductHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
