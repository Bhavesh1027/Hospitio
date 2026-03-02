using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsByIdTestFixture;


namespace HospitioApi.Test.HandleCustomersPaymentProcessors.Queries;

public class GetCustomersPaymentProcessorsByIdTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersPaymentProcessorsByIdTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomersPaymentProcessorsByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomersPaymentProcessorsByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(new() { Id = _fix.Id}), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customers payment processors successful.");

        var customersPaymentProcessorsByIdOut = (GetCustomersPaymentProcessorsByIdOut)result.Response;
        Assert.NotNull(customersPaymentProcessorsByIdOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.Id;
        _fix.Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(new() { Id = _fix.Id }), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.Id = actualId;
    }
}

public class GetCustomersPaymentProcessorsByIdTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomersPaymentProcessorsByIdOut> CustomersPaymentProcessorsByIdOut { get; set; } = new();
    public int Id;
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);
        var customerGuestAlert = CustomerPaymentProcessorFactory.SeedSingle(db, customer.Id,paymentProcessor.Id);
        Id = customerGuestAlert.Id;

       
        CustomersPaymentProcessorsByIdOut obj = new()
        {
            Id = customerGuestAlert.Id,
            CustomerId = customer.Id
        };
        CustomersPaymentProcessorsByIdOut.Add(obj);
    }

    public GetCustomersPaymentProcessorsByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}