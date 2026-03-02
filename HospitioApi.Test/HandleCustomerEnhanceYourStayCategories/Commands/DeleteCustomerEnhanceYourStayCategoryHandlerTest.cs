using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Commands;

public class DeleteCustomerEnhanceYourStayCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerEnhanceYourStayCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers enhance your stay category successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay category with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerEnhanceYourStayCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerEnhanceYourStayCategoryIn In { get; set; } = new DeleteCustomerEnhanceYourStayCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerReservation = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuest = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerReservation.Id, customer.Id);

        In.Id = customerGuest.Id;
    }

    public DeleteCustomerEnhanceYourStayCategoryHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}