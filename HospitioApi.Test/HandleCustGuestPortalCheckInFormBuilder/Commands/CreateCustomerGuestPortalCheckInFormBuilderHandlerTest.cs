using Azure.Core;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using FakeItEasy;
using Jose;
using Microsoft.Extensions.Options;
using Moq;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilder;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Shared;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilderHandlerTestFixture;

namespace HospitioApi.Test.HandleCustGuestPortalCheckInFormBuilder.Commands;

public class CreateCustomerGuestPortalCheckInFormBuilderHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerGuestPortalCheckInFormBuilderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        
        var _mail = A.Fake<ISendEmail>();

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = "Your Reservation Confirmation and Guest Portal Access Details";
        sendEmail.Addresslist = _fix.In.Email;
        sendEmail.IsHTML = true;
        sendEmail.Body = "This is Test Body";
        sendEmail.IsNoReply = true;

        A.CallTo(()=> _mail.ExecuteAsync(sendEmail, A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);   

        var result = await _fix.BuildHandler(db, _mail).Handle(new CreateCustomerGuestPortalCheckInFormBuilderRequest(_fix.In), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Customer guest created successfully.");
    }


    [Fact]
    public async Task NullRequest_Error()
    {
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        var _jwtSettings = A.Fake<IOptions<JwtSettingsOptions>>();
        var _frontEndLinksSettings = A.Fake<IOptions<FrontEndLinksSettingsOptions>>();
        var _mail = A.Fake<ISendEmail>();

        var result = await _fix.BuildHandler(db,_mail).Handle(new(null), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Request can not be null.");
    }
}

public class CreateCustomerGuestPortalCheckInFormBuilderHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestPortalCheckInFormBuilderIn In { get; set; } = new CreateCustomerGuestPortalCheckInFormBuilderIn();
    public IOptions<JwtSettingsOptions> FakeJwtSettingsOptions { get; set; } = A.Fake<IOptions<JwtSettingsOptions>>();
    public IOptions<FrontEndLinksSettingsOptions> FakeFrontEndLinksSettings { get; set; } = A.Fake<IOptions<FrontEndLinksSettingsOptions>>();
    public IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount { get; set; } = A.Fake<IOptions<HospitioApiStorageAccountOptions>>();

    public int GuestId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);
        var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);
        var guest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id);

        In.CustomerReservationId = customerReservation.Id;
        In.CustomerId = customer.Id;
        GuestId = guest.Id;

        string JwtPemPrivateKey = @"-----BEGIN PRIVATE KEY-----MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDcOxBI7jvTTxgqTJFCKx4TQEvB9nyoTBD8XpmIAvP/eCo1vOrfZYZ52DmZLNHg4QpLFvtCEIz8Q44O22qIpp62b8ZeeVweexVhpO+PO7G3ImI+MgUac/uqMWw1bNPmrOqmvnK4/dAOcW1bVSeuiPXCoSxkRgjzHi9y9rFLfJU/CmoTO175WZ/zFlnFCrxJwoAmBUPk00mHQK2PCJbedHKgIVQn028belfppLpqN5HUYRo29aquuaNjfFYIjrfQIDELErDkFdWZ1uLaVeFRZ26lHyqspZzbN3RsbERx+bmIQLcgc3bKiDk9FetOxiXPfBkyLxbF6IDTk3NWaVjjQAxJAgMBAAECggEAEYFltfuvKF4XyaT0bDyz00vqZFc3aW6XxbevSdm86LL13Uz6+RmU1YfpFtKH4ZwsY/ObspCmOZcVFPvpPUYhRNcgVCTJZxLZjD1pXV/yrF/rTI5P9t5T7sn8Pp2RP7tsRid1RFxzIUga19uuSnf7MPv8D1cRzGFl3CQydjxHkLnEFYSHQZYVqnHB7+tLhzpm4B7vmY6n8uIA/+B5M2P9Z5YFetKrFkOSGaO8Ng/kIauekeWxXly06zTW152TUJYBELVcBYvz0M18eP7y1exsLqsuthbiObV46+9sqgqRYn6TOz1T6sqniY+aQtr9Rhscklo/9N9WhDhkpFl4Jip4UQKBgQD3LqhjDrSdQi+j4/cVoWVufv1UP4rAsibPoCuB5YfSsQnfDT2Fj6QYV4vKvdZnocVlxrUnCFVe1mhL7zKT37meFBLbM1WDu9TF+f+LZEHSfqE+aK5pP5I+4VAV4F6rBHRQccT60bLc0GH1xEeNryeLgoAETWYnFx1fifnyePlRrwKBgQDkFka+Bin+iXwFA75d3kKPRJdhb4JAjMxbxtJqB0fs4RkLC6OUDtGUCVNKyyuny3yRxICRFF54tkWKPZLnrq5wCeHKA/eewS3zaLhNKzvqXUz5gRYMMwTwzkEsVZ+8sHzFVwyeCc8FmHMIv+ZxSFidiVhWMxbKhhCazkwO9ZTXhwKBgAnFt3SLHUrmVfnVxmv9gIQY0y0kgfjSUkR9IZs2FuOWijxeSqNgJW2s8GLolHRuad53N6w+YRmpwl/WKhq8ipscUg6GfggCQgw9sQOyyANpbDiKbPLOR5riz4a94yBBwdN4XABKkBa4ylasFuQcG6UhWKxS3woGmOuxCcezTrATAoGAbyVr062tSRw2Ezt/yL5GMQp7uj9ceZgi/ZYlcwWZRxVp3rgNPlj6R+lDbW8UFvBSA7Z98DS81JX9zR+0NrIozvAB1y+XuwToH3UoWnJ2//33RJ5i4A78mVvo5nHTJV/bbU6+F0UwXMmtRNY+tXVLuXj0Uw0STh6GeOmOpruFjIECgYAFaBUxopHj+174k/CSrVLG8tjpYV1HaQazdQu7egqD7yXZtdsmr/W8+QeLTROpa2m7zvuxH15Jq7sEMw+jCzrwO/3GPVgyqJCAN9pFiJPyVDFsLuYCmrYx5Avzf3ah//WjOPjK5Bcqswlg5tqBw5v+DlTl4QGARXJlIkC1itq1AQ==-----END PRIVATE KEY-----";

           FakeJwtSettingsOptions.Value.JwtPemPrivateKey = JwtPemPrivateKey;

    }

    public CreateCustomerGuestPortalCheckInFormBuilderHandler BuildHandler(ApplicationDbContext db, ISendEmail mail) =>
        new(db, Response, FakeJwtSettingsOptions, FakeFrontEndLinksSettings, mail, hospitioApiStorageAccount);
}

