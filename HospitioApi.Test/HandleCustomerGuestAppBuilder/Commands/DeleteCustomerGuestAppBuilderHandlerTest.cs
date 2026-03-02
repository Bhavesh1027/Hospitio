using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerAppBuilder;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerGuestAppBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Commands;

public class DeleteCustomerGuestAppBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerGuestAppBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer guest app builder successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = actualId;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customer guest app builder could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomerGuestAppBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerGuestAppBuilderIn In { get; set; } = new DeleteCustomerGuestAppBuilderIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);

        In.Id = customerGuestAppBuilder.Id;
    }

    public DeleteCustomerGuestAppBuilderHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
