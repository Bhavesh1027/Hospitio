using FakeItEasy;
using HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyFile;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandlePropertyGallery.Queries.GetPropertyFileHandlerTestFixture;

namespace HospitioApi.Test.HandlePropertyGallery.Queries
{
    public class GetPropertyFileHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;

        public GetPropertyFileHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _userFilesService = A.Fake<IUserFilesService>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var handler = _fix.BuildHandler(db, _userFilesService);
            var request = new GetPropertyFileRequest(_fix.In);

            A.CallTo(() => _userFilesService.GetFileAsync(A<string>._, A<CancellationToken>._))
                .Returns(new MemoryStream()); // Return a dummy MemoryStream for testing

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.HasResponse);
            Assert.True(response.Response!.Message == "File found successful.");
        }

        [Fact]
        public async Task Invalid_Request_Error()
        {
            var _userFilesService = A.Fake<IUserFilesService>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var handler = _fix.BuildHandler(db, _userFilesService);
            var actualCustomerPropertyInformationId = _fix.In.CustomerPropertyInformationId;
            _fix.In.CustomerPropertyInformationId = 0;
            var request = new GetPropertyFileRequest(_fix.In); // Invalid CustomerPropertyInformationId

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.HasFailure);
            Assert.True(response.Failure!.Message == "Invalid request.");
            _fix.In.CustomerPropertyInformationId = actualCustomerPropertyInformationId;
        }

        [Fact]
        public async Task Property_Not_Exist_Error()
        {
            var _userFilesService = A.Fake<IUserFilesService>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var handler = _fix.BuildHandler(db, _userFilesService);
            var actualId = _fix.In.Id;
            _fix.In.Id = 0;
            var request = new GetPropertyFileRequest(_fix.In); // Non-existing property ID

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.HasFailure);
            Assert.True(response.Failure!.Message == $"Given property Id not exists.");
            _fix.In.Id = actualId;
        }

        [Fact]
        public async Task No_File_Location_Found_For_Profile_Error()
        {
            var _userFilesService = A.Fake<IUserFilesService>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var handler = _fix.BuildHandler(db, _userFilesService);
            var request = new GetPropertyFileRequest(_fix.In);

            A.CallTo(() => _userFilesService.GetFileAsync(A<string>._, A<CancellationToken>._))
                .Returns(Task.FromResult<MemoryStream>(null)); // Return null to simulate no file location found

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.HasFailure);
            Assert.True(response.Failure!.Message == $"No file location found for Profile.");
        }

        [Fact]
        public async Task No_File_Location_Found_Error()
        {
            var _userFilesService = A.Fake<IUserFilesService>();
            _fix.In.CustomerPropertyInformationId = _fix.propertyInfoId;
            _fix.In.Id = _fix.propertyGalleryId;
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
   
            var handler = _fix.BuildHandler(db, _userFilesService);
            var request = new GetPropertyFileRequest(_fix.In);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.HasFailure);
            Assert.True(response.Failure!.Message == $"No file location found.");
        }
    }

    public class GetPropertyFileHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetPropertyFileIn In { get; set; } = new GetPropertyFileIn();
        public GetPropertyFileOut GetPropertyFileOut { get; set; } = null;
        public int propertyInfoId { get; set; } 
        public int propertyGalleryId { get; set; } 

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
            var propertyInfoSecond = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
            var propertyGallery = CustomerPropertyGalleryFactory.SeedSingle(db, propertyInfo.Id);
            var propertyGallerySecond = CustomerPropertyGalleryFactory.SeedSingle(db, propertyInfoSecond.Id,true);

            propertyInfoId = propertyInfoSecond.Id;
            propertyGalleryId = propertyGallerySecond.Id;
            In.Id = propertyGallery.Id;
            In.CustomerPropertyInformationId = propertyInfo.Id;

            #region
            /*GetPropertyFileOut = new()
            {
                MemoryStream = propertyGallery.Id,
                FileName = propertyGallery.FileName,
                ImageId = propertyGallery.ImageId,
                ContentType = propertyGallery.PropertyImage.ContentType,
            };*/
            #endregion
        }

        public GetPropertyFileHandler BuildHandler(ApplicationDbContext db, IUserFilesService userFilesService) =>
            new(Response, userFilesService,db);
    }
}
