using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandleTicket.Queries.GetTickets;
using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Test.EntityFactories;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleNotification.Commands.CreateNotificationsHandlerTestFixture;

namespace HospitioApi.Test.HandleNotification.Commands
{
    public class CreateNotificationsHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public CreateNotificationsHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            A.CallTo(() => _dapper.GetAll<UserNotification>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.userNotification);

            var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
            var _chatService = A.Fake<IChatService>();
            var _hubContext = A.Fake<IHubContext<ChatHub>>();

            var result = await _fix.BuildHandler(db, _hubContext, _commonRepository, _dapper, _chatService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create notification successful.");
        }
    }

    public class CreateNotificationsHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateNotificationsIn In { get; set; } = new CreateNotificationsIn();
        public List<UserNotification> userNotification { get; set; } = new List<UserNotification>();

        protected override void Seed()
        {

            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var businessType = BusinessTypeFactory.SeedSingle(db);
            var product = ProductFactory.SeedSingle(db);
            var customer = CustomerFactory.SeedSingle(db);
            var customerUser = CustomerUserFactory.SeedSingle(db, customer.Id);
            var customerReservation = CustomerReservationFactory.SeedSingle(db, customer.Id);

            In.Country = "Test";
            In.City = "Test";
            In.Postalcode = "Test Company";
            In.BusinessTypeId = businessType.Id;
            In.ProductId = product.Id;
            In.Title = "Test Title";
            In.Message = "Test Message";
            In.CurrentUserType = 1;



            UserNotification obj = new()
            {

                UserId = customer.Id,
                IsActive = 1
            };
            userNotification.Add(obj);

        }

        public CreateNotificationsHandler BuildHandler(ApplicationDbContext db, IHubContext<ChatHub> hubContext, ICommonDataBaseOprationService _commonRepository, IDapperRepository _dapper, IChatService _chatService) =>
            new(db, hubContext, Response, _commonRepository, _dapper, _chatService);
    }
}

