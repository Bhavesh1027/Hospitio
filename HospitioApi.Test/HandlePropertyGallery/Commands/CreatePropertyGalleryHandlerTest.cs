using HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandlePropertyGallery.Commands.CreatePropertyGalleryHandlerTestFixture;

namespace HospitioApi.Test.HandlePropertyGallery.Commands;

public class CreatePropertyGalleryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreatePropertyGalleryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Gallery created successfully.");
    }

    [Fact]
    public async Task Request_Not_Null_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualIn = _fix.In;
        _fix.In = null;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Request cannot be null.");
        _fix.In = actualIn;
    }
}
public class CreatePropertyGalleryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreatePropertyGalleryIn In { get; set; } = new CreatePropertyGalleryIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);

        string images = "";

        //In.CustomerPropertyInformationId = propertyInfo.Id;
        //In.IsActive = true;
        //In.PropertyImage = images;

    }

    public EditPropertyGalleryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}

