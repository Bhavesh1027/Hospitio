using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfo;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfoHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersPropertiesInfo.Commands;

public class DeleteCustomersPropertiesInfoHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public DeleteCustomersPropertiesInfoHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Delete customers property info successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers property info with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class DeleteCustomersPropertiesInfoHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public DeleteCustomersPropertiesInfoIn In { get; set; } = new DeleteCustomersPropertiesInfoIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var propertyInformation = customerProperyInformationFactory.SeedSingle(db);

        In.Id = propertyInformation.Id;
    }

    public DeleteCustomersPropertiesInfoHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}



