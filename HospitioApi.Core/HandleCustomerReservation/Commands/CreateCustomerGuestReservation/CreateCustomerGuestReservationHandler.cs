using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerGuestReservation;
public record CreateCustomerGuestReservationRequest(CreateCustomerGuestReservationIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerGuestReservationHandler : IRequestHandler<CreateCustomerGuestReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly ISendEmail _mail;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccountOptions;
    public CreateCustomerGuestReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response, IDapperRepository dapper, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, ISendEmail sendEmail, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccountOptions)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
        _jwtSettings = jwtSettings.Value;
        _mail = sendEmail;
        _hospitioApiStorageAccountOptions = hospitioApiStorageAccountOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestReservationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("GuId", request.In.UserCode, DbType.Guid);
        var customer = await _dapper.GetSingle<CustomerByGuIdOut>("[dbo].[GetCustomerByGuId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);
        if (customer == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var checkExist = await _db.CustomerReservations.Where(e => e.ReservationNumber == request.In.ReservationNumber).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer reservation already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        string format = "dd/MM/yyyy HH:mm";
        string combinedCheckInDateTime = $"{request.In.ArrivalDate} {request.In.ArrivalTime}";
        string combinedCheckOutDateTime = $"{request.In.DepartureDate} {request.In.DepartureTime}";
        DateTime? CheckinDate = null;
        DateTime? CheckoutDate = null;
        if (DateTime.TryParseExact(combinedCheckInDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalDateTime))
        {
            CheckinDate = arrivalDateTime;
        }
        if (DateTime.TryParseExact(combinedCheckOutDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime))
        {
            CheckoutDate = departureDateTime;
        }
        var customerReservation = new CustomerReservation
        {
            CustomerId = customer.Id,
            ReservationNumber = request.In.ReservationNumber,
            CheckinDate = CheckinDate,
            CheckoutDate = CheckoutDate,
            IsActive = true,
        };
        await _db.CustomerReservations.AddAsync(customerReservation);
        await _db.SaveChangesAsync(cancellationToken);
        var createdCustomerReservationOut = new CreatedCustomerReservationOut
        {
            Id = customerReservation.Id,
            CustomerId = customerReservation.CustomerId,
            ReservationNumber = customerReservation.ReservationNumber,
            CheckinDate = customerReservation.CheckinDate,
            CheckoutDate = customerReservation.CheckoutDate,
            NoOfGuestAdults = customerReservation.NoOfGuestAdults,
            NoOfGuestChilderns = customerReservation.NoOfGuestChildrens,
            Uuid = customerReservation.Uuid,
            Source = customerReservation.Source,
            IsActive = customerReservation.IsActive
        };

        var checkGuestExist = await _db.CustomerGuests.Where(e => e.CustomerReservationId == createdCustomerReservationOut.Id).FirstOrDefaultAsync(cancellationToken);
        var customerRoom = await _db.CustomerRoomNames.Where(e => e.Guid == request.In.LocationCode).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer guest already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var customerGuest = new CustomerGuest
        {
            CustomerReservationId = createdCustomerReservationOut.Id,
            Email = request.In.Email,
            Title = request.In.Title,
            Firstname = request.In.FirstName,
            Lastname = request.In.LastName,
            PhoneNumber = request.In.PhoneNumber,
            BlePinCode = request.In.BluetoothPinCode,
            AppAccessCode = request.In.AppAccessCode,
            KeyId = request.In.KeyId,
            IsActive = true,
            AgeCategory = 1,
            CustomerRoomGuid = request.In.LocationCode,
            RoomNumber = customerRoom.Name,
            isCheckInCompleted = false,
            isSkipCheckIn = false,
        };
        await _db.CustomerGuests.AddAsync(customerGuest);
        await _db.SaveChangesAsync(cancellationToken);

        var createdCustomerGuestOut = new CreatedCustomerGuestOut
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerGuest.CustomerReservationId,
            ArrivalFlightNumber = customerGuest.ArrivalFlightNumber,
            BlePinCode = customerGuest.BlePinCode,
            City = customerGuest.City,
            Country = customerGuest.Country,
            DepartureAirline = customerGuest.DepartureAirline,
            DepartureFlightNumber = customerGuest.DepartureFlightNumber,
            Email = customerGuest.Email,
            FirstJourneyStep = customerGuest.FirstJourneyStep,
            Firstname = customerGuest.Firstname,
            IdProof = customerGuest.IdProof,
            IdProofNumber = customerGuest.IdProofNumber,
            IdProofType = customerGuest.IdProofType,
            Language = customerGuest.Language,
            Lastname = customerGuest.Lastname,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Picture = customerGuest.Picture,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Rating = customerGuest.Rating,
            RoomNumber = customerGuest.RoomNumber,
            Signature = customerGuest.Signature,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            TermsAccepted = customerGuest.TermsAccepted,
            DateOfBirth = customerGuest.DateOfBirth,
            Vat = customerGuest.Vat,
            IsActive = customerGuest.IsActive
        };
        var customerdetails = await _db.Customers.Where(e => e.Id == customer.Id).FirstOrDefaultAsync(cancellationToken);

        var UId = CryptoExtension.Encrypt(customerGuest.Id.ToString(), (UserTypeEnum.Guest).ToString());
        createdCustomerGuestOut.Link = _frontEndLinksSettings.GuestPortal + "?id=" + UId;

        // createdCustomerGuestOut.Link = _frontEndLinksSettings.GuestPortal + "?token=" + Uri.EscapeDataString(GenerateToken(createdCustomerGuestOut.CustomerReservationId, customer.Id.ToString(), createdCustomerGuestOut.Id));

        customerGuest.GuestURL = UId;
        customerGuest.GuestToken = GenerateToken(createdCustomerGuestOut.CustomerReservationId, customer.Id.ToString(), createdCustomerGuestOut.Id);
        await _db.SaveChangesAsync(cancellationToken);
        var formBuilder = await _db.CustomerGuestsCheckInFormBuilders.Where(x => x.CustomerId == customer.Id).FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrEmpty(customerGuest.Email) && !string.IsNullOrEmpty(customer.Email))
        {
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

            <p style='color: #333;'>We are pleased to inform you that your reservation has been successfully confirmed. To enhance your stay and ensure a memorable experience, we invite you to access our guest app.</p>

  <p style='color: #333;'>Please click on the following link to access the guest app:</p>
            <div style='text-align: center;'>
              
                <a href='{createdCustomerGuestOut.Link}' style='background-color: #{formBuilder.Color}; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); display: inline-block;'>Access Guest App</a>
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
            sendEmail.Addresslist = request.In.Email;
            sendEmail.IsHTML = true;
            sendEmail.Body = body;
            sendEmail.IsNoReply = true;
            sendEmail.MaskEmail = customerdetails?.EmbededEmail;


			await _mail.ExecuteAsync(sendEmail, cancellationToken);

        }

        return _response.Success(new CreateCustomerGuestReservationOut("Create customer reservation successful.", createdCustomerReservationOut));
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
             new("UserType", ((int)UserTypeEnum.Guest).ToString(),ClaimValueTypes.String),
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
