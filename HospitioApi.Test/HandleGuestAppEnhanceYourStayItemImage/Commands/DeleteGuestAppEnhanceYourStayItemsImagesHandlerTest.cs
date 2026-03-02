using FakeItEasy;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.DeleteGuestAppEnhanceYourStayItemsImages;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Commands.DeleteGuestAppEnhanceYourStayItemsImagesHandlerFixure;

namespace HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Commands
{
    public class DeleteGuestAppEnhanceYourStayItemsImagesHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public DeleteGuestAppEnhanceYourStayItemsImagesHandlerTest(ThisTestFixure fix)
        {
            _fix=fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var userFileService = A.Fake<IUserFilesService>();

            var result = await _fix.BuildHandler(db,userFileService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Delete article successfully.");
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var userFileService = A.Fake<IUserFilesService>();

            int actualId = _fix.In.CustomerGuestAppEnhanceYourStayItemId;
            _fix.In.CustomerGuestAppEnhanceYourStayItemId = 0;

            var result = await _fix.BuildHandler(db, userFileService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Enhance your stay item image article with CustomerGuestAppEnhanceYourStayItemId {_fix.In.CustomerGuestAppEnhanceYourStayItemId} not found or user doesn't have the rights to delete it");
            _fix.In.CustomerGuestAppEnhanceYourStayItemId = actualId;

        }
    }
    public class DeleteGuestAppEnhanceYourStayItemsImagesHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public DeleteGuestAppEnhanceYourStayItemsImagesIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var enhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerAppBuilder.Id, customer.Id);
            var enhanceYourStay = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerAppBuilder.Id, customer.Id, enhanceYourStayCategory.Id);
            var ItemImages = CustomerGuestAppEnhanceYourStayItemsImagesFactory.SeedSingle(db, enhanceYourStay.Id);

            In.CustomerGuestAppEnhanceYourStayItemId = enhanceYourStay.Id;
        }
        public DeleteGuestAppEnhanceYourStayItemsImagesHandler BuildHandler(ApplicationDbContext db, IUserFilesService userFilesService) => new(db, Response, userFilesService);

    }
}
