using Azure.Core;
using HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistantsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersDigitalAssistants.Commands;

public class CreateCustomersDigitalAssistantsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomersDigitalAssistantsHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.Name;
        _fix.In.Name = "Test2";
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);        

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer digital assistant successful.");
    }
    [Fact]
    public async Task Already_Exists_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualName = _fix.In.Name;
        _fix.In.Name = "Test";
        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"The digital assistant {_fix.In.Name} already exists.");

        _fix.In.Name = actualName;
    }
}

public class CreateCustomersDigitalAssistantsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomersDigitalAssistantsIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        var customer = CustomerFactory.SeedSingle(db);

        var digitalassistant = CustomersDigitalAssistantsFactory.SeedSingle(db,customer.Id);
        In.CustomerId = customer.Id;
        In.Name = "Test";
        In.Details = "Test";
        In.Icon = "Test";
    }
    public CreateCustomersDigitalAssistantsHandler BuildHandler(ApplicationDbContext db) =>
       new(db, Response);
}
