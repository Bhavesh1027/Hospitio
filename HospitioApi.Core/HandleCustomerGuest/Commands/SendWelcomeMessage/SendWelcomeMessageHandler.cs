using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendWelcomeMessage;
public record SendWelcomeMessageRequest(SendWelcomeMessageIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class SendWelcomeMessageHandler : IRequestHandler<SendWelcomeMessageRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly ISendEmail _mail;
    private readonly IVonageService _vonageService;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccountOptions;
    public SendWelcomeMessageHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, ISendEmail mail, IVonageService vonageService, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccountOptions)
    {
        _db = db;
        _response = response;
        _jwtSettings = jwtSettings.Value;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
        _mail = mail;
        _vonageService = vonageService;
        _hospitioApiStorageAccountOptions = hospitioApiStorageAccountOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(SendWelcomeMessageRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.In.GuestPortal))
            return _response.Error($"Guest portal token is missing.", AppStatusCodeError.Gone410);

        var welcomemessage = await _db.CustomerGuestsCheckInFormBuilders.Where(c => c.CustomerId == Int32.Parse(request.CustomerId)).Select(c => c.GuestWelcomeMessage).FirstOrDefaultAsync(cancellationToken);
        if (string.IsNullOrEmpty(welcomemessage))
            return _response.Error($"Add welcome message from online check-in.", AppStatusCodeError.Gone410);

        var customerGuest = await _db.CustomerGuests.Where(g => g.Id == request.In.GuestId).FirstOrDefaultAsync(cancellationToken);
        var customer = await _db.Customers.Where(g => g.Id == Int32.Parse(request.CustomerId)).FirstOrDefaultAsync(cancellationToken);
        var formBuilder = await _db.CustomerGuestsCheckInFormBuilders.Where(c => c.CustomerId == Int32.Parse(request.CustomerId)).FirstOrDefaultAsync(cancellationToken);

        string body = $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reservation Confirmation</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0; border-radius: 10px;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f2ddf2f2; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2); border-radius: 10px;'>
        <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0; mso-table-rspace: 0; background-color: #f8f8f8; border-radius: 10px;'>
            <tr>
                <td style='padding: 20px 0; text-align: center; background-color: #{formBuilder.Color}; border-radius: 10px;'>
                    <img src='{_hospitioApiStorageAccountOptions.BlobStorageBaseURL}{formBuilder.Logo}' width='100' height='100' alt='{customer.BusinessName} Logo' />
                </td>
            </tr>
        </table>

        <div style='background-color: #fff; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); border-radius: 10px; padding: 20px;'>
            <p style='color: #333;'>Dear {customerGuest.Firstname} {customerGuest.Lastname},</p>

            <p style='color: #333;'>{welcomemessage}</p>

  <p style='color: #333;'>Please click on the following link to access the guest app:</p>
            <div style='text-align: center;'>
              
                <a href='{request.In.GuestPortal}' style='background-color: #{formBuilder.Color}; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); display: inline-block;'>Access Guest App</a>
            </div>

            <p style='color: #333;'>With our guest app, you can access a range of services, view local recommendations, and make the most of your flexible stay with us.</p>

            <p style='color: #333;'>If you have any questions or concerns, please do not hesitate to contact us at <a href='{customer.Email}' style='color: #007bff; text-decoration: none;'>{customer.Email}</a>, and our team will be happy to assist you.</p>

            <p style='color: #333;'>Thank you for choosing {customer.BusinessName}. We look forward to welcoming you and making your stay exceptional.</p>

             <span style='color: #{formBuilder.Color};'>The {customer.BusinessName} Team</span></p>
        </div>
    </div>
</body>
</html>";

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = "Your Reservation Confirmation and Guest Portal Access Details";
        sendEmail.Addresslist = customerGuest.Email;
        sendEmail.IsHTML = true;
        sendEmail.Body = body;
        sendEmail.IsNoReply = true;

        await _mail.ExecuteAsync(sendEmail, cancellationToken);

        //send whatsapp text message to guest
        string message = @$"Dear {customerGuest.Firstname} {customerGuest.Lastname},{welcomemessage} With our guest app, you can access a range of services view local recommendations, and make the most of your flexible stay with us.If you have any questions or concerns, please do not hesitate to contact us at {customer.Email}, and our team will be happy to assist you.Thank you for choosing {customer.BusinessName}. We look forward to welcoming you and making your stay exceptional./nPlease click on the following link to access the guest app:{request.In.GuestPortal}";
        var customerVonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == customer.Id).FirstOrDefaultAsync(cancellationToken);

        if (customerVonageCred != null)
        {
            var response = await _vonageService.SendWhatsappTextMessage(customerVonageCred.AppId, customerVonageCred.AppPrivatKey, customer.WhatsappNumber, customerGuest.PhoneNumber, message);
        }

        var createdCustomerReservationOut = new SendWelcomeMessageGuestOut
        {
            GuestId = request.In.GuestId,
            GuestPortal = request.In.GuestPortal,
        };

        return _response.Success(new SendWelcomeMessageOut("Create customer reservation successful.", createdCustomerReservationOut));

    }
}
