using FakeItEasy;
using Microsoft.Extensions.Azure;
using Moq;
using HospitioApi.Core.HandleAdminStaffAlerts.Queries.GetAdminStaffAlerts;
using HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminStaffAlerts.Queries.GetAdminStaffAlertsHandlerFixure;

namespace HospitioApi.Test.HandleAdminStaffAlerts.Queries
{
    public class GetAdminStaffAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetAdminStaffAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<AdminStaffAlertsOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get admin staff alert successful.");

            var businessTypesOut = (GetAdminStaffAlertsOut)result.Response;
            Assert.NotNull(businessTypesOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }
    public class GetAdminStaffAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<AdminStaffAlertsOut> Out { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users = UserFactory.SeedSingle(db);
            var adminStaffAlert = AdminStaffAlertFactory.SeedMany(db, 4, users.Id);
            foreach (var alert in adminStaffAlert)
            {
                AdminStaffAlertsOut adminStaffAlertsOut = new();
                adminStaffAlertsOut.Id = alert.Id;
                adminStaffAlertsOut.Name = alert.Name;
                adminStaffAlertsOut.Platfrom = alert.Platfrom;
                adminStaffAlertsOut.PhoneCountry = alert.PhoneCountry;
                adminStaffAlertsOut.PhoneNumber = alert.PhoneNumber;
                adminStaffAlertsOut.WaitTimeInMintes = alert.WaitTimeInMintes;
                adminStaffAlertsOut.IsActive = alert.IsActive;
                adminStaffAlertsOut.Msg = alert.Msg;
                adminStaffAlertsOut.UserId = alert.UserId;
                Out.Add(adminStaffAlertsOut);
            }
        }
        public GetAdminStaffAlertsHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}
