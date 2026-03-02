using FakeItEasy;
using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Vonage.Numbers;
using Vonage.Request;
using Vonage;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleVonageSMS.Queries.GetCustomerOwnNumbersHandlerFixure;
using HospitioApi.Core.Services.Vonage;

namespace HospitioApi.Test.HandleVonageSMS.Queries
{
    public class GetCustomerOwnNumbersHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerOwnNumbersHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        //[Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            A.CallTo(() => vonageService.ListOwnedNumbers(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, SearchPattern.StartsWith)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumbersSearchResponse()
            {
                Count = 500,
                Numbers = new List<Number>()
            });

            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == $"Numbers  get successfully");

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
            Assert.True(result.Failure!.Message == $"Vonage Credential Not Available");
            _fix.In.CustomerId = actualId;
        }
    
    }
    public class GetCustomerOwnNumbersHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public GetCustomerOwnNumbersIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var vonageCred = VonageCredentialFactory.SeedSingle(db, customer.Id);

            In.CustomerId = customer.Id;
        }
        public GetCustomerOwnNumbersHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService) =>
            new(db, Response, vonageService);
    }
}
