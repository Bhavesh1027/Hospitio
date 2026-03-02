using FakeItEasy;
using Moq;
using HospitioApi.Core.HandleLeads.Queries.GetLeads;
using HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using static HospitioApi.Core.HandleLeads.Queries.GetLeads.GetLeadsOut;
using ThisTestFixture = HospitioApi.Test.HandleNotification.Queries.GetNotificationsHandlerTestFixture;

namespace HospitioApi.Test.HandleNotification.Queries
{
    public class GetNotificationsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetNotificationsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            A.CallTo(() => _dapper.GetAll<NotificationOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.NotificationOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get notification successful.");

            var notificationOut = (GetNotificationsOut)result.Response;
            Assert.NotNull(notificationOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetNotificationsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<NotificationOut> NotificationOut { get; set; } = new();
        public GetNotificationsIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var businessType = BusinessTypeFactory.SeedSingle(db);
            var product = ProductFactory.SeedSingle(db);
            var notifications = NotificationFactory.SeedMany(db, businessType.Id, product.Id, 1);

            In.UserId = customer.Id;
            In.PageNo = 1;
            In.PageSize = 10;

            foreach (var notification in notifications)
            {
                NotificationOut obj = new()
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt
                };
                NotificationOut.Add(obj);
            }
        }

        public GetNotificationsHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}


