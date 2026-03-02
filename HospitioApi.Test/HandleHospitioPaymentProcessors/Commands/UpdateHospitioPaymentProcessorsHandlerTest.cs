using HospitioApi.Core.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessors;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;


using ThisTestFixture = HospitioApi.Test.HandleHospitioPaymentProcessors.Commands.UpdateHospitioPaymentProcessorsHandlerTestFixture;


namespace HospitioApi.Test.HandleHospitioPaymentProcessors.Commands;
public class UpdateHospitioPaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateHospitioPaymentProcessorsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update hospitio payment processor successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Hospitio payment processor with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;

    }
}

public class UpdateHospitioPaymentProcessorsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateHospitioPaymentProcessorsIn In { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

        var hospitioPaymentProcessorFactory = HospitioPaymentProcessorFactory.SeedSingle(db, paymentProcessor.Id);

        In.Id = hospitioPaymentProcessorFactory.Id;
        In.PaymentProcessorId = paymentProcessor.Id;
        In.ClientId = "Payment Processor Client";
        In.Currency = "Dollar";
        In.ClientSecret = "Client Secret";
    }

    public UpdateHospitioPaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}
