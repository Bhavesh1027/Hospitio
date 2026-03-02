using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminStaffAlerts.Commands
{
    public class UpdateAdminStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public UpdateAdminStaffAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Update admin staff alert successful.");
        }
        [Fact]
        public async Task Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            string actualName = _fix.In.Name;
            _fix.In.Name = _fix.SecondName;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The admin staff alert {_fix.In.Name} already exists.");
            _fix.In.Name = actualName;

        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            string actualName = _fix.In.Name;
            int actualId = _fix.In.Id;
            _fix.In.Name = "Unique Name";
            _fix.In.Id = 1003;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Admin staff alert with Id {_fix.In.Id} could not be found.");
            _fix.In.Name = actualName;
            _fix.In.Id = actualId;
        }
    }
    public class UpdateAdminStaffAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public UpdateAdminStaffAlertsIn In { get; set; } = new();
        public int SecondId { get; set; }
        public string? SecondName { get; set; }
        public IVonageService vonageService { get; set; } = A.Fake<IVonageService>();
        public IOptions<VonageSettingsOptions> vonageOptiopns { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var users = UserFactory.SeedSingle(db);
            var adminStaffAlert = AdminStaffAlertFactory.SeedSingle(db, users.Id);
            var adminStaffAlertSecond = AdminStaffAlertFactory.SeedSingle(db, users.Id, adminStaffAlert.Name);
            SecondId = adminStaffAlertSecond.Id;    
            SecondName = adminStaffAlertSecond.Name;

            In.Id = adminStaffAlert.Id;
            In.Name = "Updated Name";
            In.Platfrom = "3";
            In.PhoneCountry = "In";
            In.PhoneNumber = "1234567890";
            In.WaitTimeInMintes = 20;
            In.IsActive = true;
            In.Msg = "New Messages";
            In.UserId = adminStaffAlert.UserId;
        }
        public UpdateAdminStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
                new(db, Response, vonageService, vonageOptiopns);
    }
}
