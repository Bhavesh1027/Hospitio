//using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
//using HospitioApi.Core.Services.HandlerResponse;
//using HospitioApi.Data;
//using Xunit;
//using Xunit.Abstractions;
//using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands.UpdateCustomerPropertyEmergencyNumberHandlerTestFixture;

//namespace HospitioApi.Test.HandleCustomerPropertyEmergencyNumber.Commands;

//public class UpdateCustomerPropertyEmergencyNumberHandlerTest : IClassFixture<ThisTestFixture>
//{
//    private readonly ThisTestFixture _fix;
//    private readonly ITestOutputHelper _output;

//    public UpdateCustomerPropertyEmergencyNumberHandlerTest(ThisTestFixture fixture, ITestOutputHelper output)
//    {
//        _fix = fixture;
//        _output = output;
//    }

//    [Fact]
//    public async Task Update_Success()
//    {
//        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

//        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

//        Assert.True(result.HasResponse);
//        Assert.True(result.Response!.Message == "Update customer property emergency number successful.");
//    }

//    [Fact]
//    public async Task Add_Success()
//    {
//        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

//        var actualId = _fix.In.Id;
//        _fix.In.Id = 0;

//        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

//        Assert.True(result.HasResponse);
//        Assert.True(result.Response!.Message == "Create customer property emergency number successful.");

//        _fix.In.Id = actualId;
//    }


//    [Fact]
//    public async Task Not_Found_Error()
//    {
//        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

//        var actualId = _fix.In.Id;
//        _fix.In.Id = 999;

//        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

//        Assert.True(result.HasFailure);
//        Assert.True(result.Failure!.Message == $"Customer property emergency number with Id {_fix.In.Id} could not be found.");

//        _fix.In.Id = actualId;
//    }


//}

//public class UpdateCustomerPropertyEmergencyNumberHandlerTestFixture : DbFixture
//{
//    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
//    public UpdateCustomerPropertyEmergencyNumberIn In { get; set; } = new UpdateCustomerPropertyEmergencyNumberIn();
//    protected override void Seed()
//    {
//        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
//        db.Database.EnsureDeleted();
//        db.Database.EnsureCreated();

//        var customer = CustomerFactory.SeedSingle(db);
//        var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
//        var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
//        var propertyEmergencyNumber = CustomerPropertyEmergencyNumberFactory.SeedSingle(db, propertyInfo.Id);

//        In.Id = propertyEmergencyNumber.Id;
//        In.CustomerPropertyInformationId = propertyInfo.Id;
//        In.Name = propertyEmergencyNumber.Name;
//        In.PhoneCountry = propertyEmergencyNumber.PhoneCountry;
//        In.PhoneNumber = propertyEmergencyNumber.PhoneNumber;

//    }

//    public UpdateCustomerPropertyEmergencyNumberHandler BuildHandler(ApplicationDbContext db) =>
//        new(db, Response);
//}

