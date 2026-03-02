using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartmentById;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
using HospitioApi.Core.HandleProduct.Queries.GetProductById;
using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsByIdHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomersDigitalAssistants.Queries;

public class GetCustomersDigitalAssistantsByIdHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;
    public GetCustomersDigitalAssistantsByIdHandlerTest(ThisTestFixture fixture) => _fix = fixture;
    [Fact]
    public async Task Success()
    {
        var _dapper = A.Fake<IDapperRepository>();

        A.CallTo(() => _dapper.GetSingle<CustomersDigitalAssistantsOut>(A<string>.Ignored, null, CancellationToken.None,
            CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.CustomersDigitalAssistantsOut);

        var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);
        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Get customers digital assistant successful.");

        var departmentOut = (GetCustomersDigitalAssistantsByIdOut)result.Response;
        Assert.NotNull(departmentOut);
    }
    [Fact]
    public async Task Not_Found_Error()
    {
        Mock<IDapperRepository> _dappar = new Mock<IDapperRepository>();

        var actualId = _fix.CustomersDigitalAssistantsOut.Id;
        _fix.CustomersDigitalAssistantsOut.Id = 0;

        var result = await _fix.BuildHandler(_dappar.Object).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Customers digital assistant could not be found");

        _fix.CustomersDigitalAssistantsOut.Id = actualId;
    }
}

public class GetCustomersDigitalAssistantsByIdHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public GetCustomersDigitalAssistantsByIdIn In { get; set; } = new();
    public CustomersDigitalAssistantsOut CustomersDigitalAssistantsOut { get; set; } = new();

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var digitalAssistant = CustomersDigitalAssistantsFactory.SeedSingle(db,customer.Id);
        In.Id = digitalAssistant.Id;
        CustomersDigitalAssistantsOut = new()
        {
           Id = digitalAssistant.Id,
           CustomerId = customer.Id,
           Name = digitalAssistant.Name,
           Details = digitalAssistant.Details,
           Icon = digitalAssistant.Icon,
        };
    }
    public GetCustomersDigitalAssistantsByIdHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
}
