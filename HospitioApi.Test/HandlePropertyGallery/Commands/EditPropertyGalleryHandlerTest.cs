using FakeItEasy;
using Humanizer;
using HospitioApi.Core.HandlePropertyGallery.Commands.EditPropertyGallery;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandlePropertyGallery.Commands.EditPropertyGalleryHandlerTestFixture;

namespace HospitioApi.Test.HandlePropertyGallery.Commands;

public class EditPropertyGalleryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public EditPropertyGalleryHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        var _userFilesService = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Gallery edited successfully.");
    }

    [Fact]
    public async Task Request_Not_Null_Error()
    {
        var _userFilesService = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualIn = _fix.In;
        _fix.In = null;

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Request cannot be null.");
        _fix.In = actualIn;
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _userFilesService = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var actualPropertyInformationId = _fix.In.CustomerPropertyInformationId;
        _fix.In.Id = 0;
        _fix.In.CustomerPropertyInformationId = 0;

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Given property Id not exists.");

        _fix.In.Id = actualId;
        _fix.In.CustomerPropertyInformationId = actualPropertyInformationId;
    }


}

public class EditPropertyGalleryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditPropertyGalleryIn In { get; set; } = new EditPropertyGalleryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
        var propertyGallery = CustomerPropertyGalleryFactory.SeedSingle(db, propertyInfo.Id);

        string images = string.Empty;

        In.Id = propertyGallery.Id;
        In.CustomerPropertyInformationId = propertyInfo.Id;
        In.IsActive = true;
        In.PropertyImages = images;

    }

    public EditPropertyGalleryHandler BuildHandler(ApplicationDbContext db, IUserFilesService userFilesService) =>
        new(db, userFilesService, Response);
}


