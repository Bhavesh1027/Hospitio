using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Moq;
using HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatId;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using HospitioApi.Test.EntityFactories;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatIdHandlerFixure;

namespace HospitioApi.Test.HandleCustomerUsers.Queries
{
    public class GetCustomerInfoByWidgetChatIdHandlerTest : IClassFixture<ThisTestFixure>
    {
        private readonly ThisTestFixure _fix;
        public GetCustomerInfoByWidgetChatIdHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        //[Fact]
        public async Task success()
        {
            using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);
            var mockBlobServiceClient = new Mock<BlobServiceClient>();
            var mockContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            // Configure BlobServiceClient to return a specific value for the GetBlobContainerClient method

            mockBlobServiceClient.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(mockContainerClient.Object);

            // Configure BlobContainerClient to return a specific value for the GetBlobClient method
            mockContainerClient.Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(mockBlobClient.Object);

            // Configure BlobClient to return a specific Uri for the GenerateSasUri method
            mockBlobClient.Setup(x => x.GenerateSasUri(It.IsAny<BlobSasPermissions>(), It.IsAny<DateTimeOffset>()))
                .Returns(new Uri("https://example.com/blob.jpg"));

            var result = await _fix.BuildHandler(db).Handle(new(_fix.In), CancellationToken.None);

            mockBlobServiceClient.Verify(x => x.GetBlobContainerClient(It.IsAny<string>()), Moq.Times.Once);
            mockContainerClient.Verify(x => x.GetBlobClient(It.IsAny<string>()), Moq.Times.Once);
            mockBlobClient.Verify(x => x.GenerateSasUri(It.IsAny<BlobSasPermissions>(), It.IsAny<DateTimeOffset>()), Moq.Times.Once);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get Customer successful.");
        }
    }
    public class GetCustomerInfoByWidgetChatIdHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public IOptions<HospitioApiStorageAccountOptions> fakeHospitioApiStorageAccount { get; set; } = A.Fake<IOptions<HospitioApiStorageAccountOptions>>();
        public IOptions<ChatWidgetLinksSettingsOptions> fakeChatWidgetLinksSettings { get; set; } = A.Fake<IOptions<ChatWidgetLinksSettingsOptions>>();
        public GetCustomerInfoByWidgetChatIdIn In { get; set; } = new();
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = customerFactory.SeedSingle(db);
            CustomerLevelFactory.SeedSingle(db, 1);
            var customerUsers = CustomerUserFactory.SeedSingle(db, customer.Id,1);
            CustomerGuestsCheckInFormBuildersFactory.SeedSingle(db, customer.Id);

            var encryptedText = CryptoExtension.Encrypt(customer.Id.ToString(), (UserTypeEnum.Customer).ToString());
            In.WidgetChatId = encryptedText;

            //fakeHospitioApiStorageAccount.Value.ConnectionStringKey = "DefaultEndpointsProtocol=https;AccountName=geastorage2023;AccountKey=RGFlgnWSNUit/xc0V9TgNZpX33/VGjM9SQGEIjTwg1c20xSlRi/76dxIz6b1rKsT+5bRRN7j9vJe+AStsVsGyg==;EndpointSuffix=core.windows.net";
            //fakeHospitioApiStorageAccount.Value.UserFilesContainerName = "hospitiodatastore";

        }
        public GetCustomerInfoByWidgetChatIdHandler BuildHandler(ApplicationDbContext db) => new(db, Response, fakeHospitioApiStorageAccount, fakeChatWidgetLinksSettings);
    }
}
