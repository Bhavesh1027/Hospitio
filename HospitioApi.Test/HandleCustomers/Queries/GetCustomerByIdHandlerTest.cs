using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Queries.GetCustomerByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomers.Queries;

public class GetCustomerByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomerByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetAllJsonData<CustomerByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerByIdForHospitioOuts);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, string.Empty), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customer successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        var _dapper = A.Fake<IDapperRepository>();

        List<CustomerByIdOut>? obj = null;
        A.CallTo(() => _dapper.GetAllJsonData<CustomerByIdOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(obj);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In, string.Empty), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}

public class GetCustomerByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomerByIdIn In { get; set; } = new();
    public List<CustomerByIdOut> CustomerByIdForHospitioOuts { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customers = CustomerFactory.SeedMany(db, 1);

        foreach (var customer in customers)
        {
            CustomerByIdOut obj = new()
            {
                Id = customer.Id,
                BusinessName = customer.BusinessName,
                Email = customer.Email,
                BusinessCloseTime = customer.BusinessCloseTime,
                BusinessStartTime = customer.BusinessStartTime,
                BusinessTypeId = customer.BusinessTypeId,
                ClientDoamin = customer.ClientDoamin,
                Cname = customer.Cname,
                DoNotDisturbGuestEndTime = customer.DoNotDisturbGuestEndTime,
                DoNotDisturbGuestStartTime = customer.DoNotDisturbGuestStartTime,
                IncomingTranslationLangage = customer.IncomingTranslationLangage,
                IsActive = customer.IsActive,
            };

            CustomerByIdForHospitioOuts.Add(obj);
        }
    }
    public GetCustomerByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}
