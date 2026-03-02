using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;
using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vonage.Common.Monads;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminCustomerAlerts.Commands
{
    public class DeleteAdminCustomerAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public DeleteAdminCustomerAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete admin customer alerts successful.");

        }
        [Fact]
        public async Task AdminCustomerAlert_NotFound_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            int actualId = _fix.In.Id;
            _fix.In.Id = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);


            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Admin customer alerts could not be found.");
            _fix.In.Id = actualId;
        }
    }
    public class DeleteAdminCustomerAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteAdminCustomerAlertsIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var adminCustomerAlert = AdminCustomerAlertFactory.SeedSingle(db);
            In.Id = adminCustomerAlert.Id;

        }
        public DeleteAdminCustomerAlertsHandler BuildHandler(ApplicationDbContext db) =>
       new(db, Response);
    }
}
