using FakeItEasy;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImage;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImageHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyServiceImage.Commands;

public class DeleteCustomerPropertyServiceImageHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerPropertyServiceImageHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer property image successfully.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _commonRepository = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer property service image is not found with id {_fix.In.Id}");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerPropertyServiceImageHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerPropertyServiceImageIn In { get; set; } = new DeleteCustomerPropertyServiceImageIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = CustomerPropertyServiceFactory.SeedSingle(db);
        var propertyServiceImage = CustomerPropertyServiceImageFactory.SeedSingle(db, customerProperty.Id);

        In.Id = propertyServiceImage.Id;
    }

    public DeleteCustomerPropertyServiceImageHandler BuildHandler(ApplicationDbContext db, IUserFilesService commonRepository) =>
        new(db, Response, commonRepository);
}