using FakeItEasy;
using HospitioApi.Core.HandleAdminCustomerAlerts.Queries.GetAdminCustomerAlerts;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleAdminCustomerAlerts.Queries.GetAdminCustomerAlertsHandlerFixure;


namespace HospitioApi.Test.HandleAdminCustomerAlerts.Queries
{
    public class GetAdminCustomerAlertsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetAdminCustomerAlertsHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<AdminCustomerAlertsOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.GetAdminCustomerAlertsOut);
            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);
            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get admin customer alerts successful.");

            var adminCustomerAlertsOut = (GetAdminCustomerAlertsOut)result.Response;
            Assert.NotNull(adminCustomerAlertsOut);
        }
        [Fact]
        public async Task Data_Not_Available_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();
            var actualData = _fix.GetAdminCustomerAlertsOut;
            _fix.GetAdminCustomerAlertsOut = null;
            A.CallTo(() => _dapper.GetAll<AdminCustomerAlertsOut>(A<string>.Ignored, null, CancellationToken.None,
                CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.GetAdminCustomerAlertsOut);
            var result = await _fix.BuildHandler(_dapper).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.GetAdminCustomerAlertsOut = actualData;
        }

    }
    public class GetAdminCustomerAlertsHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<AdminCustomerAlertsOut> GetAdminCustomerAlertsOut { get; set; }
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var adminCustomerAlerts = AdminCustomerAlertFactory.SeedMany(db, 3);
            List<AdminCustomerAlertsOut> adminCustomerAlertsOuts = new List<AdminCustomerAlertsOut>();
            GetAdminCustomerAlertsOut = adminCustomerAlertsOuts;
            foreach (var item in adminCustomerAlerts)
            {
                AdminCustomerAlertsOut Obj = new();
                Obj.Id = item.Id;
                Obj.MsgWaitTimeInMinutes = item.MsgWaitTimeInMinutes;
                Obj.Msg  = item.Msg;
                Obj.IsActive  = item.IsActive;
                adminCustomerAlertsOuts.Add(Obj);
            }
            GetAdminCustomerAlertsOut  = adminCustomerAlertsOuts;

        }
        public GetAdminCustomerAlertsHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);
    }
}
