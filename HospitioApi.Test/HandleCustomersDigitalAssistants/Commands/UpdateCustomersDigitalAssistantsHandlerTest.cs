using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Data;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistantsHandlerTestFixture;
using Azure.Core;

namespace HospitioApi.Test.HandleCustomersDigitalAssistants.Commands;

public class UpdateCustomersDigitalAssistantsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public UpdateCustomersDigitalAssistantsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customers digital assistants");
    }
    
    
    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The digital assistant {_fix.In.Name} already exists.");

        _fix.In.Id = actualId;
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualId = _fix.In.Id;
        var actualName = _fix.In.Name;
        _fix.In.Name = "TestTest";
        _fix.In.Id = 0;
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers digital assistants with Id {_fix.In.Id} could not be found.");
        _fix.In.Id = actualId;
        _fix.In.Name = actualName;
    }
}

public class UpdateCustomersDigitalAssistantsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();

    public UpdateCustomersDigitalAssistantsIn In { get; set; } = new();
    public CustomerDigitalAssistant CustomerDigitalAssistant { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();

        var customer = CustomerFactory.SeedSingle(db);
        var digitalAssistant = CustomersDigitalAssistantsFactory.SeedSingle(db, customer.Id);
        CustomerDigitalAssistant = digitalAssistant;

        In.Id = digitalAssistant.Id;
        In.Name = digitalAssistant.Name;
        In.Details = "Test";
        In.Icon = "Test";
    }
    public UpdateCustomersDigitalAssistantsHandler BuildHandler(ApplicationDbContext db) => new(db, Response);
}
