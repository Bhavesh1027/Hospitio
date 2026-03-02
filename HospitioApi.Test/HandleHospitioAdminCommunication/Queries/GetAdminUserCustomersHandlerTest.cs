using FakeItEasy;
using HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersHandlerFixure;

namespace HospitioApi.Test.HandleHospitioAdminCommunication.Queries
{
    public class GetAdminUserCustomersHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetAdminUserCustomersHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAllJsonData<AdminUserCustomersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get admin user customers successfully.");

            var Out = (GetAdminUserCustomersOut)result.Response;
            Assert.NotNull(Out);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAllJsonData<AdminUserCustomersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.FakeOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }

    }
    public class GetAdminUserCustomersHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetAdminUserCustomersIn In { get; set; } = new();
        public List<AdminUserCustomersOut>? Out { get; set; }
        public List<AdminUserCustomersOut>? FakeOut { get; set; } = null;
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users  = UserFactory.SeedSingle(db);
            var customer  = CustomerFactory.SeedSingle(db);
            var customerUser = CustomerUserFactory.SeedSingle(db,customer.Id);

            Out = new List<AdminUserCustomersOut>();

            AdminUserCustomersOut adminUserCustomersOut = new AdminUserCustomersOut();
            adminUserCustomersOut.Id = customer.Id;
            adminUserCustomersOut.BusinessName = customer.BusinessName;
            adminUserCustomersOut.FirstName = null;
            adminUserCustomersOut.LastName = null;
            adminUserCustomersOut.Email = customer.Email;
            adminUserCustomersOut.Title = null;
            adminUserCustomersOut.PhoneCountry = customer.PhoneCountry;
            adminUserCustomersOut.PhoneNumber = customer.PhoneNumber;
            adminUserCustomersOut.UserType = "CustomerUser";
            adminUserCustomersOut.UserName = null;
           
            List<UserOuts> userOuts = new List<UserOuts>();
            UserOuts userOut = new UserOuts();
            userOut.Id = customerUser.Id;
            userOut.FirstName = customerUser.FirstName;
            userOut.LastName = customerUser.LastName;
            userOut.Email = customerUser.Email;
            userOut.ProfilePicture = customerUser.ProfilePicture;
            userOut.PhoneCountry = customerUser.PhoneCountry;
            userOut.PhoneNumber = customerUser.PhoneNumber;
            userOuts.Add(userOut);

            adminUserCustomersOut.UserOuts = userOuts;
            Out.Add(adminUserCustomersOut);
        }
        public GetAdminUserCustomersHandler BuildHandler(IDapperRepository _dapper) =>
          new(_dapper, Response);
    }
}
