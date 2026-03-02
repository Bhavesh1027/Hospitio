using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneys;
using HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleGuestRequest.Queries.GetGuestRequestsHandlerTestFixture;

namespace HospitioApi.Test.HandleGuestRequest.Queries;

public class GetGuestRequestsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetGuestRequestsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<GuestRequestsOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.GuestRequestsOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In,_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get guest requests successful.");

        var departmentOut = (GetGuestRequestsOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In,_fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}
public class GetGuestRequestsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetGuestRequestsIn In { get; set; } = new();
    public int CustomerId { get; set; } 
    public List<GuestRequestsOut> GuestRequestsOut { get; set; } = new();

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var appbuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        var guest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);
        var enhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, appbuilder.Id, customer.Id);
        //In.CustomerId = customer.Id;
        var guestRequests = GuestRequestFactory.SeedMany(db, 1);       
        foreach (var item in guestRequests)
        {
            GuestRequestsOut obj = new()
            {
                Id = item.Id
                //RequestType = item.RequestType,
                //MinuteValue = item.MinuteValue,
                //DayValue = item.DayValue,
                //MonthValue  = item.MonthValue,
                //YearValue = item.YearValue,
                //HourValue = item.HourValue
            };
            GuestRequestsOut.Add(obj);
        }
        In.PageSize = 10;
        In.PageNo = 1;
        CustomerId = customer.Id;
    }
    public GetGuestRequestsHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}
