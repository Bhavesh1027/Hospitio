using FakeItEasy;
using HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReception.Commands.CreateCustomerReceptionHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReception.Commands;

public class CreateCustomerReceptionHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomerReceptionHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer reception category successful.");
    }
}
public class CreateCustomerReceptionHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerReceptionIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appbuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);

        List<CreateCustomerReceptionCategoryIn> createCustomerReceptionItemIns = new List<CreateCustomerReceptionCategoryIn>();
        CreateCustomerReceptionCategoryIn createCustomerReceptionItemIn = new CreateCustomerReceptionCategoryIn();
        createCustomerReceptionItemIn.CustomerId = customer.Id;
        createCustomerReceptionItemIn.CustomerGuestAppBuilderId = appbuilder.Id;
        createCustomerReceptionItemIns.Add(createCustomerReceptionItemIn);
        In.CuastomerReceptionCategories = createCustomerReceptionItemIns;

    }
    public CreateCustomerReceptionHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
       new(db, Response, commonRepository);
}
