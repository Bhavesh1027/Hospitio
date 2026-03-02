using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsTestFixture;


namespace HospitioApi.Test.HandleCustomersPaymentProcessors.Queries;

public class GetCustomersPaymentProcessorsTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersPaymentProcessorsTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomersPaymentProcessorsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomersPaymentProcessorsOut);
        
        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer payment processors successful.");

        var customersPaymentProcessorsOut = (GetCustomersPaymentProcessorsOut)result.Response;
        Assert.NotNull(customersPaymentProcessorsOut);
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

        var actualId = _fix.CustomersPaymentProcessorsOut.FirstOrDefault().Id;
        _fix.CustomersPaymentProcessorsOut!.FirstOrDefault().Id = 0;

        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Data not available");

        _fix.CustomersPaymentProcessorsOut.FirstOrDefault().Id = actualId;
    }
}

public class GetCustomersPaymentProcessorsTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public List<CustomersPaymentProcessorsOut> CustomersPaymentProcessorsOut { get; set; } = new();
    public GetCustomersPaymentProcessorsIn In { get; set; } = new();
    

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var paymentProcessor = PaymentProcessorFactory.SeedSingle(db);
        var customerPaymentProcessorFactory = CustomerPaymentProcessorFactory.SeedSingle(db, customer.Id, paymentProcessor.Id);

        In.CustomerId = customer.Id;
        In.PageSize = 10;
        In.PageNo = 1;


        CustomersPaymentProcessorsOut obj = new()
        {
            Id = customerPaymentProcessorFactory.Id,
            CustomerId = customer.Id,
            PaymentProcessorId = paymentProcessor.Id,
            //ClientId = customerPaymentProcessorFactory.ClientId,
            //ClientSecret = customerPaymentProcessorFactory.ClientSecret,
            //Currency = customerPaymentProcessorFactory.Currency
        };
        CustomersPaymentProcessorsOut.Add(obj);
    }

    public GetCustomersPaymentProcessorsHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}