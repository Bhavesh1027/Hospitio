using Azure.Core;
using FakeItEasy;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConcierge;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersConcierge.Commands.DeleteCustomerConciergeHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersConcierge.Commands;

public class DeleteCustomerConciergeHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerConciergeHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers concierge successful.");
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
        Assert.True(result.Failure!.Message == $"Customers concierge with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerConciergeHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerConciergeIn In { get; set; } = new DeleteCustomerConciergeIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var conciergeCategory = CustomerConciergeCategoryFactory.SeedSingle(db, customer.Id, customerGuestApp.Id);

        In.Id = conciergeCategory.Id;
    }

    public DeleteCustomerConciergeHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response);
}
