using FakeItEasy;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyService.Commands.CreateCustomerPropertyServiceHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyService.Commands;

public class CreateCustomerPropertyServiceHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerPropertyServiceHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer property service successful.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        var actualCustomerPropertyInfoId = _fix.In.CustomerPropertyInformationId;
        _fix.In.CustomerPropertyInformationId = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The customer property information not found.");

        _fix.In.CustomerPropertyInformationId = actualCustomerPropertyInfoId;
    }
}
public class CreateCustomerPropertyServiceHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerPropertyServiceIn In { get; set; } = new CreateCustomerPropertyServiceIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        In.CustomerPropertyInformationId = customerProperty.Id;
    }

    public CreateCustomerPropertyServiceHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}