using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Commands;

public class CreateCustomerGuestsCheckInFormBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerGuestsCheckInFormBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer guests checkIn form builder created successfully.");
    }
}
public class CreateCustomerGuestsCheckInFormBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestsCheckInFormBuilderIn In { get; set; } = new CreateCustomerGuestsCheckInFormBuilderIn();
    public ICommonDataBaseOprationService _common { get; set; }
    public IOptions<HospitioApiStorageAccountOptions> _hospitioApiStorageAccount { get; set; } = A.Fake<IOptions<HospitioApiStorageAccountOptions>>();
    public IUserFilesService _userFilesService { get; set; }
    private IHubContext<ChatHub> _hubContext { get; set; } = A.Fake<IHubContext<ChatHub>>();
    private IChatService _chatService { get; set; }
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = customerFactory.SeedSingle(db);

        In.CustomerId = customer.Id;
        In.Color = "Test";
    }

    public CreateCustomerGuestsCheckInFormBuilderHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response , _common, _hospitioApiStorageAccount , _userFilesService , _hubContext , _chatService);
}
