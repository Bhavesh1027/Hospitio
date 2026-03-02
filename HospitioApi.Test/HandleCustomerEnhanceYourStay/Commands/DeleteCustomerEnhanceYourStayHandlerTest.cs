using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStay;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStayHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStay.Commands;

public class DeleteCustomerEnhanceYourStayHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerEnhanceYourStayHandlerTest(ThisTestFixture fixture) => _fix = fixture;

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

        var actualId = _fix.In.CategoryId;
        _fix.In.CategoryId = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay category with Id {_fix.In.CategoryId} could not be found.");

        _fix.In.CategoryId = actualId;
    }
}
public class DeleteCustomerEnhanceYourStayHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerEnhanceYourStayIn In { get; set; } = new DeleteCustomerEnhanceYourStayIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuest = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerGuestApp.Id, customer.Id);

        In.CategoryId = customerGuest.Id;
    }

    public DeleteCustomerEnhanceYourStayHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}