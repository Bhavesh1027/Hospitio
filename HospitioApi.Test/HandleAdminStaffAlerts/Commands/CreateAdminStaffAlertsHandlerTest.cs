using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.CreateAdminStaffAlerts;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminStaffAlerts.Commands.CreateAdminStaffAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminStaffAlerts.Commands
{
    public class CreateAdminStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public CreateAdminStaffAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            _fix.RemoveData(_fix.AdminStaffAlert);
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create admin staff alert successful.");
        }
        [Fact]
        public async Task AdminStaffAlert_Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            _fix.SeedData();
            string actualName = _fix.In.Name;
            _fix.In.Name = _fix.Name;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The admin staff alert {_fix.Name} already exists.");
            _fix.In.Name = actualName;
        }
        [Fact]
        public async Task Five_StaffAlert_Limit_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            _fix.SeedData();
            string actualName = _fix.In.Name;
            _fix.In.Name = "UniqueAdminStaffAlert";
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Not created,only 5 staff alert add.");
            _fix.In.Name = actualName;

        }
    }
    public class CreateAdminStaffAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateAdminStaffAlertsIn In { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public AdminStaffAlert AdminStaffAlert { get; set; } 
        public IVonageService vonageService { get; set; } = A.Fake<IVonageService>();
        public IOptions<VonageSettingsOptions> vonageOptiopns { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
        protected override void Seed()
        {

            In.PhoneNumber = "+6985745896589";
            In.Name = "Test Name";
            In.PhoneCountry = "In";
            In.Platfrom = "2";
            In.WaitTimeInMintes = 10;
            In.IsActive = true;
            In.Msg = "This is Test Message";
        }
        public void RemoveData(AdminStaffAlert adminStaffAlert)
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            if(adminStaffAlert != null)
            {
                AdminStaffAlertFactory.Remove(db, adminStaffAlert);
            }

        }
        public void SeedData()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users = UserFactory.SeedSingle(db);
            var adminStaffAlert = AdminStaffAlertFactory.SeedSingle(db, users.Id);
            var adminStaffAlerts = AdminStaffAlertFactory.SeedMany(db, 5, users.Id);
            Name = adminStaffAlert.Name;

            AdminStaffAlert = adminStaffAlert;

            In.UserId = users.Id;
        }
        public CreateAdminStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response, vonageService, vonageOptiopns);
    }
}
