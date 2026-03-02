using HospitioApi.Core.HandleDisplayorder.Commands.UpdateDisplayorder;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandleDisplayorder.Commands.UpdateDisplayorderHandleTestFixture;

namespace HospitioApi.Test.HandleDisplayorder.Commands;

public class UpdateDisplayorderHandleTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public UpdateDisplayorderHandleTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update Successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Screen with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }


}

public class UpdateDisplayorderHandleTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateDisplayorderIn In { get; set; } = new UpdateDisplayorderIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
        var displayOrder = ScreenDisplayOrderAndStatusesFactory.SeedSingle(db, propertyInfo.Id,1);

        In.Id = displayOrder.Id;
        In.ScreenName = displayOrder.ScreenName;
        In.JsonData = displayOrder.JsonData;
        In.RefrenceId = propertyInfo.Id;

    }

    public UpdateDisplayorderHandle BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}

