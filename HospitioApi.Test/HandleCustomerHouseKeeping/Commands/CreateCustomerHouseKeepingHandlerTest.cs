using FakeItEasy;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeepingHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerHouseKeeping.Commands;

public class CreateCustomerHouseKeepingHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerHouseKeepingHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer house keeping successful.");
    }
}
public class CreateCustomerHouseKeepingHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerHouseKeepingIn In { get; set; } = new CreateCustomerHouseKeepingIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);
        var customerGuest = CustomerGuestAppBuilderFactory.SeedSingle(db);
        List<CreateCustomerHouseKeepingCategoryIn> createCustomerHouseKeepingCategoryIns = new List<CreateCustomerHouseKeepingCategoryIn>();
        CreateCustomerHouseKeepingCategoryIn createCustomerHouseKeepingCategoryIn = new CreateCustomerHouseKeepingCategoryIn();
        createCustomerHouseKeepingCategoryIn.CustomerId = customer.Id;
        createCustomerHouseKeepingCategoryIn.CustomerGuestAppBuilderId = customerGuest.Id;
        createCustomerHouseKeepingCategoryIns.Add(createCustomerHouseKeepingCategoryIn);
        In.CustomerHouseKeepingCategories = createCustomerHouseKeepingCategoryIns;
    }

    public CreateCustomerHouseKeepingHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}