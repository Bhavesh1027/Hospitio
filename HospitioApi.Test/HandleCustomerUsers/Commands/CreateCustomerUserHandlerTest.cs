using HospitioApi.Core.HandleCustomerUsers.Commands.CreateCustomerUser;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Xunit;
using TheTestFixure = HospitioApi.Test.HandleCustomerUsers.Commands.CreateCustomerUserHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Commands
{
    public class CreateCustomerUserHandlerTest : IClassFixture<TheTestFixure>
    {
        private readonly TheTestFixure _fix;
        public CreateCustomerUserHandlerTest(TheTestFixure fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task Success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            var actualEmail = _fix.In.Email;
            _fix.In.Email = "testEmail@gmail.com";

            var actualUserName = _fix.In.UserName;
            _fix.In.UserName = "TestUserName";

           var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "User added successfully");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
            _fix.In.Email = actualEmail;
            _fix.In.UserName =  actualUserName;

        }
        [Fact]
        public async Task Department_Not_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var departmentId = _fix.In.DepartmentId;
            _fix.In.DepartmentId = 0;

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Department not exist.");

            _fix.In.DepartmentId= departmentId;
            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
        }
        [Fact]
        public async Task Group_Not_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var departmentId = _fix.In.GroupId;
            _fix.In.GroupId = 0;
            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Group not exist .");

            _fix.In.GroupId= departmentId;
            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
        }
        [Fact]
        public async Task DepartmentManager_Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.DeptManager;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Department manager of given department already exist.");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
        }
        [Fact]
        public async Task GroupLeader_Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.GroupLeader;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Group leader of given group already exist.");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
        }
        [Fact]
        public async Task CEO_Already_Exist_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.CEO;

            int actualId = (int)_fix.In.Id;
            _fix.In.Id = _fix.IdForCEO;

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);
            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "CEO already exist.");

            _fix.In.Id = actualId;
            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
        }
        [Fact]
        public async Task Email_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            //var actualEmail = _fix.In.Email;
            //_fix.In.Email = "testEmail@gmail.com";

            var actualUserName = _fix.In.UserName;
            _fix.In.UserName = "TestUserName";

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Email already exits.");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
            //_fix.In.Email = actualEmail;
            _fix.In.UserName =  actualUserName;

        }
        [Fact]
        public async Task UserName_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            var actualEmail = _fix.In.Email;
            _fix.In.Email = "testEmail@gmail.com";

            //var actualUserName = _fix.In.UserName;
            //_fix.In.UserName = "TestUserName";

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Username already exits.");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
            _fix.In.Email = actualEmail;
            //_fix.In.UserName = actualUserName;

        }
        [Fact]
        public async Task Email_or_UserName_Already_Exist()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var actualCustomerLevelId = _fix.In.CustomerUserLevelId;
            _fix.In.CustomerUserLevelId = (int)Shared.Enums.UserLevel.Staff;

            var actualUserName = _fix.In.UserName;
            _fix.In.UserName = _fix.In.Email;

            var actualEmail = _fix.In.Email;
            _fix.In.Email = "testEmailUnique@gmail.com";

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Email or username already exits.");

            _fix.In.CustomerUserLevelId = actualCustomerLevelId;
            _fix.In.Email = actualEmail;
            _fix.In.UserName = actualUserName;

        }
    }
    public class CreateCustomerUserHandlerFixure : DbFixture    
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateCustomerUserIn In { get; set; } = new();
        public int IdForCEO { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);

            var ceoLevels = CustomerLevelFactory.SeedSingle(db,2);
            var departmentManagerLevels = CustomerLevelFactory.SeedSingle(db,3);
            var GroupLeaderLevels = CustomerLevelFactory.SeedSingle(db,4);
            var staffLevels = CustomerLevelFactory.SeedSingle(db,5);


            var customerDepartment = CustomerDepartmentsFactory.SeedSingle(db, customer.Id);
            var departmnetManager = CustomerUserFactory.SeedSingle(db, customer.Id, departmentManagerLevels.Id, customerDepartment.Id);
            var customerGroup = customerGroupFactory.SeedSingle(db, customerDepartment.Id, customer.Id);
            var groupLeader = CustomerUserFactory.SeedSingle(db, customer.Id, GroupLeaderLevels.Id, customerDepartment.Id, customerGroup.Id);

            // CustomerUSer level >> staff
            var customerUserSatff = CustomerUserFactory.SeedSingle(db,customer.Id,staffLevels.Id,customerDepartment.Id,customerGroup.Id);

            // CustomerUser level >> ceo
            var customerUserCEO = CustomerUserFactory.SeedSingle(db, customer.Id, ceoLevels.Id, customerDepartment.Id, customerGroup.Id);

            In.Id = customerUserSatff.Id;
            In.FirstName = customerUserSatff.FirstName;
            In.LastName = customerUserSatff.LastName;
            In.Email = customerUserSatff.Email;
            In.Title = customerUserSatff.Title;
            In.ProfilePicture = customerUserSatff.ProfilePicture;
            In.PhoneNumber = customerUserSatff.PhoneNumber;
            In.PhoneCountry = customerUserSatff.PhoneCountry;
            In.DepartmentId = customerUserSatff.CustomerDepartmentId;
            In.GroupId = customerUserSatff.CustomerGroupId;
            In.SupervisorId = customerUserSatff.SupervisorId;
            In.CustomerUserLevelId = customerUserSatff.CustomerLevelId;
            In.UserName = customerUserSatff.UserName;
            In.Password = "StrongPassword";
            In.CustomerId = customerUserSatff.CustomerId;
            In.IsActive = true;

        }

        public CreateCustomerUserHandler BuildHandler(ApplicationDbContext db) =>
            new(db, Response);
    }
}
