using FakeItEasy;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImage;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Text;
using Xunit;
using ThisTestFixture = HospitioApi.Test.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMSHandlerTestFixture;

namespace HospitioApi.Test.HandleCustomerGuestPMS.Commands;

public class CreateCustomerGuestPMSHandlerTest : IClassFixture<ThisTestFixture>
{
    private readonly ThisTestFixture _fix;

    public CreateCustomerGuestPMSHandlerTest(ThisTestFixture fixture) => _fix = fixture;

    [Fact]
    public async Task Success()
    {
        var _userFilesService = A.Fake<IUserFilesService>();
        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var webFile = new WebFileOut()
        {
            Name = "test.jpg",
            MemoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file data"))
        };
        _fix.In.DocumentAttachment = A.Fake<IFormFile>(); // Create a fake IFormFile
        A.CallTo(() => _userFilesService.UploadWebFileOnGivenPathAsync(A<IFormFile>._, A<string>._, A<CancellationToken>._ , false))
            .Returns(Task.FromResult(webFile));

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasResponse);
        Assert.True(result.Response!.Message == "Guest create successful.");
    }

    [Fact]
    public async Task UnableToUploadFile()
    {
        var _userFilesService = A.Fake<IUserFilesService>();
        A.CallTo(() => _userFilesService.UploadWebFileOnGivenPathAsync(A<IFormFile>._, A<string>._, A<CancellationToken>._, false))
            .Returns(Task.FromResult<WebFileOut>(null));

        using var db = new ApplicationDbContext(_fix.DbContextOptions, _fix.TenantService);

        var result = await _fix.BuildHandler(db, _userFilesService).Handle(new(_fix.In, _fix.CustomerId), CancellationToken.None);

        Assert.True(result.HasFailure);
        Assert.True(result.Failure!.Message == "Unable to uploaded documentattachment.");
        Assert.True(result.Failure!.StatusCodeError == AppStatusCodeError.InternalServerError500);
    }
}
public class CreateCustomerGuestPMSHandlerTestFixture : DbFixture
{
    public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
    public CreateCustomerGuestPMSIn In { get; set; } = new CreateCustomerGuestPMSIn();
    public IFormFile File { get; }
    public string CustomerId { get; set; }

    protected override void Seed()
    {
        using var db = new ApplicationDbContext(DbContextOptions, TenantService);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var customer = CustomerFactory.SeedSingle(db);

        CustomerId = customer.Id.ToString();

        In.DocumentAttachment = File;
        In.DocumentType = "jpg";
        In.ContainerName = "Test";
        In.Title = "Title";
        In.FirstName = "Test";
        In.LastName = "Test";
        In.Email = "Test@gmail.com";
        In.MobileNumber = "123456789";
        In.Street = "Test Street";
        In.PostalCode = "12345";
        In.City = "Test";
        In.Country = "Test";
        In.ReservationNumber = "12345";
        In.ArrivalDate = DateTime.Now;
        In.DepartureDate = DateTime.Now;
        In.VATNumber = "12345";
        In.DocumentName = "Test";

    }

    public CreateCustomerGuestPMSHandler BuildHandler(ApplicationDbContext db, IUserFilesService userFilesService) =>
        new(db, userFilesService, Response);
}
