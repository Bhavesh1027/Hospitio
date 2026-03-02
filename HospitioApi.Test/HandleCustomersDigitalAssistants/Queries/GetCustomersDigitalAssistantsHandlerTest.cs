using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersDigitalAssistants.Queries;

public class GetCustomersDigitalAssistantsHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersDigitalAssistantsHandlerTest(ThisTestFixture fixture)
    {
        _fix = fixture;
    }

    [Fact]
    public async Task Success()
    {
        var _dappar = A.Fake<IDapperRepository>();
        A.CallTo(() => _dappar.GetAll<CustomersDigitalAssistantsOut>(A<string>.Ignored, null, CancellationToken.None,
            System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomersDigitalAssistantsOut);

        var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get digital assistants successful.");

        var departmentOut = (GetCustomersDigitalAssistantsOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]

    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
        var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Data not available");
    }
}
public class GetCustomersDigitalAssistantsHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomersDigitalAssistantsIn In { get; set; } = new();
    public List<CustomersDigitalAssistantsOut> CustomersDigitalAssistantsOut { get; set; } = new();

    //public GetDepartmentsIn

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        //In.CustomerId = customer.Id;
        var digitalAssistants = CustomersDigitalAssistantsFactory.SeedMany(db, 1);
        In.CustomerId = customer.Id;
        foreach (var item in digitalAssistants)
        {
            CustomersDigitalAssistantsOut obj = new()
            {
                Id = item.Id,
                CustomerId = item.CustomerId,
                Name = item.Name,
                Details = item.Details,
                Icon = item.Icon                
            };
            CustomersDigitalAssistantsOut.Add(obj);
        }
    }
    public GetCustomersDigitalAssistantsHandler BuildHandler(IDapperRepository _dappar) => new(_dappar, Response);
}
