using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilderById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestAppBuilder.Queries;

public class GetCustomerGuestAppBuilderByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerGuestAppBuilderByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomerGuestAppBuilderByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestAppBuilderByIdOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer guest app builder successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _dapper = A.Fake<IDapperRepository>();

        CustomerGuestAppBuilderByIdOut? obj = null;
        A.CallTo(() => _dapper.GetSingle<CustomerGuestAppBuilderByIdOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomerGuestAppBuilderByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerGuestAppBuilderByIdIn In { get; set; } = new GetCustomerGuestAppBuilderByIdIn();
    public CustomerGuestAppBuilderByIdOut CustomerGuestAppBuilderByIdOut { get; set; } = new();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
        var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);

        CustomerGuestAppBuilderByIdOut.Id = customerGuestAppBuilder.Id;
        CustomerGuestAppBuilderByIdOut.CustomerId = customer.Id;
        CustomerGuestAppBuilderByIdOut.CustomerRoomNameId = customerRoomName.Id;
        CustomerGuestAppBuilderByIdOut.Message = "test";
        CustomerGuestAppBuilderByIdOut.SecondaryMessage = "test";
        CustomerGuestAppBuilderByIdOut.LocalExperience = true;
        CustomerGuestAppBuilderByIdOut.Ekey = true;
        CustomerGuestAppBuilderByIdOut.PropertyInfo = true;
        CustomerGuestAppBuilderByIdOut.EnhanceYourStay = true;
        CustomerGuestAppBuilderByIdOut.Reception = true;
        CustomerGuestAppBuilderByIdOut.Housekeeping = true;
        CustomerGuestAppBuilderByIdOut.RoomService = true;
        CustomerGuestAppBuilderByIdOut.Concierge = true;
        CustomerGuestAppBuilderByIdOut.TransferServices = true;
        CustomerGuestAppBuilderByIdOut.IsActive = true;
    }

    public GetCustomerGuestAppBuilderByIdHandler BuildHandler(IDapperRepository dapper) =>
        new(dapper, Response);
}
