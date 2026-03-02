using DocumentFormat.OpenXml.Drawing;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetail;
using HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;

using ThisTestFixure = HospitioApi.Test.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetailHandlerFixure;

namespace HospitioApi.Test.HandleCustomerReservation.Commands
{
    public class CreateCustomerAutoReservationWithGestDetailHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public CreateCustomerAutoReservationWithGestDetailHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var mock = new Mock<CreateCustomerAutoReservationWithGestDetailHandler>();

            A.CallTo(()=> _fix.sendEmail.ExecuteAsync(null,A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create customer reservation successful.");
        }
        //[Fact]
        //public async Task FailedToCall_Centurian_API()
        //{
        //    using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
        //    var mock = new Mock<CreateCustomerAutoReservationWithGestDetailHandler>();

        //    A.CallTo(() => _fix.sendEmail.ExecuteAsync(null, A<CancellationToken>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(true);

        //    var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

        //    Assert.True(result.HasFailure);
        //    Assert.True(result.Failure!.Message == "Failed to call Centurion API: {centurionApiResponse.Error}");
        //}
    }
    public class CreateCustomerAutoReservationWithGestDetailHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public IOptions<JwtSettingsOptions> jwtSettings { get; set; } = A.Fake<IOptions<JwtSettingsOptions>>();
        public IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings { get; set; } = A.Fake<IOptions<FrontEndLinksSettingsOptions>>();
        public ISendEmail sendEmail { get; set; } = A.Fake<ISendEmail>();
        public IHttpClientFactory _httpClient { get; set; } = A.Fake<IHttpClientFactory>();
        public IOptions<EndpointSettings> endPointSettings { get; set; } = A.Fake<IOptions<EndpointSettings>>();
        public CreateCustomerAutoReservationWithGestDetailIn In { get; set; } = new();
        public IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount { get; set; } = A.Fake<IOptions<HospitioApiStorageAccountOptions>>();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var Customer = CustomerFactory.SeedSingle(db);
            var roomNumbersecond = CustomerRoomNamesRepository.SeedSingle(db, Customer.Id);
            var roomNumber = CustomerRoomNamesRepository.SeedSingle(db, Customer.Id);

            var customerReservationSecond = CustomerReservationFactory.SeedSingle(db, Customer.Id);
            var customerReservation = CustomerReservationFactory.SeedSingle(db, Customer.Id);
            In.CustomerId = customerReservation.CustomerId;
            DateTime checkInDate = (DateTime)customerReservation.CheckinDate;
            checkInDate.AddDays(-1);
            In.CheckinDate = checkInDate;
            DateTime checkOutDate = (DateTime)customerReservation.CheckoutDate;
            checkOutDate.AddDays(1);
            In.CheckoutDate = checkOutDate;

            var customerGuestSecond = CustomerGuestFactory.SeedSingle(db, customerReservationSecond.Id, roomNumbersecond.Name);
            var customerGuest = CustomerGuestFactory.SeedSingle(db, customerReservation.Id, roomNumber.Name);

            In.RoomNumber = roomNumbersecond.Name;
            frontEndLinksSettings.Value.GuestPortal = "http://localhost:3002/";
            jwtSettings.Value.JwtPemPrivateKey = @"-----BEGIN PRIVATE KEY-----MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDcOxBI7jvTTxgqTJFCKx4TQEvB9nyoTBD8XpmIAvP/eCo1vOrfZYZ52DmZLNHg4QpLFvtCEIz8Q44O22qIpp62b8ZeeVweexVhpO+PO7G3ImI+MgUac/uqMWw1bNPmrOqmvnK4/dAOcW1bVSeuiPXCoSxkRgjzHi9y9rFLfJU/CmoTO175WZ/zFlnFCrxJwoAmBUPk00mHQK2PCJbedHKgIVQn028belfppLpqN5HUYRo29aquuaNjfFYIjrfQIDELErDkFdWZ1uLaVeFRZ26lHyqspZzbN3RsbERx+bmIQLcgc3bKiDk9FetOxiXPfBkyLxbF6IDTk3NWaVjjQAxJAgMBAAECggEAEYFltfuvKF4XyaT0bDyz00vqZFc3aW6XxbevSdm86LL13Uz6+RmU1YfpFtKH4ZwsY/ObspCmOZcVFPvpPUYhRNcgVCTJZxLZjD1pXV/yrF/rTI5P9t5T7sn8Pp2RP7tsRid1RFxzIUga19uuSnf7MPv8D1cRzGFl3CQydjxHkLnEFYSHQZYVqnHB7+tLhzpm4B7vmY6n8uIA/+B5M2P9Z5YFetKrFkOSGaO8Ng/kIauekeWxXly06zTW152TUJYBELVcBYvz0M18eP7y1exsLqsuthbiObV46+9sqgqRYn6TOz1T6sqniY+aQtr9Rhscklo/9N9WhDhkpFl4Jip4UQKBgQD3LqhjDrSdQi+j4/cVoWVufv1UP4rAsibPoCuB5YfSsQnfDT2Fj6QYV4vKvdZnocVlxrUnCFVe1mhL7zKT37meFBLbM1WDu9TF+f+LZEHSfqE+aK5pP5I+4VAV4F6rBHRQccT60bLc0GH1xEeNryeLgoAETWYnFx1fifnyePlRrwKBgQDkFka+Bin+iXwFA75d3kKPRJdhb4JAjMxbxtJqB0fs4RkLC6OUDtGUCVNKyyuny3yRxICRFF54tkWKPZLnrq5wCeHKA/eewS3zaLhNKzvqXUz5gRYMMwTwzkEsVZ+8sHzFVwyeCc8FmHMIv+ZxSFidiVhWMxbKhhCazkwO9ZTXhwKBgAnFt3SLHUrmVfnVxmv9gIQY0y0kgfjSUkR9IZs2FuOWijxeSqNgJW2s8GLolHRuad53N6w+YRmpwl/WKhq8ipscUg6GfggCQgw9sQOyyANpbDiKbPLOR5riz4a94yBBwdN4XABKkBa4ylasFuQcG6UhWKxS3woGmOuxCcezTrATAoGAbyVr062tSRw2Ezt/yL5GMQp7uj9ceZgi/ZYlcwWZRxVp3rgNPlj6R+lDbW8UFvBSA7Z98DS81JX9zR+0NrIozvAB1y+XuwToH3UoWnJ2//33RJ5i4A78mVvo5nHTJV/bbU6+F0UwXMmtRNY+tXVLuXj0Uw0STh6GeOmOpruFjIECgYAFaBUxopHj+174k/CSrVLG8tjpYV1HaQazdQu7egqD7yXZtdsmr/W8+QeLTROpa2m7zvuxH15Jq7sEMw+jCzrwO/3GPVgyqJCAN9pFiJPyVDFsLuYCmrYx5Avzf3ah//WjOPjK5Bcqswlg5tqBw5v+DlTl4QGARXJlIkC1itq1AQ==-----END PRIVATE KEY-----";
        }
        public CreateCustomerAutoReservationWithGestDetailHandler BuildHandler(ApplicationDbContext db) =>
                new(db, Response, jwtSettings, frontEndLinksSettings, sendEmail, _httpClient, endPointSettings, hospitioApiStorageAccount);
    }
}
