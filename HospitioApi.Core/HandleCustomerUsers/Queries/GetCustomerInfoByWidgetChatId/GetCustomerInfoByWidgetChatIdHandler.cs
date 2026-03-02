using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerInfoByWidgetChatId;
public record GetCustomerInfoByWidgetChatIdRequest(GetCustomerInfoByWidgetChatIdIn In)
    : IRequest<AppHandlerResponse>;

public class GetCustomerInfoByWidgetChatIdHandler : IRequestHandler<GetCustomerInfoByWidgetChatIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
    private readonly ChatWidgetLinksSettingsOptions _frontEndLinksSettings;

    public GetCustomerInfoByWidgetChatIdHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount,
        IOptions<ChatWidgetLinksSettingsOptions> frontEndLinksSettings
        )
    {
        _db = db;
        _response = response;
        _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerInfoByWidgetChatIdRequest request, CancellationToken cancellationToken)
    {
        var CustomerId = CryptoExtension.Decrypt(request.In.WidgetChatId, (UserTypeEnum.Customer).ToString());

        var customer = await _db.Customers.Where(c => c.Id == int.Parse(CustomerId)).FirstOrDefaultAsync(cancellationToken);
        var CustomerGuestsCheckInFormBuilderData = await _db.CustomerGuestsCheckInFormBuilders.Where(s => s.CustomerId == int.Parse(CustomerId)).FirstOrDefaultAsync(cancellationToken);
        var CustomerUserId = await _db.CustomerUsers.Where(s => s.CustomerId == int.Parse(CustomerId) && s.CustomerLevelId == 1).FirstOrDefaultAsync(cancellationToken);

        if (CustomerGuestsCheckInFormBuilderData == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        Uri? temporaryUrl = null;
        if (CustomerGuestsCheckInFormBuilderData.Logo != null)
        {
            string storageConnectionString = _hospitioApiStorageAccount.ConnectionStringKey;
            string containerName = _hospitioApiStorageAccount.UserFilesContainerName;
            TimeSpan expirationTime = TimeSpan.FromDays(365.25 * 100); // Adjust as needed

            var blobServiceClient = new BlobServiceClient(storageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(CustomerGuestsCheckInFormBuilderData.Logo);

            // Generate a temporary URL with read access that expires
            temporaryUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationTime));
        }

        var customerInfo = new GetCustomerInfoByWidgetChatIdResponseOut
        {
            Logo = temporaryUrl != null ? temporaryUrl.ToString() : null,
            Color = CustomerGuestsCheckInFormBuilderData.Color != null ? CustomerGuestsCheckInFormBuilderData.Color : null,
            CustomerUserId = CustomerUserId.Id,
            ChatWidgetPortal = _frontEndLinksSettings.ChatWidget,
            Cname = customer?.Cname
        };

        return _response.Success(new GetCustomerInfoByWidgetChatIdOut("Get Customer successful.", customerInfo));
    }
}
