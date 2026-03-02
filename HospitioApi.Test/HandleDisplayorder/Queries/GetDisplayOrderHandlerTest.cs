using FakeItEasy;
using HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;
using HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleDisplayorder.Queries.GetDisplayOrderHandlerTestFixture;

namespace HospitioApi.Test.HandleDisplayorder.Queries
{
    public class GetDisplayOrderHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetDisplayOrderHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<DisplayOrderOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(_fix.DisplayOrderOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get successful.");

            var displayOrderOut = (GetDisplayOrderOut)result.Response;
            Assert.NotNull(displayOrderOut);
        }

        [Fact]
        public async Task NotFound_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var _dapper = A.Fake<IDapperRepository>();

            DisplayOrderOut? obj = null;
            A.CallTo(() => _dapper.GetSingle<DisplayOrderOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x != null).Returns(obj);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }

    public class GetDisplayOrderHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetDisplayOrderIn In { get; set; } = new GetDisplayOrderIn();
        public DisplayOrderOut DisplayOrderOut { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
            var displayOrder = ScreenDisplayOrderAndStatusesFactory.SeedSingle(db, propertyInfo.Id, 1);

            DisplayOrderOut = new()
            {
                Id = displayOrder.Id,
                ScreenName = displayOrder.ScreenName,
                JsonData = displayOrder.JsonData,
                RefrenceId = propertyInfo.Id
            };
        }

        public GetDisplayOrderHandler BuildHandler(IDapperRepository _dapper) =>
            new(_dapper, Response);
    }
}


