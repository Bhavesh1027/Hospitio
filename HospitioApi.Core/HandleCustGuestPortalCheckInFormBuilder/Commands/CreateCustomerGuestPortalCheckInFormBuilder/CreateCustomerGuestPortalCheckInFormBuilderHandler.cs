using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilder;
public record CreateCustomerGuestPortalCheckInFormBuilderRequest(CreateCustomerGuestPortalCheckInFormBuilderIn In)
: IRequest<AppHandlerResponse>;
public class CreateCustomerGuestPortalCheckInFormBuilderHandler : IRequestHandler<CreateCustomerGuestPortalCheckInFormBuilderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly ISendEmail _mail;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccountOptions;
    public CreateCustomerGuestPortalCheckInFormBuilderHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, ISendEmail mail,IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccountOptions)
    {
        _db = db;
        _response = response;
        _jwtSettings = jwtSettings.Value;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
        _mail = mail;
        _hospitioApiStorageAccountOptions = hospitioApiStorageAccountOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestPortalCheckInFormBuilderRequest request, CancellationToken cancellationToken)
    {
        if (request.In == null)
        {
            return _response.Error($"Request can not be null.", AppStatusCodeError.Forbidden403);
        }
        var In = request.In;
        var reqObjIn = new CustomerGuest();
        reqObjIn.CustomerReservationId = In.CustomerReservationId;
        reqObjIn.Firstname = In.Firstname;
        reqObjIn.Lastname = In.Lastname;
        reqObjIn.Email = In.Email;
        reqObjIn.Picture = In.Picture;
        reqObjIn.PhoneCountry = In.PhoneCountry;
        reqObjIn.PhoneNumber = In.PhoneNumber;
        reqObjIn.Country = In.Country;
        reqObjIn.Language = In.Language;
        reqObjIn.IdProof = In.IdProof;
        reqObjIn.IdProofType = In.IdProofType;
        reqObjIn.IdProofNumber = In.IdProofNumber;
        reqObjIn.BlePinCode = In.BlePinCode;
        reqObjIn.Pin = In.Pin;
        reqObjIn.Street = In.Street;
        reqObjIn.StreetNumber = In.StreetNumber;
        reqObjIn.City = In.City;
        reqObjIn.Postalcode = In.Postalcode;
        reqObjIn.ArrivalFlightNumber = In.ArrivalFlightNumber;
        reqObjIn.DepartureAirline = In.DepartureAirline;
        reqObjIn.DepartureFlightNumber = In.DepartureFlightNumber;
        reqObjIn.Signature = In.Signature;
        reqObjIn.RoomNumber = In.RoomNumber;
        reqObjIn.TermsAccepted = In.TermsAccepted;
        reqObjIn.FirstJourneyStep = In.FirstJourneyStep;
        reqObjIn.Rating = In.Rating;
        reqObjIn.DateOfBirth = In.DateOfBirth;
        reqObjIn.Vat = In.Vat;
        reqObjIn.AgeCategory = In.AgeCategory;
        reqObjIn.IsCoGuest = true;

        await _db.CustomerGuests.AddAsync(reqObjIn, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        CreatedCustomerGuestsCheckInFormBuilderOut outObj = new()
        {
            GuestId = reqObjIn.Id
        };

        var UId = CryptoExtension.Encrypt(outObj.GuestId.ToString(), (UserTypeEnum.Guest).ToString());

        string Link = _frontEndLinksSettings.GuestPortal + "?id=" + UId;
        //string Link = _frontEndLinksSettings.GuestPortal + "?token=" + Uri.EscapeDataString(GenerateToken(reqObjIn.CustomerReservationId, In.CustomerId.ToString(), outObj.GuestId));

        reqObjIn.GuestURL = UId;
        reqObjIn.GuestToken = GenerateToken(reqObjIn.CustomerReservationId, In.CustomerId.ToString(), outObj.GuestId);
        await _db.SaveChangesAsync(cancellationToken);

        var customerDetail = await _db.Customers.Where(e => e.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        var customerCheckInFormBuilder = await _db.CustomerGuestsCheckInFormBuilders.Where(x => x.CustomerId == customerDetail.Id).FirstOrDefaultAsync(cancellationToken);

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
                <td style='padding: 20px 0; text-align: center; background-color: #{customerCheckInFormBuilder.Color}; border-radius: 10px;'>
                    <img src='{_hospitioApiStorageAccountOptions.BlobStorageBaseURL}{customerCheckInFormBuilder.Logo}' width='100' height='100' alt='{customerDetail.BusinessName} Logo' />
                </td>
            </tr>
        </table>

        <div style='background-color: #fff; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); border-radius: 10px; padding: 20px;'>
            <p style='color: #333;'>Dear {reqObjIn.Firstname} {reqObjIn.Lastname},</p>

            <p style='color: #333;'>We are pleased to inform you that your reservation has been successfully confirmed. To enhance your stay and ensure a memorable experience, we invite you to access our guest app.</p>

  <p style='color: #333;'>Please click on the following link to access the guest app:</p>
            <div style='text-align: center;'>
              
                <a href='{Link}' style='background-color: #{customerCheckInFormBuilder.Color}; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); display: inline-block;'>Access Guest App</a>
            </div>

            <p style='color: #333;'>With our guest app, you can access a range of services, view local recommendations, and make the most of your flexible stay with us.</p>

            <p style='color: #333;'>If you have any questions or concerns, please do not hesitate to contact us at <a href='{customerDetail.Email}' style='color: #007bff; text-decoration: none;'>{customerDetail.Email}</a>, and our team will be happy to assist you.</p>

            <p style='color: #333;'>Thank you for choosing {customerDetail.BusinessName}. We look forward to welcoming you and making your stay exceptional.</p>

             <span style='color: #{customerCheckInFormBuilder.Color};'>The {customerDetail.BusinessName} Team</span></p>
        </div>
    </div>
</body>
</html>";


        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = "Your Reservation Confirmation and Guest Portal Access Details";
        sendEmail.Addresslist = request.In.Email;
        sendEmail.IsHTML = true;
        sendEmail.Body = body;
        sendEmail.IsNoReply = true;

        await _mail.ExecuteAsync(sendEmail, cancellationToken);

        return _response.Success(new CreateCustomerGuestPortalCheckInFormBuilderOut("Customer guest created successfully.", outObj));
    }
    public string GenerateToken(int? reservationId, string customerId, int customerGuestId)
    {
        var utcNow = DateTime.UtcNow;
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var IssuedAt = new DateTimeUtcUnixEpoch(utcNow);

        var claims = new List<Claim>
        {
             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Iat, IssuedAt.UnixEpoch.ToString(), ClaimValueTypes.Integer64),
             new("ReservationId", reservationId.ToString(),ClaimValueTypes.Integer64),
             new("GuestId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("CustomerId", customerId.ToString(),ClaimValueTypes.Integer64),
             new("UserId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("UserType", ((int)UserTypeEnum.Guest).ToString(),ClaimValueTypes.String)
        };

        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
         issuer: _jwtSettings.Issuer,
         audience: _jwtSettings.Audience,
         subject: new ClaimsIdentity(claims),
         notBefore: utcNow,
         expires: utcNow.Add(TimeSpan.FromDays(30)),
         issuedAt: utcNow,
         signingCredentials: signingCredentials
         );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
