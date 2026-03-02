using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;

using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Queries.GetCustomerPropertyEmergencyNumberByIdHandlerTestFixture;


namespace HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Queries;

public class GetCustomerPropertyEmergencyNumberByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerPropertyEmergencyNumberByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerPropertyEmergencyNumberByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerPropertyEmergencyNumberByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer property emergency number successful.");

        var customerPropertyEmergencyNumberByIdOut = (GetCustomerPropertyEmergencyNumberByIdOut)result.Response;
        Assert.NotNull(customerPropertyEmergencyNumberByIdOut);
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

public class GetCustomerPropertyEmergencyNumberByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerPropertyEmergencyNumberByIdIn In { get; set; } = new GetCustomerPropertyEmergencyNumberByIdIn();
    public CustomerPropertyEmergencyNumberByIdOut CustomerPropertyEmergencyNumberByIdOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
        var propertyEmergencyNumber = CustomerPropertyEmergencyNumberFactory.SeedSingle(db, propertyInfo.Id);

        CustomerPropertyEmergencyNumberByIdOut = new()
        {
            Id = propertyEmergencyNumber.Id,
            CustomerPropertyInformationId = propertyInfo.Id,
            Name = propertyEmergencyNumber.Name,
            PhoneCountry = propertyEmergencyNumber.PhoneCountry,
            PhoneNumber = propertyEmergencyNumber.PhoneNumber

        };
    }

    public GetCustomerPropertyEmergencyNumberByIdHandler BuildHandler(IDapperRepository _dapper) =>
        new(_dapper, Response);
}

