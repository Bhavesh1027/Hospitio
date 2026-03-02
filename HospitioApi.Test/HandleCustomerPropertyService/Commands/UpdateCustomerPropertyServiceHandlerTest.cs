using HospitioApi.Core.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyService;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyService.Commands;

public class UpdateCustomerPropertyServiceHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerPropertyServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer property service updated successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer property service image not found.");

        _fix.In.Id = actualId;
    }
    [Fact]
    public async Task Image_Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualPropertyServiceImageId = _fix.In.UPCustomerPropertyServiceImageIns[0].Id;
        _fix.In.UPCustomerPropertyServiceImageIns[0].Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer property service image not found.");

        _fix.In.Id = actualId;
        _fix.In.UPCustomerPropertyServiceImageIns[0].Id = actualPropertyServiceImageId;
    }
}
public class UpdateCustomerPropertyServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerPropertyServiceIn In { get; set; } = new UpdateCustomerPropertyServiceIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        var propertyService = CustomerPropertyServiceFactory.SeedSingle(db);
        var customerPropertyServiceImage = CustomerPropertyServiceImageFactory.SeedSingle(db, propertyService.Id);

        List<UPCustomerPropertyServiceImageIn> customerPropertyServiceImageIns = new List<UPCustomerPropertyServiceImageIn>();
        UPCustomerPropertyServiceImageIn customerPropertyServiceImageIn = new UPCustomerPropertyServiceImageIn();
        customerPropertyServiceImageIn.Id = customerPropertyServiceImage.Id;
        customerPropertyServiceImageIns.Add(customerPropertyServiceImageIn);

        In.Id = propertyService.Id;
        In.CustomerPropertyInformationId = customerProperty.Id;
        In.UPCustomerPropertyServiceImageIns = customerPropertyServiceImageIns;
    }

    public UpdateCustomerPropertyServiceHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}