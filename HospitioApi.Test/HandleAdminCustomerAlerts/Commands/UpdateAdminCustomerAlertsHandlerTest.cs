using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;
using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminCustomerAlerts.Commands
{
    public class UpdateAdminCustomerAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public UpdateAdminCustomerAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Update admin customer alerts successful.");
        }
        [Fact]
        public async Task AdminCustomerAlert_Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            int actualId = _fix.In.Id;
            string actualMsg = _fix.In.Msg;
            _fix.In.Id = _fix.FakeId;
            _fix.In.Msg = _fix.FakeMSg;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "The admin customer alerts already exists.");
            _fix.In.Id = actualId;
            _fix.In.Msg = actualMsg;
        }
        [Fact]
        public async Task AdminCustomerAlert_Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            int actualId = _fix.In.Id;
            string actualMsg = _fix.In.Msg;
            _fix.In.Msg = "This is Unique MEssage For Test";
            _fix.In.Id = 1002;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "The admin customer alert could not be found.");

            _fix.In.Id = actualId;
            _fix.In.Msg = actualMsg;
        }
    }
    public class UpdateAdminCustomerAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public UpdateAdminCustomerAlertsIn In { get; set; } = new();
        public int FakeId { get; set; }
        public string FakeMSg { get; set; } = string.Empty;

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var adminCustomerAlert = AdminCustomerAlertFactory.SeedSingle(db);
            var adminCustomerAlertSecond = AdminCustomerAlertFactory.SeedSingle(db, adminCustomerAlert.Msg);

            FakeId = adminCustomerAlertSecond.Id;
            FakeMSg = adminCustomerAlertSecond.Msg;

            In.Id = adminCustomerAlert.Id;
            In.Msg = "Modify Test Msg";
            In.MsgWaitTimeInMinutes = 20;
            In.IsActive = true;
        }
        public UpdateAdminCustomerAlertsHandler BuildHandler(ApplicationDbContext db) =>
                new(db, Response);
    }
}
