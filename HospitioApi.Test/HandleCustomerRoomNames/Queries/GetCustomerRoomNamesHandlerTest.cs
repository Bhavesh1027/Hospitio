using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using Moq;
using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerRoomNames.Queries.GetCustomerRoomNamesHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerRoomNames.Queries
{
    public class GetCustomerRoomNamesHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetCustomerRoomNamesHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomerAppBuilders>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.CustomerAppBuilders);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.CustomerId), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get customer room name successful.");

            var roomsOut = (GetCustomerRoomNamesOut)result.Response;
            Assert.NotNull(roomsOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            Mock<IDapperRepository> _dapper = new Mock<IDapperRepository>();

            var result = await _fix.BuildHandler(_dapper.Object).Handle(new(_fix.CustomerId), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetCustomerRoomNamesHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<CustomerAppBuilders> CustomerAppBuilders { get; set; } = new();
        private IHubContext<ChatHub> _hubContext { get; set; } = A.Fake<IHubContext<ChatHub>>();
        private IChatService _chatService { get; set; }

        public string CustomerId { get; set; }
        private ApplicationDbContext _db { get; set; } = A.Fake<ApplicationDbContext>();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var roomNames = CustomerRoomNamesRepository.SeedMany(db, customer.Id, 1);
            CustomerId = customer.Id.ToString();

            foreach (var room in roomNames)
            {
                CustomerAppBuilders obj = new()
                {
                    Id = room.Id,
                    Name = room.Name,
                    IsWork = 1,
                };
                CustomerAppBuilders.Add(obj);
            }
        }

        public GetCustomerRoomNamesHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response , _hubContext , _chatService , _db);
    }
}


