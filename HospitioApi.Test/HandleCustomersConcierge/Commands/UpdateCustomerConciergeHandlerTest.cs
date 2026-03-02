using FakeItEasy;
using HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
using HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersConcierge.Commands.UpdateCustomerConciergeHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersConcierge.Commands;

public class UpdateCustomerConciergeHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateCustomerConciergeHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer concierge category successful.");
    }
}
public class UpdateCustomerConciergeHandlerTestFixture : DbFixture
{
    public int CustomerId { get; set; }
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerConciergeIn In { get; set; } = new UpdateCustomerConciergeIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var receptionCategory = CustomerConciergeCategoryFactory.SeedSingle(db, customer.Id, appBuilder.Id);

        CustomerId = customer.Id;
        List<UpdateCustomerConciergeCategoryIn> updateCustomerConciergeCategoryIns = new List<UpdateCustomerConciergeCategoryIn>();
        UpdateCustomerConciergeCategoryIn updateCustomerConciergeCategoryIn = new UpdateCustomerConciergeCategoryIn();
        updateCustomerConciergeCategoryIn.Id = receptionCategory.Id;
        updateCustomerConciergeCategoryIn.CustomerGuestAppBuilderId = appBuilder.Id;
        updateCustomerConciergeCategoryIns.Add(updateCustomerConciergeCategoryIn);
        In.CustomerConciergeCategories = updateCustomerConciergeCategoryIn;

    }

    public UpdateCustomerConciergeHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}
