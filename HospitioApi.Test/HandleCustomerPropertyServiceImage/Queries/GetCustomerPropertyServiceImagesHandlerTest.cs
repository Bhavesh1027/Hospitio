using FakeItEasy;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImages;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImagesHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyServiceImage.Queries;

public class GetCustomerPropertyServiceImagesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerPropertyServiceImagesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Not_Found_Error()
    {
        var _commonRepository = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.CustomerPropertyServiceId;
        _fix.In.CustomerPropertyServiceId = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"No customer property service images found.");

        _fix.In.CustomerPropertyServiceId = actualId;
    }
}
public class GetCustomerPropertyServiceImagesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerPropertyServiceImagesIn In { get; set; } = new GetCustomerPropertyServiceImagesIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = CustomerPropertyServiceFactory.SeedSingle(db);
        var propertyServiceImage = CustomerPropertyServiceImageFactory.SeedSingle(db, customerProperty.Id);

        In.CustomerPropertyServiceId = customerProperty.Id;
    }

    public GetCustomerPropertyServiceImagesHandler BuildHandler(ApplicationDbContext db, IUserFilesService commonRepository) =>
        new(Response, commonRepository, db);
}