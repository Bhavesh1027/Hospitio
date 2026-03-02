using FakeItEasy;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStayHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStay.Commands;

public class CreateCustomerEnhanceYourStayHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerEnhanceYourStayHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer enhance your stay category successful.");
    }
}
public class CreateCustomerEnhanceYourStayHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public CreateCustomerEnhanceYourStayIn In { get; set; } = new CreateCustomerEnhanceYourStayIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerGuest = CustomerGuestAppBuilderFactory.SeedSingle(db);

        In.CustomerId = customer.Id;
        In.CustomerGuestAppBuilderId = customerGuest.Id;
    }

    public CreateCustomerEnhanceYourStayHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}