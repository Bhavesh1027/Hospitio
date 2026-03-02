using FakeItEasy;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtrasHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerPropertyExtras.Commands;

public class UpdateCustomerPropertyExtrasHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerPropertyExtrasHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Update customer property extra successful.");
    }
}
public class UpdateCustomerPropertyExtrasHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerPropertyExtrasIn In { get; set; } = new UpdateCustomerPropertyExtrasIn();
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customerProperty = customerProperyInformationFactory.SeedSingle(db);
        var propertyExtra = propertyExtraFactory.SeedSingle(db);

        List<CustomerPropertyExtrasIn> customerPropertyExtrasIns = new List<CustomerPropertyExtrasIn>();
        CustomerPropertyExtrasIn customerPropertyExtrasIn = new CustomerPropertyExtrasIn();
        customerPropertyExtrasIn.Id = propertyExtra.Id;
        customerPropertyExtrasIn.CustomerPropertyInformationId = customerProperty.Id;
        customerPropertyExtrasIns.Add(customerPropertyExtrasIn);
        In.CustomerPropertyExtrasIns = customerPropertyExtrasIns;

    }

    public UpdateCustomerPropertyExtrasHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, commonRepository, Response);
}