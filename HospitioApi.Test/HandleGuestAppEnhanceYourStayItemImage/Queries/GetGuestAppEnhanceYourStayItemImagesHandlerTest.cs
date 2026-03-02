using FakeItEasy;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImages;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImagesHandlerFixure;

namespace HospitioApi.Test.HandleGuestAppEnhanceYourStayItemImage.Queries
{
    public class GetGuestAppEnhanceYourStayItemImagesHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetGuestAppEnhanceYourStayItemImagesHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _dappar = A.Fake<IDapperRepository>();

            A.CallTo(() => _dappar.GetAll<GuestAppEnhanceYourStayItemImageOut>(A<string>.Ignored, null, CancellationToken.None,
                System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get enhance your stay item images successful.");

            var departmentOut = (GetGuestAppEnhanceYourStayItemImagesOut)result.Response;
            Assert.NotNull(departmentOut);
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _dappar = A.Fake<IDapperRepository>();

            A.CallTo(() => _dappar.GetAll<GuestAppEnhanceYourStayItemImageOut>(A<string>.Ignored, null, CancellationToken.None,
                System.Data.CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.fakeResponseOut);

            var result = await _fix.BuildHandler(_dappar).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");
        }
    }
    public class GetGuestAppEnhanceYourStayItemImagesHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<GuestAppEnhanceYourStayItemImageOut> responseOut { get; set; } = new();
        public List<GuestAppEnhanceYourStayItemImageOut> fakeResponseOut { get; set; } = new();
        public GetGuestAppEnhanceYourStayItemImagesIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var customerAppBuilder  = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var enhanceYourStayCategory = customerEnhanceYourStayCategoryFactory.SeedSingle(db, customerAppBuilder.Id,customer.Id);    
            var enhanceYourStay = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerAppBuilder.Id,customer.Id, enhanceYourStayCategory.Id);
            var ItemImages = CustomerGuestAppEnhanceYourStayItemsImagesFactory.SeedSingle(db, enhanceYourStay.Id);

            GuestAppEnhanceYourStayItemImageOut guestAppEnhanceYourStayItemImageOut = new();
            guestAppEnhanceYourStayItemImageOut.ItemsImages = ItemImages.ItemsImages;
            responseOut.Add(guestAppEnhanceYourStayItemImageOut);

            fakeResponseOut.Add(guestAppEnhanceYourStayItemImageOut);
            fakeResponseOut.Remove(guestAppEnhanceYourStayItemImageOut);
        }
        public GetGuestAppEnhanceYourStayItemImagesHandler BuildHandler(IDapperRepository _dapper) => new(_dapper, Response);

    }
}
