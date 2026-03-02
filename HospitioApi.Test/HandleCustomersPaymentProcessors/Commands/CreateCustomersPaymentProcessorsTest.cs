using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessorsHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomersPaymentProcessors.Commands;
public class CreateCustomersPaymentProcessorsTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateCustomersPaymentProcessorsTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create customer payment processor successful.");
    }
}

public class CreateCustomersPaymentProcessorsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomersPaymentProcessorsIn In { get; set; } = new CreateCustomersPaymentProcessorsIn();
    public string CustomerId { get; set; } = string.Empty;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);


        var customerPaymentProcessorFactory = CustomerPaymentProcessorFactory.SeedSingle(db, customer.Id,paymentProcessor.Id);
       
        In.CustomerId=customer.Id;
        In.PaymentProcessorId=paymentProcessor.Id;
        //In.ClientId=customerPaymentProcessorFactory.ClientId;
        //In.Currency=customerPaymentProcessorFactory.Currency;
        //In.ClientSecret=customerPaymentProcessorFactory.ClientSecret;
    }

    public CreateCustomersPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}