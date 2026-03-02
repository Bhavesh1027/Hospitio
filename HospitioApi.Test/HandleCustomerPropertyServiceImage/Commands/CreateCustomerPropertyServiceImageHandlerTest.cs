using FakeItEasy;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImage;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImageHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyServiceImage.Commands;

public class CreateCustomerPropertyServiceImageHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerPropertyServiceImageHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.File), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer property service image created successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<IUserFilesService>();

        var actualCustomerPropertyServiceId = _fix.In.CustomerPropertyServiceId;
        _fix.In.CustomerPropertyServiceId = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.File), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer property service not found.");

        _fix.In.CustomerPropertyServiceId = actualCustomerPropertyServiceId;
    }
}
public class CreateCustomerPropertyServiceImageHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerPropertyServiceImageIn In { get; set; } = new CreateCustomerPropertyServiceImageIn();
    public IFormFile File { get; }
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = CustomerPropertyServiceFactory.SeedSingle(db);
        In.CustomerPropertyServiceId = customerProperty.Id;
    }

    public CreateCustomerPropertyServiceImageHandler BuildHandler(ApplicationDbContext db, IUserFilesService commonRepository) =>
        new(Response, commonRepository, db);
}