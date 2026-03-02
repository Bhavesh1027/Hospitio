using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilders;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuildersHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries;

public class GetCustomerGuestAppBuildersHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerGuestAppBuildersHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerGuestAppBuildersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerGuestAppBuildersOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest app builder successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAll<CustomerGuestAppBuildersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new List<CustomerGuestAppBuildersOut>());

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomerGuestAppBuildersHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerGuestAppBuildersIn In { get; set; } = new GetCustomerGuestAppBuildersIn();
    public List<CustomerGuestAppBuildersOut> CustomerGuestAppBuildersOuts { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedMany(db, 1);

        In.CustomerId = customer.Id;

        foreach (var obj in customerGuestAppBuilder)
        {
            CustomerGuestAppBuildersOut customerGuestAppBuildersOut = new()
            {
                Id = obj.Id,
                CustomerId = obj.CustomerId,
                CustomerRoomNameId = obj.CustomerRoomNameId,
                Message = "test",
                SecondaryMessage = "test",
                LocalExperience = true,
                Ekey = true,
                PropertyInfo = true,
                EnhanceYourStay = true,
                Reception = true,
                Housekeeping = true,
                RoomService = true,
                Concierge = true,
                TransferServices = true,
                IsActive = true,
            };

            CustomerGuestAppBuildersOuts.Add(customerGuestAppBuildersOut);
        };
    }

    public GetCustomerGuestAppBuildersHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
