using FakeItEasy;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhannceYourStayItemExtraHandlerFixure;

namespace HospitioApi.Test.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries
{
    public class GetCustomerGuestAppEnhannceYourStayItemExtraHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerGuestAppEnhannceYourStayItemExtraHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.responseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get guest app enhance your stay catefory items successful.");

            var CustomerGuestsOut = (GetCustomerGuestAppEnhannceYourStayItemExtraOut)result.Response;
            Assert.NotNull(CustomerGuestsOut);
           
        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _commonRepository = A.Fake<ICommonDataBaseOprationService>();
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _dapper.GetAll<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.FakeResponseOut);

            var result = await _fix.BuildHandler(_dapper).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Data not available");

        }
}
    public class GetCustomerGuestAppEnhannceYourStayItemExtraHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerGuestAppEnhannceYourStayItemExtraIn In { get; set; } = new();
        public List<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut> responseOut { get; set; } = new();
        public List<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut> FakeResponseOut { get; set; }  = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);
            var customerRoomName = CustomerRoomNamesRepository.SeedSingle(db, customer.Id);
            var customerGuestAppBuilder = CustomerGuestAppBuilderFactory.SeedSingle(db, customerRoomName.Id);
            var enhanceStayItem = CustomerGuestAppEnhanceYourStayItemFactory.SeedSingle(db, customerGuestAppBuilder.Id, customer.Id, null);
            var enhanceYourStayCategoryItemsExtra = CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory.SeedSingle(db, enhanceStayItem.Id);

            In.CustomerGuestAppEnhanceYourStayItemId = enhanceStayItem.Id;

            CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut customersGuestAppEnhanceYourStayCategoryItemsExtraOut = new();

            customersGuestAppEnhanceYourStayCategoryItemsExtraOut.Questions = enhanceYourStayCategoryItemsExtra.Questions;
            customersGuestAppEnhanceYourStayCategoryItemsExtraOut.QueType = enhanceYourStayCategoryItemsExtra.QueType;
            customersGuestAppEnhanceYourStayCategoryItemsExtraOut.OptionValues = enhanceYourStayCategoryItemsExtra.OptionValues;
            customersGuestAppEnhanceYourStayCategoryItemsExtraOut.Id = enhanceYourStayCategoryItemsExtra.Id;
            customersGuestAppEnhanceYourStayCategoryItemsExtraOut.IsActive = enhanceYourStayCategoryItemsExtra.IsActive;

            responseOut.Add(customersGuestAppEnhanceYourStayCategoryItemsExtraOut);

            CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut fakeObj = new();   
            FakeResponseOut.Add(fakeObj);
            FakeResponseOut.Remove(fakeObj);
        }
        public GetCustomerGuestAppEnhannceYourStayItemExtraHandler BuildHandler(IDapperRepository _dapper) =>
                new(_dapper, Response);
    }
}
