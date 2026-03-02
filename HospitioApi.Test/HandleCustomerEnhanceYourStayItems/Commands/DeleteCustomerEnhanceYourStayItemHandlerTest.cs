using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItemHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayItems.Commands;

public class DeleteCustomerEnhanceYourStayItemHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerEnhanceYourStayItemHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers enhance your stay  item successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay  item with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerEnhanceYourStayItemHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerEnhanceYourStayItemIn In { get; set; } = new DeleteCustomerEnhanceYourStayItemIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuestAppEnhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id);
        var customerGuest = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id, customerGuestAppEnhanceYourStayCategory.Id);


        In.Id = customerGuest.Id;
    }

    public DeleteCustomerEnhanceYourStayItemHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}