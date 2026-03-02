
using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;


using ThisTestFixture = HospitioApi.Test.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessorsHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomersPaymentProcessors.Commands;
public class UpdateCustomersPaymentProcessorsTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomersPaymentProcessorsTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customers payment processors successfull.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customers payment processors with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
      
    }
}

public class UpdateCustomersPaymentProcessorsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomersPaymentProcessorsIn In { get; set; } = new();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        CustomerId = customer.Id.ToString();

        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

        var customerPaymentProcessorFactory = CustomerPaymentProcessorFactory.SeedSingle(db, customer.Id,paymentProcessor.Id);

        In.Id = customerPaymentProcessorFactory.Id;
        In.CustomerId = customer.Id;
        In.PaymentProcessorId = paymentProcessor.Id;
        In.ClientId = "Payment Processor Client";
        In.Currency = "Dollar";
        In.ClientSecret = "Client Secret";
    }

    public UpdateCustomersPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}