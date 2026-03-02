using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands.DeleteCustomerPropertyEmergencyNumberHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands;

public class DeleteCustomerPropertyEmergencyNumberHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomerPropertyEmergencyNumberHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customer property emergency number successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer property emergency number with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomerPropertyEmergencyNumberHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomerPropertyEmergencyNumberIn In { get; set; } = new DeleteCustomerPropertyEmergencyNumberIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
        var propertyEmergencyNumber = CustomerPropertyEmergencyNumberFactory.SeedSingle(db, propertyInfo.Id);

        In.Id = propertyEmergencyNumber.Id;
    }

    public DeleteCustomerPropertyEmergencyNumberHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}


