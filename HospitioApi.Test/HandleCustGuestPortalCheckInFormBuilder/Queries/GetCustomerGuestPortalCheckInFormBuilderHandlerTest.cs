using FakeItEasy;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Queries;

public class GetCustomerGuestPortalCheckInFormBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public GetCustomerGuestPortalCheckInFormBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        A.CallTo(() => _dapper.GetAllJsonData<GetCustomerGuestResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestResponseOut);
        
        var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In),CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer reservation successful.");
    }

    [Fact]
    public async Task NotFound_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        List<GetCustomerGuestResponseOut>? obj = null;
        A.CallTo(() => _dapper.GetAllJsonData<GetCustomerGuestResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

        var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In),CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }

    [Fact]
    public async Task LinkExpired_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var actualDate = _fix.CustomerGuestResponseOut[0].GetCustomerReservationResponseOut.CheckoutDate;
        _fix.CustomerGuestResponseOut[0].GetCustomerReservationResponseOut.CheckoutDate = DateTime.UtcNow.AddDays(-15);

        A.CallTo(() => _dapper.GetAllJsonData<GetCustomerGuestResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomerGuestResponseOut);

        var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Link is expired");

        _fix.CustomerGuestResponseOut[0].GetCustomerReservationResponseOut.CheckoutDate = actualDate;
    }
}

public class GetCustomerGuestPortalCheckInFormBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerGuestPortalCheckInFormBuilderIn In { get; set; } = new GetCustomerGuestPortalCheckInFormBuilderIn();
    public List<GetCustomerGuestResponseOut> CustomerGuestResponseOut { get; set; } = new List<GetCustomerGuestResponseOut>();
    public ApplicationDbContext db;

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);
        In.GuestId = customerGuest.Id;
        In.ReservationId = customerReservation.Id;
        GetCustomerGuestResponseOut obj = new()
        {
            IsCoGuest = customerGuest.IsCoGuest,
            Name = customerGuest.Firstname,
            GetCustomerReservationResponseOut = new()
            {
                Id = customerReservation.Id,
                CheckoutDate = DateTime.UtcNow.AddDays(1)
            }
        };

        CustomerGuestResponseOut.Add(obj);
    }

    public GetCustomerGuestPortalCheckInFormBuilderHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db) =>
        new(_dapper,Response,db);
}
