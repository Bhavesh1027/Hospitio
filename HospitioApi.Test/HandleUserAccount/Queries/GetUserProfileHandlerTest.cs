using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserProfile;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleUserAccount.Queries.GetUserProfileHandlerFixure;

namespace HospitioApi.Test.HandleUserAccount.Queries
{
    public class GetUserProfileHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetUserProfileHandlerTest(ThisTestFixure fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetSingle<GetProfileOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get user profile successful.");

            var userProfileOut = (GetUserProfileOut)result.Response;
            Assert.NotNull(userProfileOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();

            var actualObj = _fix.fakeResponseOut;
            _fix.fakeResponseOut = null;
            A.CallTo(() => _dapper.GetSingle<GetProfileOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.fakeResponseOut = actualObj;
        }
    }
    public class GetUserProfileHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetUserProfileIn In { get; set; } = new();
        public GetProfileOut fakeResponseOut { get; set; } = new(); 
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users =  UserFactory.SeedSingle(db);

            fakeResponseOut.Id = users.Id;
            fakeResponseOut.FirstName = users.FirstName;
            fakeResponseOut.LastName = users.LastName;
            fakeResponseOut.Email = users.Email;
            fakeResponseOut.Title = users.Title;
            fakeResponseOut.ProfilePicture = users.ProfilePicture;
            fakeResponseOut.PhoneCountry = users.PhoneCountry;
            fakeResponseOut.PhoneNumber = users.PhoneNumber;
        
        }
            public GetUserProfileHandler BuildHandler(IDapperRepository dapper) =>
                    new(dapper, Response);
    }
}
