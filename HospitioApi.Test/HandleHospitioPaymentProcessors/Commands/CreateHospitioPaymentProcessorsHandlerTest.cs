using HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleHospitioPaymentProcessors.Commands.CreateHospitioPaymentProcessorsHandlerTestFixture;


namespace HospitioApi.Test.HandleHospitioPaymentProcessors.Commands;
public class CreateHospitioPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public CreateHospitioPaymentProcessorsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Create hospitio payment processor successful.");
    }
}

public class CreateHospitioPaymentProcessorsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateHospitioPaymentProcessorsIn In { get; set; } = new CreateHospitioPaymentProcessorsIn();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

        var hospitioPaymentProcessorFactory = HospitioPaymentProcessorFactory.SeedSingle(db, paymentProcessor.Id);

        In.PaymentProcessorId = paymentProcessor.Id;
        //In.ClientId = hospitioPaymentProcessorFactory.ClientId;
        //In.Currency = hospitioPaymentProcessorFactory.Currency;
        //In.ClientSecret = hospitioPaymentProcessorFactory.ClientSecret;
    }

    public CreateHospitioPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
