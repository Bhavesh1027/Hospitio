using Azure.Core;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistantsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersDigitalAssistants.Commands;

public class UpdateIsActiveCustomersDigitalAssistantsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateIsActiveCustomersDigitalAssistantsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == $"Digital assistant with {_fix.In.Id} has been updated. IsActive has been set to {_fix.In.IsActive}.");
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers digital assistant with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}
public class UpdateIsActiveCustomersDigitalAssistantsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public UpdateIsActiveCustomersDigitalAssistantsIn In { get; set; } = new();
    public CustomerDigitalAssistant CustomerDigitalAssistant { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var digitalAssistant = CustomersDigitalAssistantsFactory.SeedSingle(db,customer.Id);
        CustomerDigitalAssistant = digitalAssistant;

        In.Id = digitalAssistant.Id;
        In.IsActive = true;
    }
    public UpdateIsActiveCustomersDigitalAssistantsHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}
