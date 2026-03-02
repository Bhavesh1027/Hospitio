using Azure.Core;
using HospitioApi.Core.HandlePaymentProcessors.Commands.UpdatePaymentProcessors;
using HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using Xunit.Abstractions;
using ThisTestFixture = HospitioApi.Test.HandlePaymentProcessors.Commands.UpdatePaymentProcessorsHandlerTestFixture;

namespace HospitioApi.Test.HandlePaymentProcessors.Commands;

public class UpdatePaymentProcessorsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    private readonly ITestOutputHelper _output;

    public UpdatePaymentProcessorsHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
    {
        _fix = fixture;
        _output = output;
    }

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update payment processors successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Payment processor with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }


}

public class UpdatePaymentProcessorsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdatePaymentProcessorsIn In { get; set; } = new UpdatePaymentProcessorsIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);

        In.Id = paymentProcessor.Id;
        In.Name = paymentProcessor.GRName;

    }

    public UpdatePaymentProcessorsHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response);
}

