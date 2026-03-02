using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminCustomerAlerts.Commands
{
    public class CreateAdminCustomerAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public CreateAdminCustomerAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            _fix.RemoveData(_fix.adminCustomerAlerts);
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create admin customer alert successful.");
        }
        [Fact]
        public async Task Only_OneCustomer_AlertError_Check()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);


            _fix.seedData();
            string actualMsg = _fix.In.Msg;
            _fix.In.Msg = "UniqueAdminCustomerAlert";
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Not created,only 1 customer alert Add.");
            _fix.In.Msg = actualMsg;

        }
        [Fact]
        public async Task AlertMsg_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            string actualMsg = _fix.In.Msg;
            _fix.In.Msg = _fix.alertMsg;

            _fix.seedData();
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The admin customer alerts already exists.");
            _fix.In.Msg = actualMsg;
        }
    }
    public class CreateAdminCustomerAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateAdminCustomerAlertsIn In { get; set; } = new();
        public string alertMsg { get; set; } = string.Empty;
        public AdminCustomerAlert adminCustomerAlerts { get; set; }
        protected override void Seed()
        {
            In.Msg = "This is Test Alert Message";
            In.MsgWaitTimeInMinutes = 10;
            In.IsActive = true;
        }
        public void RemoveData(AdminCustomerAlert adminCustomerAlert)
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            if (adminCustomerAlert != null)
            {
                AdminCustomerAlertFactory.Remove(db, adminCustomerAlert);
            }
        }
        public void seedData()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var adminCustomerAlert = AdminCustomerAlertFactory.SeedSingle(db);
            alertMsg = adminCustomerAlert.Msg;
            adminCustomerAlerts = adminCustomerAlert;
        }
        public CreateAdminCustomerAlertsHandler BuildHandler(ApplicationDbContext db) =>
          new(db, Response);
    }


}
