using FakeItEasy;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsersForDropdown;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleUserAccount.Queries.GetUsersForDropdownHandlerFixure;

namespace HospitioApi.Test.HandleUserAccount.Queries
{
    public class GetUsersForDropdownHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetUsersForDropdownHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            A.CallTo(() => _dapper.GetAll<AdminUsersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.userId), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get admin staff successful.");

            var userForDropDown = (GetUsersForDropdownOut)result.Response;
            Assert.NotNull(userForDropDown);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualObj = _fix.fakeResponseOut;
            _fix.fakeResponseOut = null;

            A.CallTo(() => _dapper.GetAll<AdminUsersOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(db, _dapper).Handle(new(_fix.userId), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
            _fix.fakeResponseOut = actualObj;
        }
    }
    public class GetUsersForDropdownHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public int userId { get; set; } = new();
        public List<AdminUsersOut> fakeResponseOut {get;set;} = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();


            var users = UserFactory.SeedSingle(db);
            userId = users.Id;

            AdminUsersOut adminUsersOut = new();
            adminUsersOut.Id = users.Id;
            adminUsersOut.Name = users.FirstName;
            adminUsersOut.PhoneCountry = users.PhoneCountry;
            adminUsersOut.PhoneNumber = users.PhoneNumber;

            fakeResponseOut.Add(adminUsersOut);


        }

        public GetUsersForDropdownHandler BuildHandler(ApplicationDbContext db, IDapperRepository dapper) =>
            new(dapper, Response, db);
    }
}
