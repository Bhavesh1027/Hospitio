using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.MultiTenancy;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;
using FakeItEasy;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerReception.Commands.UpdateCustomerReceptionHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerReception.Commands;

public class UpdateCustomerReceptionHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerReceptionHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, _fix.CustomerId.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer reception successful.");
    }
}
public class UpdateCustomerReceptionHandlerTestFixture : DbFixture
{
    public int CustomerId { get; set; }
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerReceptionIn In { get; set; } = new UpdateCustomerReceptionIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var receptionCategory = CustomerReceptionCategoryFactory.SeedSingle(db, customer.Id, appBuilder.Id);
        CustomerId = customer.Id;
        //List<UpdateCustomerReceptionCategoryIn> updateCustomerReceptionCategoryIns = new List<UpdateCustomerReceptionCategoryIn>();
        UpdateCustomerReceptionCategoryIn updateCustomerReceptionCategoryIn = new UpdateCustomerReceptionCategoryIn();
        updateCustomerReceptionCategoryIn.Id = receptionCategory.Id;
        updateCustomerReceptionCategoryIn.CustomerGuestAppBuilderId = appBuilder.Id;
        //updateCustomerReceptionCategoryIns.Add(updateCustomerReceptionCategoryIn);
        In.CustomerReceptionCategories = updateCustomerReceptionCategoryIn;

    }

    public UpdateCustomerReceptionHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository);
}
