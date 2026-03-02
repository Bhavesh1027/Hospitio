using FakeItEasy;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlerts;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminStaffAlerts.Commands
{
    public class DeleteAdminStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public DeleteAdminStaffAlertsHandlerTest(ThisTestFixture fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete admin staff alert successful.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            int actualId = _fix.In.Id;
            _fix.In.Id = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Admin staff alert with {_fix.In.Id} not found.");
            _fix.In.Id = actualId;
        }
    }
    public class DeleteAdminStaffAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteAdminStaffAlertsIn In { get; set; } = new();
        public IVonageService vonageService { get; set; } = A.Fake<IVonageService>();
        public IOptions<VonageSettingsOptions> vonageOptiopns { get; set; } = A.Fake<IOptions<VonageSettingsOptions>>();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var users = UserFactory.SeedSingle(db);
            var adminStaffAlert = AdminStaffAlertFactory.SeedSingle(db, users.Id);
            In.Id = adminStaffAlert.Id;
        }
        public DeleteAdminStaffAlertsHandler BuildHandler(ApplicationDbContext db) =>
                new(db, Response, vonageService, vonageOptiopns);
    }
}
