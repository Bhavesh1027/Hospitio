using FakeItEasy;
using HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersDetail;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersDetailHandlerFixure;

namespace HospitioApi.Test.HandleHospitioAdminCommunication.Queries
{
    public class GetAdminUserCustomersDetailHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetAdminUserCustomersDetailHandlerTest(ThisTestFixture fixture) => _fix = fixture;
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GetAdminUserCustomersDetailResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Out);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get admin user customers detail successfully.");

            var Out = (GetAdminUserCustomersDetailOut)result.Response;
            Assert.NotNull(Out);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GetAdminUserCustomersDetailResponseOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.FakeOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }
    public class GetAdminUserCustomersDetailHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetAdminUserCustomersDetailIn In {get;set;} = new();
        public GetAdminUserCustomersDetailResponseOut Out { get;set;} = new();  
        public GetAdminUserCustomersDetailResponseOut FakeOut { get;set;} 
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users = UserFactory.SeedSingle(db);

            In.CustomerId = users.Id;
            In.UserType = "HospitioUser";

            Out = new();
            Out.UserId = users.Id;
            Out.BusinessName = null;
            Out.FirstName = users.FirstName;
            Out.LastName = users.LastName;
            Out.Email = users.Email;
            Out.ProfilePicture = users.ProfilePicture;
            Out.PhoneCountry = users.PhoneCountry;
            Out.PhoneNumber = users.PhoneNumber;
            Out.IncomingTranslationLangage = null;
            Out.NoOfRooms = null;
            Out.BizType = null;
            Out.ServicePackageName = null;
            Out.CreatedAt = users.CreatedAt;
            Out.UserType = "HospitioUser";


        }
         public GetAdminUserCustomersDetailHandler BuildHandler(IDapperRepository _dapper) =>
                   new(_dapper, Response);
    }
}
