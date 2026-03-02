using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Shared;
using SelectPdf;
using System.Net.Mail;
using System.Text;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendGuestPdfMail;
public record SendGuestPdfMailRequest(SendGuestPdfMailIn In) : IRequest<AppHandlerResponse>;


public class SendGuestPdfMailHandler : IRequestHandler<SendGuestPdfMailRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ISendEmail _mail;
    private readonly CustomerGuestCheckInFormSendPDftoEmailSettingsOptions _emailOptions;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
    public SendGuestPdfMailHandler(ApplicationDbContext db, IHandlerResponseFactory response, ISendEmail mail, IOptions<CustomerGuestCheckInFormSendPDftoEmailSettingsOptions> emailOptions, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount)
    {
        _db = db;
        _response = response;
        _mail = mail;
        _emailOptions = emailOptions.Value;
        _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
    }
    public async Task<AppHandlerResponse> Handle(SendGuestPdfMailRequest request, CancellationToken cancellationToken)
    {
        var guestInfo = await _db.CustomerGuests.Where(s => s.Id == int.Parse(request.In.GuestId)).FirstOrDefaultAsync(cancellationToken);
        var CustomerUsersInfo = await _db.CustomerUsers.Where(s => s.CustomerId == int.Parse(request.In.CustomerId) && s.CustomerLevelId == 1).FirstOrDefaultAsync(cancellationToken);
        var checkInBuliderInfo = await _db.CustomerGuestsCheckInFormBuilders.Where(s => s.CustomerId == int.Parse(request.In.CustomerId)).FirstOrDefaultAsync(cancellationToken);

        string guestPdf = request.In.HtmlContent;

        var pdfBytes = GeneratePdfFromHtmlAsync(guestPdf);

        byte[] documentByte = null;

        if (guestInfo.IdProof != null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                documentByte = await httpClient.GetByteArrayAsync(GetUrlByPath(guestInfo.IdProof));
            }
        }
        await SendEmailWithAttachment(pdfBytes,checkInBuliderInfo.SubmissionMail, _emailOptions, _mail, CancellationToken.None, documentByte != null ? documentByte : null);

        return _response.Success(new SendGuestPdfMailOut("Email send Successfylly"));
    }
    public async Task<bool> SendEmailWithAttachment(byte[] pdfBytes, string mail, CustomerGuestCheckInFormSendPDftoEmailSettingsOptions emailOptions, ISendEmail _mail, CancellationToken cancellationToken, byte[]? documentByte)
    {
        List<Attachment> attachementStringArr = new List<Attachment>();

        MemoryStream stream = new MemoryStream(pdfBytes);
        Attachment attachment = new Attachment(stream, "CheckInForm.pdf", "application/pdf");
        attachementStringArr.Add(attachment);

        if (documentByte != null)
        {
            MemoryStream stream1 = new MemoryStream(documentByte);
            Attachment attachment1 = new Attachment(stream1, "document.pdf", "application/pdf");
            attachementStringArr.Add(attachment1);
        }

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = emailOptions.Subject;
        sendEmail.Addresslist = mail;
        sendEmail.IsHTML = false;
        sendEmail.Body = "Hello, \nAn online check-in form has been submitted by a guest, and the corresponding PDF is attached. Please review it.  \nThanks.";
        sendEmail.IsNoReply = false;
        sendEmail.Attachments = attachementStringArr;

        var isSend = await _mail.ExecuteAsync(sendEmail, cancellationToken);
        return isSend;
    }

    public byte[] GeneratePdfFromHtmlAsync(string htmlContent)
    {
        HtmlToPdf htmlToPdf = new HtmlToPdf();
        PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(htmlContent);
        byte[] pdf = pdfDocument.Save();
        pdfDocument.Close();

        return pdf;
    }

    public string GetUrlByPath(string path)
    {
        string storageConnectionString = _hospitioApiStorageAccount.ConnectionStringKey;
        string containerName = _hospitioApiStorageAccount.UserFilesContainerName;
        TimeSpan expirationTime = TimeSpan.FromDays(365.25 * 100); // Adjust as needed

        var blobServiceClient = new BlobServiceClient(storageConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(path);

        // Generate a temporary URL with read access that expires
        var temporaryUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationTime));

        return temporaryUrl.ToString();
    }
}
