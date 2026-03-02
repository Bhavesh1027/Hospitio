using Azure.Core;
using HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleUserAccount.Commands.UpdateUserProfileHandlerFixure;

namespace HospitioApi.Test.HandleUserAccount.Commands
{
    public class UpdateUserProfileHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public UpdateUserProfileHandlerTest(ThisTestFixure fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == $"Update User Details successfully.");
        }
        [Fact]
        public async Task Email_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actulaEmail = _fix.In.Email;
            _fix.In.Email = _fix.Email;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The Email {_fix.In.Email} already exists.");
            _fix.In.Email = actulaEmail; 
        }
        [Fact]
        public async Task UserName_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actulaUserName = _fix.In.UserName;
            _fix.In.UserName = _fix.UserName;
            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"The UserName {_fix.In.UserName} already exists.");
             _fix.In.UserName = actulaUserName;
        }
    }
    public class UpdateUserProfileHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public UpdateUserProfileIn In { get; set; } = new();
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var users = UserFactory.SeedSingle(db);
            var usersSecond = UserFactory.SeedSingle(db);
            var hospitioonBoarding = HospitioOnboardingsFactory.SeedSingle(db);

            Email = usersSecond.Email;
            UserName = usersSecond.UserName;

            In.UserType = (int)UserTypeEnum.Hospitio;
            In.UserId = users.Id;
            In.UserName = "uniqueUserName";
            In.Email = "mail@gmail.com";
            In.FirstName = users.FirstName;
            In.LastName = users.LastName;
            In.Title = users.Title;
            In.ProfilePicture = users.ProfilePicture;
            In.PhoneNumber = users.PhoneNumber;
            In.Password = "StrongPassword";
        }
        public UpdateUserProfileHandler BuildHandler(ApplicationDbContext db) =>
       new(db, Response);
    }
}
