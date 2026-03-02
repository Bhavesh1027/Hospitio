using FakeItEasy;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeepingHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerHouseKeeping.Commands;

public class DeleteCustomerHouseKeepingHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerHouseKeepingHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers house keeping successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers house keeping with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerHouseKeepingHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerHouseKeepingIn In { get; set; } = new DeleteCustomerHouseKeepingIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuest = housekeepingCategoryFactory.SeedSingle(db, customerGuestApp.Id, customer.Id);

        In.Id = customerGuest.Id;
    }

    public DeleteCustomerHouseKeepingHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}