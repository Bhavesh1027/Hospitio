using FakeItEasy;
using HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using System.Data;
using Xunit;
using static HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery.GetPropertyGalleryOut;
using ThisTestFixture = HospitioApi.Test.HandlePropertyGallery.Queries.GetPropertyGalleryHandlerTestFixture;

namespace HospitioApi.Test.HandlePropertyGallery.Queries
{
    public class GetPropertyGalleryHandlerTest : IClassFixture<ThisTestFixture>
    {
        private readonly ThisTestFixture _fix;
        public GetPropertyGalleryHandlerTest(ThisTestFixture fixture) => _fix = fixture;

        [Fact]
        public async Task Success()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            A.CallTo(() => _dapper.GetAll<PropertyGalleryOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.PropertyGalleryOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get property images successfully.");

            var propertyGalleryOut = (GetPropertyGalleryOut)result.Response;
            Assert.NotNull(propertyGalleryOut);
        }

        [Fact]
        public async Task Not_Found_Error()
        {
            var _dapper = A.Fake<IDapperRepository>();
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

            A.CallTo(() => _dapper.GetAll<PropertyGalleryOut>(A<string>.Ignored, null, CancellationToken.None, CommandType.StoredProcedure)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.FakePropertyGalleryOut);

            var result = await _fix.BuildHandler(_dapper, db).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data not available");
        }
    }

    public class GetPropertyGalleryHandlerTestFixture : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public List<PropertyGalleryOut> PropertyGalleryOut { get; set; } = new();
        public List<PropertyGalleryOut> FakePropertyGalleryOut { get; set; } = new();
        public GetPropertyGalleryIn In { get; set; } = new();

        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var guestAppBulder = CustomerGuestAppBuilderFactory.SeedSingle(db);
            var propertyInfo = customerProperyInformationFactory.SeedSingle(db, customer.Id, guestAppBulder.Id);
            var propertyGallery = CustomerPropertyGalleryFactory.SeedMany(db, propertyInfo.Id,1);

            In.CustomerPropertyInformationId = propertyInfo.Id;

            foreach (var gallery in propertyGallery)
            {
                PropertyGalleryOut obj = new()
                {
                    Id = gallery.Id,
                    PropertyImage = gallery.PropertyImage,
                    IsDeleted = false
                };
                PropertyGalleryOut.Add(obj);
            }
            foreach (var gallery in propertyGallery)
            {
                PropertyGalleryOut obj = new()
                {
                    Id = gallery.Id,
                    PropertyImage = gallery.PropertyImage,
                    IsDeleted = true
                };
                FakePropertyGalleryOut.Add(obj);
            }
        }

        public GetPropertyGalleryHandler BuildHandler(IDapperRepository _dapper, ApplicationDbContext db) =>
            new(_dapper, db, Response);
    }
}


