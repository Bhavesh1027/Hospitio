using FakeItEasy;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersConcierge.Commands.CreateCustomerConciergeHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomersConcierge.Commands;

public class CreateCustomerConciergeHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerConciergeHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer concierge category successful.");
    }
}
public class CreateCustomerConciergeHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerConciergeIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appbuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);

        List<CreateCustomerConciergeCategoryIn> createCustomerConciergeCategoryIns = new List<CreateCustomerConciergeCategoryIn>();
        CreateCustomerConciergeCategoryIn createCustomerConciergeCategoryIn = new CreateCustomerConciergeCategoryIn();
        createCustomerConciergeCategoryIn.CustomerId = customer.Id;
        createCustomerConciergeCategoryIn.CustomerGuestAppBuilderId = appbuilder.Id;
        createCustomerConciergeCategoryIns.Add(createCustomerConciergeCategoryIn);
        In.CustomerConciergeCategories = createCustomerConciergeCategoryIns;

    }
    public CreateCustomerConciergeHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
       new(db, Response, commonRepository);
}
