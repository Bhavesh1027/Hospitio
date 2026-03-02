using Azure.Core;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategoryHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerEnhanceYourStayCategories.Commands;

public class UpdateCustomerEnhanceYourStayCategoryHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerEnhanceYourStayCategoryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customers enhance your stay category successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var customerBuilderId = _fix.In.CustomerGuestAppBuilderId;
        var customerId = _fix.In.CustomerId;
        _fix.In.CustomerGuestAppBuilderId = 0;
        _fix.In.CustomerId = 0;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers enhance your stay category with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
        _fix.In.CustomerGuestAppBuilderId = customerBuilderId;
        _fix.In.CustomerId = customerId;
    }

}
public class UpdateCustomerEnhanceYourStayCategoryHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerEnhanceYourStayCategoryIn In { get; set; } = new UpdateCustomerEnhanceYourStayCategoryIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerReservation = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuest = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerReservation.Id, customer.Id);

        In.Id = customerGuest.Id;
        In.CustomerGuestAppBuilderId = customerGuest.CustomerGuestAppBuilderId;
        In.CustomerId = customerGuest.CustomerId;
        In.CategoryName = "cat";
    }

    public UpdateCustomerEnhanceYourStayCategoryItemHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}