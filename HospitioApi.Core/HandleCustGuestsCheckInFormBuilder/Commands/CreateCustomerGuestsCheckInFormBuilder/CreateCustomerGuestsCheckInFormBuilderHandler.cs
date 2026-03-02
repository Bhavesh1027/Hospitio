using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Dynamic;
using System.Text.Json;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder;
public record CreateCustomerGuestsCheckInFormBuilderRequest(CreateCustomerGuestsCheckInFormBuilderIn In)
: IRequest<AppHandlerResponse>;

public class CreateCustomerGuestsCheckInFormBuilderHandler : IRequestHandler<CreateCustomerGuestsCheckInFormBuilderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
    private readonly ICommonDataBaseOprationService _common;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    private readonly IUserFilesService _userFilesService;
    public CreateCustomerGuestsCheckInFormBuilderHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        ICommonDataBaseOprationService common,
        IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount,
        IUserFilesService userFilesService,
        IHubContext<ChatHub> hubContext,
        IChatService chatService)
    {
        _db = db;
        _response = response;
        _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
        _common = common;
        _userFilesService = userFilesService;
        _hubContext = hubContext;
        _chatService = chatService;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestsCheckInFormBuilderRequest request, CancellationToken cancellationToken)
    {
        var customer = await _db.CustomerGuestsCheckInFormBuilders.Include(i => i.Customer).Where(x => x.CustomerId == request.In.CustomerId && x.IsActive == true).FirstOrDefaultAsync(cancellationToken);
        var customerBusinessName= await _db.Customers.Where(i => i.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (customer != null)
        {
            return _response.Error($"{customer.Customer?.BusinessName} is already exists in customer guest check in form builder.", AppStatusCodeError.Forbidden403);
        }
        var In = request.In;

        Uri? temporaryUrl = null;
        string json = "";
        if (In.Logo != null)
        {
            string storageConnectionString = _hospitioApiStorageAccount.ConnectionStringKey;
            string containerName = _hospitioApiStorageAccount.UserFilesContainerName;
            TimeSpan expirationTime = TimeSpan.FromDays(365.25 * 100); // Adjust as needed

            var blobServiceClient = new BlobServiceClient(storageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(In.Logo);

            // Generate a temporary URL with read access that expires
            temporaryUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationTime));

            int[] size = new int[] { 72, 96, 128, 144, 152, 192, 256, 384, 512 };
            // int[] size = new int[] {  152, 256, 512 };
            List<dynamic> dynamicList = new List<dynamic>();
            GuestsCheckInFormBuilderJsonOut jsonObj = new GuestsCheckInFormBuilderJsonOut
            {
                short_name = customerBusinessName.BusinessName,
                name = customerBusinessName.BusinessName, 
                scope = ".",
                start_url = "/",
                display = "standalone",
                icons = dynamicList,
                theme_color = "#000000",
                background_color = "#ffffff"

            };

            dynamic icon1 = new ExpandoObject();

            icon1.src = temporaryUrl.ToString();
            icon1.type = "image/x-icon";
            dynamicList.Add(icon1);


            foreach (var item in size)
            {
                dynamic Icon = new ExpandoObject();

                Icon.src = await _common.ResizeImageFromUrlAsync(temporaryUrl.ToString(), item, item, _userFilesService);
                Icon.type = "image/png";
                Icon.sizes = $"{item}x{item}";


                if (item == 192 || item == 512)
                {
                    Icon.purpose = "maskable";
                }

                dynamicList.Add(Icon);
            }

            json = JsonSerializer.Serialize(jsonObj);
            //byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            //// Create a MemoryStream from the byte array
            //using (var jsonStream = new MemoryStream(jsonBytes))
            //{
            //    string jsonFileName = "imagesize.json";
            //    string documentName = ((UploadDocumentType)5).ToString();
            //    // Upload the JSON file to your Blob storage using the existing method
            //    var webFile = await _userFilesService.UploadWebFileOnGivenPathAsync(new FormFile(jsonStream, 0, jsonBytes.Length, jsonFileName, jsonFileName), documentName, CancellationToken.None , false);

            //    var URL = webFile.TempSasUri;
            //    var Location = webFile.Location;
            //}
        }

        var reqObjIn = new CustomerGuestsCheckInFormBuilder();
        reqObjIn.CustomerId = In.CustomerId;
        reqObjIn.Color = In.Color;
        reqObjIn.Name = In.Name;
        reqObjIn.Stars = In.Stars;
        reqObjIn.Logo = In.Logo;
        reqObjIn.AppImage = In.AppImage;
        reqObjIn.SplashScreen = In.SplashScreen;
        reqObjIn.IsOnlineCheckInFormEnable = In.IsOnlineCheckInFormEnable;
        reqObjIn.IsRedirectToGuestAppEnable = In.IsRedirectToGuestAppEnable;
        reqObjIn.SubmissionMail = In.SubmissionMail;
        reqObjIn.TermsLink = In.TermsLink;
        reqObjIn.IsActive = In.IsActive;
        reqObjIn.GuestWelcomeMessage = In.GuestWelcomeMessage;
        reqObjIn.JsonData = json;

        await _db.CustomerGuestsCheckInFormBuilders.AddAsync(reqObjIn, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        if (In.CustomerGuestsCheckInFormFieldIn != null)
        {
            List<CustomerGuestsCheckInFormField> CustomerGuestsCheckInFormField = new();
            foreach (var nestIn in In.CustomerGuestsCheckInFormFieldIn)
            {
                CustomerGuestsCheckInFormField obj = new()
                {
                    CustomerId = In.CustomerId,
                    CustomerGuestsCheckInFormBuilderId = reqObjIn.Id,
                    Name = nestIn.Name,
                    FieldType = nestIn.FieldType,
                    RequiredFields = nestIn.RequiredFields,
                    DisplayOrder = nestIn.DisplayOrder,
                };
                CustomerGuestsCheckInFormField.Add(obj);
            }
            await _db.CustomerGuestsCheckInFormFields.AddRangeAsync(CustomerGuestsCheckInFormField, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        CreatedCustomerGuestsCheckInFormBuilderOut outObj = new()
        {
            Id = reqObjIn.Id
        };

        var guestPoratalCustomerUsersList = await (from cu in _db.CustomerUsers join cup in _db.CustomerUsersPermissions on cu.Id equals cup.CustomerUserId into cups from cup in cups.DefaultIfEmpty() join cp in _db.CustomerPermissions on cup.CustomerPermissionId equals cp.Id into cps from cp in cps.DefaultIfEmpty() where cu.CustomerId == request.In.CustomerId && cu.DeletedAt == null && cu.IsActive == true && (cu.CustomerLevelId == 1 || (cup != null && (cup.IsView == true || cup.IsEdit == true) && cp.NormalizedName == "GuestPortal")) select cu.Id).ToListAsync(cancellationToken);

        if (guestPoratalCustomerUsersList != null || guestPoratalCustomerUsersList.Count != 0)
        {
            var guestProtalStatus = _chatService.GetCustomerOnboardingStatus(_db, request.In.CustomerId.ToString());
            foreach (var user in guestPoratalCustomerUsersList)
            {
                await _hubContext.Clients.Group($"user-2-{user}").SendAsync("GetCustomerOnboardingStatus", guestProtalStatus.Result);
            }
        }

        return _response.Success(new CreateCustomerGuestsCheckInFormBuilderOut("Customer guests checkIn form builder created successfully.", outObj));
    }
}

