using Azure.Core;
using FakeItEasy;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReception.Commands.DeleteCustomerReceptionHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReception.Commands;

public class DeleteCustomerReceptionHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerReceptionHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers reception successful.");
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
        Assert.True(result.Failure!.Message == $"Customers reception with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerReceptionHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerReceptionIn In { get; set; } = new DeleteCustomerReceptionIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customer = customerFactory.SeedSingle(db);
        var customerGuestAppReception = CustomerReceptionCategoryFactory.SeedSingle(db, customer.Id, customerGuestApp.Id);

        In.Id = customerGuestAppReception.Id;
    }

    public DeleteCustomerReceptionHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}
