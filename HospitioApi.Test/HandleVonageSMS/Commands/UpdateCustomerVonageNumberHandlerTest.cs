using FakeItEasy;
using HospitioApi.Core.HandleVonageSMS.Commands.UpdateCustomerVonageNumber;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using Vonage.Numbers;
using Vonage.Request;
using Vonage;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleVonageSMS.Commands.UpdateCustomerVonageNumberHandlerFixure;
using HospitioApi.Core.Services.Vonage;

namespace HospitioApi.Test.HandleVonageSMS.Commands
{
    public class UpdateCustomerVonageNumberHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public UpdateCustomerVonageNumberHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();
            A.CallTo(() => vonageService.UpdateNumber(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumberTransactionResponse()
            {
                ErrorCode = "200"
            });
            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == $"Number Update successfully");
        }
        [Fact]
        public async Task VonageCred_Not_Available_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            int actualId = _fix.In.CustomerId;
            _fix.In.CustomerId = 0;
            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Vonage Credential Not Available");
            _fix.In.CustomerId = actualId;
        }
        [Fact]
        public async Task Data_Not_Available_Error()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            A.CallTo(() => vonageService.UpdateNumber(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumberTransactionResponse()
            {
                ErrorCode = "500"
            });
            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data Not Available");
        }
    }
    public class UpdateCustomerVonageNumberHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public UpdateCustomerVonageNumberIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = CustomerFactory.SeedSingle(db);
            var vonageCred = VonageCredentialFactory.SeedSingle(db, customer.Id);

            In.CustomerId = customer.Id;
            In.Number = "+695874589645";
            In.Country = "US";
        }
        public UpdateCustomerVonageNumberHandler BuildHandler(ApplicationDbContext db,IVonageService vonageService) =>
              new(db, Response, vonageService);
    }
}
