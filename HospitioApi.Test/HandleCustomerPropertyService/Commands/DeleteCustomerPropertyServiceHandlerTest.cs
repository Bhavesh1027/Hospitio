using HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyService.Commands;

public class DeleteCustomerPropertyServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerPropertyServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete property service successful");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer property service id not found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerPropertyServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerPropertyServiceIn In { get; set; } = new DeleteCustomerPropertyServiceIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = CustomerPropertyServiceFactory.SeedSingle(db);

        In.Id = customerProperty.Id;
    }

    public DeleteCustomerPropertyServiceHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}