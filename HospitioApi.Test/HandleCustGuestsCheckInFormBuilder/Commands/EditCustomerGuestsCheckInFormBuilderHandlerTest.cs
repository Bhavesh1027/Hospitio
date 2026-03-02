using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.EditCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Commands.EditCustomerGuestsCheckInFormBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestsCheckInFormBuilder.Commands;

public class EditCustomerGuestsCheckInFormBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public EditCustomerGuestsCheckInFormBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer guests checkIn formbuilder edited successfully.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var actualId = _fix.In.Id;
        var customerId = _fix.In.CustomerId;
        _fix.In.CustomerId = 0;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Given customer guests checkIn Id not exist");

        _fix.In.Id = actualId;
        _fix.In.CustomerId = customerId;
    }
}
public class EditCustomerGuestsCheckInFormBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public EditCustomerGuestsCheckInFormBuilderIn In { get; set; } = new EditCustomerGuestsCheckInFormBuilderIn();
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
        var customerGuestsCheckInFormBuilder = CustomerGuestsCheckInFormBuildersFactory.SeedSingle(db, customer.Id);

        In.Id = customerGuestsCheckInFormBuilder.Id;
        In.CustomerId = customer.Id;
    }

    public EditCustomerGuestsCheckInFormBuilderHandler BuildHandler(ApplicationDbContext db) =>
        new(db, Response, _common, _hospitioApiStorageAccount, _userFilesService , _hubContext, _chatService);
}