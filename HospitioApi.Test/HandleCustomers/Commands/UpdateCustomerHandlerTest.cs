using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomers.Commands.UpdateCustomerHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomers.Commands;

public class UpdateCustomerHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public UpdateCustomerHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success_UserType_Customer()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        A.CallTo(() => _commonRepository.CustomersUpdate(_fix.In, _fix.customerModel, db, CancellationToken.None, _fix.sendEmail )).WhenArgumentsMatch(x => x != null).Returns(_fix.customerModel);

        A.CallTo(() => _commonRepository.CustomerRoomNamesUpdate(_fix.In.UpdateCustomerRoomNamesIns, _fix.customerModel.Id, db, CancellationToken.None, UserTypeEnum.Customer)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerRoomNames);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, UserTypeEnum.Customer, _fix.In.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer update successful.");
    }

    [Fact]
    public async Task Success_UserType_Hospitio()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        A.CallTo(() => _commonRepository.CustomersUpdate(_fix.In, _fix.customerModel, db, CancellationToken.None, _fix.sendEmail)).WhenArgumentsMatch(x => x != null).Returns(_fix.customerModel);

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, UserTypeEnum.Hospitio, _fix.In.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer update successful.");
    }

    [Fact]
    public async Task Not_Found_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _commonRepository = A.Fake<ICommonDataBaseOprationService>();

        var actualId = _fix.In.Id;
        _fix.In.Id = 0;

        var result = await _fix.BuildHandler(db, _commonRepository).Handle(new(_fix.In, UserTypeEnum.Customer, _fix.In.Id.ToString()), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == $"Customer with Id {_fix.In.Id} could not be found.");

        _fix.In.Id = actualId;
    }
}

public class UpdateCustomerHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public UpdateCustomerIn In { get; set; } = new();
    public Customer customerModel { get; set; } = new();
    public List<CustomerRoomName> CustomerRoomNames { get; set; } = new();
    public ISendEmail sendEmail;
    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        customerModel = customer;
        CustomerRoomNames = CustomerRoomNamesRepository.SeedMany(db, customer.Id, 1);

        In.Id = customer.Id;
        In.BusinessTypeId = customer.BusinessTypeId;
        In.BusinessName = customer.BusinessName;
        In.NoOfRooms = customer.NoOfRooms;
        In.TimeZone = customer.TimeZone;
        In.WhatsappCountry = customer.WhatsappCountry;
        In.WhatsappNumber = customer.WhatsappNumber;
        In.Cname = customer.Cname;
        In.ClientDoamin = customer.ClientDoamin;
        In.Email = customer.Email;
        In.Messenger = customer.Messenger;
        In.SmsTitle = customer.SmsTitle;
        In.ViberCountry = customer.ViberCountry;
        In.ViberNumber = customer.ViberNumber;
        In.TelegramCounty = customer.TelegramCounty;
        In.TelegramNumber = customer.TelegramNumber;
        In.PhoneCountry = customer.PhoneCountry;
        In.PhoneNumber = customer.PhoneNumber;
        In.BusinessStartTime = customer.BusinessStartTime;
        In.BusinessCloseTime = customer.BusinessCloseTime;
        In.DoNotDisturbGuestStartTime = customer.DoNotDisturbGuestStartTime;
        In.DoNotDisturbGuestEndTime = customer.DoNotDisturbGuestEndTime;
        In.StaffAlertsOffduty = customer.StaffAlertsOffduty;
        In.NoMessageToGuestWhileQuiteTime = customer.NoMessageToGuestWhileQuiteTime;
        In.IncomingTranslationLangage = customer.IncomingTranslationLangage;
        In.NoTranslateWords = customer.NoTranslateWords;
        In.IsActive = customer.IsActive;

        List<UpdateCustomerRoomNamesIn> customerRoomNames = new();

        foreach (var customerRoomName in CustomerRoomNames)
        {
            UpdateCustomerRoomNamesIn obj = new()
            {
                Id = customerRoomName.Id,
                Name = customerRoomName.Name,
                IsActive = customerRoomName.IsActive,
            };
            customerRoomNames.Add(obj);
        }

        In.UpdateCustomerRoomNamesIns = customerRoomNames;
    }

    public UpdateCustomerHandler BuildHandler(ApplicationDbContext db, ICommonDataBaseOprationService commonRepository) =>
        new(db, Response, commonRepository,sendEmail);
}
