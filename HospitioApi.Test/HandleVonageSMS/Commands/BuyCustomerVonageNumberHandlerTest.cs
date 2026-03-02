using Azure.Core;
using FakeItEasy;
using Humanizer;
using Moq;
using HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using Vonage;
using Vonage.Numbers;
using Vonage.Request;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleVonageSMS.Commands.BuyCustomerVonageNumberHandlerFixure;

namespace HospitioApi.Test.HandleVonageSMS.Commands
{
    public class BuyCustomerVonageNumberHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public BuyCustomerVonageNumberHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var vonageService = A.Fake<IVonageService>();

            A.CallTo(() => vonageService.BuyNumber(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumberTransactionResponse()
            {
                ErrorCode = "200"
            });

            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == $"Number Buy successfully");
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

            A.CallTo(() => vonageService.BuyNumber(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).WhenArgumentsMatch(x => x.Count() > 0).Returns(new NumberTransactionResponse()
            {
                ErrorCode = "300"
            });

            var result = await _fix.BuildHandler(db, vonageService).Handle(new(_fix.In), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == $"Data Not Available");
        }
    }
    public class BuyCustomerVonageNumberHandlerFixure : DbFixture
    {
        public BuyCustomerVonageNumberIn In { get; set; } = new();
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
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
        public BuyCustomerVonageNumberHandler BuildHandler(ApplicationDbContext db, IVonageService vonageService) =>
              new(db, Response, vonageService);
    }
}
