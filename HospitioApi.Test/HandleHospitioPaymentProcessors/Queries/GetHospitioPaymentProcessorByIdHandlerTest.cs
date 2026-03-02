using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessorById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessorByIdHandlerTestFixture;


namespace HospitioApi.Test.HandleHospitioPaymentProcessors.Queries;

public class GetHospitioPaymentProcessorByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetHospitioPaymentProcessorByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<HospitioPaymentProcessorByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.HospitioPaymentProcessorByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get hospitio payment processor successful.");

        var hospitioPaymentProcessorsByIdOut = (GetHospitioPaymentProcessorByIdOut)result.Response;
        Assert.NotNull(hospitioPaymentProcessorsByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.In.Id = actualId;
    }
}

public class GetHospitioPaymentProcessorByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetHospitioPaymentProcessorByIdIn In { get; set; } = new GetHospitioPaymentProcessorByIdIn();
    public HospitioPaymentProcessorByIdOut HospitioPaymentProcessorByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);
        var hospitioPaymentProcessorFactory = HospitioPaymentProcessorFactory.SeedSingle(db, paymentProcessor.Id);

        HospitioPaymentProcessorByIdOut = new()
        {
            Id = hospitioPaymentProcessorFactory.Id,
            PaymentProcessorId = paymentProcessor.Id,
            //ClientId = hospitioPaymentProcessorFactory.ClientId,
            //ClientSecret = hospitioPaymentProcessorFactory.ClientSecret,
            //Currency = hospitioPaymentProcessorFactory.Currency

        };
    }

    public GetHospitioPaymentProcessorByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}
