using FakeItEasy;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using HospitioApi.Core.HandleVonageSMS.Commands.GetAvailableNumbers;
using HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Vonage;
using Vonage.Numbers;
using Vonage.Request;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleVonageSMS.Queries.GetAvailableNumbersHandlerFixure;

namespace HospitioApi.Test.HandleVonageSMS.Queries
{
    public class GetAvailableNumbersHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetAvailableNumbersHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            A.CallTo(() => vonageService.SearchNumbers(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, SearchPattern.StartsWith, 0,0)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumbersSearchResponse()
            {
                Count = 500,
                Numbers = new List<Number>()
            });

            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == $"Numbers get successfully");
        }
        [Fact]
        public async Task VonageCred_Not_Available_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            var actualId = _fix.In.CustomerId;
            _fix.In.CustomerId = 0;
            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Vonage Credential; Not Available");
            _fix.In.CustomerId = actualId;
        }
        [Fact]
        public async Task Data_Not_Available_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            A.CallTo(() => vonageService.SearchNumbers(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, SearchPattern.StartsWith, 0, 0)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumbersSearchResponse()
            {
                Count = 0,
                Numbers = null
            });

            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data Not Available");
        }
    }
    public class GetAvailableNumbersHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetAvailableNumbersIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var vonageCred = VonageCredentialFactory.SeedSingle(db, customer.Id);

            In.CustomerId = customer.Id;
            In.country = "US";
            In.pattern = "9";
            In.features = "SMS";
        }
        public GetAvailableNumbersHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService) =>
            new(db, Response, vonageService);
    }
}
