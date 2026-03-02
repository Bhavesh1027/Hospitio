using FakeItEasy;
using HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using ThisTestFixture = HospitioApi.Test.HandleCustomerRoomService.Commands.DeleteCustomerRoomServicesHandlerTestFixture;
using Xunit;
using Azure.Core;

namespace HospitioApi.Test.HandleCustomerRoomService.Commands;

public class DeleteCustomerRoomServicesHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerRoomServicesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers room service successful.");
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
        Assert.True(result.Failure!.Message == $"Customers room service with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerRoomServicesHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerRoomServiceIn In { get; set; } = new DeleteCustomerRoomServiceIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var roomServiceCategory = CustomerRoomServiceCategoryFactory.SeedSingle(db, customer.Id, customerGuestApp.Id);

        In.Id = roomServiceCategory.Id;
    }

    public DeleteCustomerRoomServicesHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}
