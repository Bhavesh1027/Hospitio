using HospitioApi.Core.HandleCustomerUsers.Commands.CreateCustomerUser;
using HospitioApi.Core.HandleCustomerUsers.Commands.UpdateCustomerUserStatus;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerUsers.Commands.UpdateCustomerUserStatusHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Commands
{
    public class UpdateCustomerUserStatusHandlerTest : IClassFixture<UpdateCustomerUserStatusHandlerFixure>
    {
        private readonly ThisTestFixure _fix;
        public UpdateCustomerUserStatusHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Update CustomerUser status successfully.");
        }
        [Fact]
        public async Task CustomerUser_Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualId = _fix.In.CustomerUserId;
            _fix.In.CustomerUserId = 0;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Customer User not found.");
            _fix.In.CustomerUserId = actualId;
        }
    }
    public class UpdateCustomerUserStatusHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public UpdateCustomerUserStatusIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customerUser = CustomerUserFactory.SeedSingle(db); 
            In.CustomerUserId = customerUser.Id;
            In.IsActive = true;
        }
        public UpdateCustomerUserStatusHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}
