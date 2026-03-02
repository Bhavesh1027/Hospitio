using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtras;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtrasHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Commands;

public class DeleteCustomerPropertyExtrasHandlerTest: IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerPropertyExtrasHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer property extra successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer property extra could not be found.");

        _fix.In.Id = actualId;
    }
}
public class DeleteCustomerPropertyExtrasHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerPropertyExtrasIn In { get; set; } = new DeleteCustomerPropertyExtrasIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerGuestApp = propertyExtraFactory.SeedSingle(db);

        In.Id = customerGuestApp.Id;
    }

    public DeleteCustomerPropertyExtrasHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}