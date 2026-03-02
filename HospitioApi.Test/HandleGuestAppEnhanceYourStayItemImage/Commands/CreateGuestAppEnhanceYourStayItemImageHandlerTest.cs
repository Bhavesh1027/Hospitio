using FakeItEasy;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemImageHandlerFixure;

namespace HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Commands
{
    public class CreateGuestAppEnhanceYourStayItemImageHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public CreateGuestAppEnhanceYourStayItemImageHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Create guest app enhance your stay item images successful.");
        }
    }
    public class CreateGuestAppEnhanceYourStayItemImageHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public CreateGuestAppEnhanceYourStayItemImageIn In { get; set; } = new();
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

            GuestAppEnhanceYourStayItemAttachementIn guestAppEnhanceYourStayItemAttachementIn = new();
            guestAppEnhanceYourStayItemAttachementIn.ItemsImage = ItemImages.ItemsImages;
            guestAppEnhanceYourStayItemAttachementIn.DisplayOrder = ItemImages.DisaplayOrder;
            List<GuestAppEnhanceYourStayItemAttachementIn> guestAppEnhanceYourStayItemAttachementIns = new();
            guestAppEnhanceYourStayItemAttachementIns.Add(guestAppEnhanceYourStayItemAttachementIn);
            In.ItemsImages = guestAppEnhanceYourStayItemAttachementIns;

        }
        public CreateGuestAppEnhanceYourStayItemImageHandler BuildHandler(ApplicationDbContext db) => new(db, Response);

    }
}
