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
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetail;
public record CreateCustomerAutoReservationWithGestDetailRequest(CreateCustomerAutoReservationWithGestDetailIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerAutoReservationWithGestDetailHandler : IRequestHandler<CreateCustomerAutoReservationWithGestDetailRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	private readonly JwtSettingsOptions _jwtSettings;
	private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
	private readonly ISendEmail _mail;
	private readonly IHttpClientFactory _HttpClientFactory;
	private readonly EndpointSettings _EndpointSettings;
	private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccountOptions;
	public CreateCustomerAutoReservationWithGestDetailHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, ISendEmail mail, IHttpClientFactory httpClientFactory, IOptions<EndpointSettings> endpointssettings, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccountOptions)
	{
		_db = db;
		_response = response;
		_jwtSettings = jwtSettings.Value;
		_frontEndLinksSettings = frontEndLinksSettings.Value;
		_mail = mail;
		_HttpClientFactory = httpClientFactory;
		_EndpointSettings = endpointssettings.Value;
		_hospitioApiStorageAccountOptions = hospitioApiStorageAccountOptions.Value;
	}
	public async Task<AppHandlerResponse> Handle(CreateCustomerAutoReservationWithGestDetailRequest request, CancellationToken cancellationToken)
	{
		var reservations = await _db.CustomerReservations
		.Where(r =>
			request.In.CheckinDate < r.CheckoutDate &&
			request.In.CheckoutDate > r.CheckinDate &&
			r.CustomerId == request.In.CustomerId)
		.ToListAsync();

		bool isRoomAvailable = !reservations.Any(r =>
			_db.CustomerGuests.Any(g => g.RoomNumber == request.In.RoomNumber && g.CustomerReservationId == r.Id));

		if (isRoomAvailable)
		{
			var customerDeatil = await _db.Customers.Where(e => e.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
			var customerRooms = await _db.CustomerRoomNames.Where(e => e.Name == request.In.RoomNumber && e.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
			var formBuilder = await _db.CustomerGuestsCheckInFormBuilders.Where(x => x.CustomerId == customerDeatil.Id).FirstOrDefaultAsync(cancellationToken);

			string uniqueId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			string reservationNumber = "GEA" + uniqueId;

			ReservationExtraDetail reservationExtraDetail = new ReservationExtraDetail();
			reservationExtraDetail.ReservationNumber = reservationNumber;
			reservationExtraDetail.UserCode = customerDeatil.Guid;
			reservationExtraDetail.LocationCode = customerRooms.Guid;

			#region centurionaddreservation
			if (customerDeatil.GuidType == 1)
			{
				var centurionApiResponse = await MakeCenturionApiCall(request, reservationExtraDetail);

				if (centurionApiResponse.Success)
				{
					reservationExtraDetail.AppAccessCode = centurionApiResponse.AccessCode;
					reservationExtraDetail.BluetoothPinCode = centurionApiResponse.BlePinCode;
					reservationExtraDetail.KeyId = centurionApiResponse.KeyId;
				}
				else
				{
					return _response.Error($"Failed to call Centurion API: {centurionApiResponse.Error}", AppStatusCodeError.InternalServerError500);
				}
			}
			#endregion

			var customerReservation = new CustomerReservation
			{
				CustomerId = request.In.CustomerId,
				ReservationNumber = reservationNumber,
				CheckinDate = request.In.CheckinDate,
				CheckoutDate = request.In.CheckoutDate,
				Source = request.In.Source,
				IsActive = true,
			};

			await _db.CustomerReservations.AddAsync(customerReservation);
			await _db.SaveChangesAsync(cancellationToken);

			var checkExist = await _db.CustomerGuests.Where(e => e.CustomerReservationId == customerReservation.Id).FirstOrDefaultAsync(cancellationToken);
			if (checkExist != null)
			{
				return _response.Error($"The customer guest already exists.", AppStatusCodeError.UnprocessableEntity422);
			}
			var customerGuest = new CustomerGuest
			{
				CustomerReservationId = customerReservation.Id,
				Email = request.In.Email,
				FirstJourneyStep = request.In.FirstJourneyStep,
				Title = request.In.Title,
				Firstname = request.In.Firstname,
				Lastname = request.In.Lastname,
				PhoneCountry = request.In.PhoneCountry,
				PhoneNumber = request.In.PhoneNumber,
				IsActive = true,
				AgeCategory = 1,
				RoomNumber = request.In.RoomNumber,
				isCheckInCompleted = false,
				isSkipCheckIn = false,
				AppAccessCode = reservationExtraDetail.AppAccessCode,
				BlePinCode = reservationExtraDetail.BluetoothPinCode,
				KeyId = reservationExtraDetail.KeyId,
			};

			await _db.CustomerGuests.AddAsync(customerGuest);
			await _db.SaveChangesAsync(cancellationToken);

			var UId = CryptoExtension.Encrypt(customerGuest.Id.ToString(), (UserTypeEnum.Guest).ToString());

			string Link = _frontEndLinksSettings.GuestPortal + "?id=" + UId;
			//string Link = _frontEndLinksSettings.GuestPortal + "?token=" + Uri.EscapeDataString(GenerateToken(customerReservation.Id, request.In.CustomerId.ToString(), customerGuest.Id));

			customerGuest.GuestURL = UId;
			customerGuest.GuestToken = GenerateToken(customerReservation.Id, request.In.CustomerId.ToString(), customerGuest.Id);
			await _db.SaveChangesAsync(cancellationToken);

			//string body = $"<a href='{Link}'>{Link}</a>" ;

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
                    <img src='{_hospitioApiStorageAccountOptions.BlobStorageBaseURL}{formBuilder.Logo}' width='100' height='100' alt='{customerDeatil.BusinessName} Logo' />
                </td>
            </tr>
        </table>

        <div style='background-color: #fff; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); border-radius: 10px; padding: 20px;'>
            <p style='color: #333;'>Dear {customerGuest.Firstname} {customerGuest.Lastname},</p>

            <p style='color: #333;'>We are pleased to inform you that your reservation has been successfully confirmed. To enhance your stay and ensure a memorable experience, we invite you to access our guest app.</p>

  <p style='color: #333;'>Please click on the following link to access the guest app:</p>
            <div style='text-align: center;'>
              
                <a href='{Link}' style='background-color: #{formBuilder.Color}; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); display: inline-block;'>Access Guest App</a>
            </div>

            <p style='color: #333;'>With our guest app, you can access a range of services, view local recommendations, and make the most of your flexible stay with us.</p>

            <p style='color: #333;'>If you have any questions or concerns, please do not hesitate to contact us at <a href='{customerDeatil.Email}' style='color: #007bff; text-decoration: none;'>{customerDeatil.Email}</a>, and our team will be happy to assist you.</p>

            <p style='color: #333;'>Thank you for choosing {customerDeatil.BusinessName}. We look forward to welcoming you and making your stay exceptional.</p>

             <span style='color: #{formBuilder.Color};'>The {customerDeatil.BusinessName} Team</span></p>
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
			sendEmail.MaskEmail = customerDeatil.EmbededEmail;

			await _mail.ExecuteAsync(sendEmail, cancellationToken);

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

			return _response.Success(new CreateCustomerAutoReservationWithGestDetailOut("Create customer reservation successful.", createdCustomerReservationOut));
		}
		else
		{
			return _response.Error($"This Room is not available for the requested dates.", AppStatusCodeError.UnprocessableEntity422);
		}
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
			 new("UserId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
			 new("CustomerId", customerId.ToString(),ClaimValueTypes.Integer64),
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
	private async Task<ApiResponse> MakeCenturionApiCall(CreateCustomerAutoReservationWithGestDetailRequest request, ReservationExtraDetail reservationExtraDetail)
	{
		try
		{
			CultureInfo culture = new CultureInfo("en-US");
			var httpClient = _HttpClientFactory.CreateClient("Centurion");
			httpClient = await AssignBearerToken(httpClient, _EndpointSettings.Centurion.Username, _EndpointSettings.Centurion.Password);
			var reservationDto = new
			{
				title = request.In.Title,
				firstName = request.In.Firstname,
				lastName = request.In.Lastname,
				email = request.In.Email,
				phoneNumber = request.In.PhoneNumber,
				arrivalDate = request.In.CheckinDate?.ToString("dd/MM/yyyy", culture),
				departureDate = request.In.CheckoutDate?.ToString("dd/MM/yyyy", culture),
				arrivalTime = request.In.CheckinDate?.ToString("HH:mm"),
				departureTime = request.In.CheckoutDate?.ToString("HH:mm"),
				reservationNumber = reservationExtraDetail.ReservationNumber,
				locationCode = reservationExtraDetail.LocationCode,
				userCode = reservationExtraDetail.UserCode,
				bluetoothPinCode = reservationExtraDetail.BluetoothPinCode,
				appAccessCode = reservationExtraDetail.AppAccessCode,
			};

			var apiResponse = await httpClient.PostAsync("api/middleware/reservation", SerializeObjectForPost(reservationDto));

			var response = new ApiResponse() { Success = false };
			var result = await apiResponse.Content.ReadAsStringAsync();
			response = !string.IsNullOrEmpty(result) ? JsonSerializer.Deserialize<ApiResponse>(result) : response;

			return response;
		}
		catch (Exception ex)
		{
			// Handle any exceptions that may occur during the API call
			// You can log the exception or handle it as needed
			return new ApiResponse
			{
				Success = false,
				Error = $"Failed to call Centurion API: {ex.Message}"
			};
		}
	}
	public static async Task<HttpClient> AssignBearerToken(HttpClient client, string username, string password)
	{
		var response = await client.PostAsync("api/middleware/token", SerializeObjectForPost(new { username, password }));

		if (response.IsSuccessStatusCode)
		{
			var apiToken = JsonSerializer.Deserialize<ApiToken>(await response.Content.ReadAsStringAsync());
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.token);
		}

		return client;
	}
	public static StringContent SerializeObjectForPost(object obj)
	{
		return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
	}
	public sealed record ApiToken(string token);
}
public sealed class ApiResponse
{
	[JsonPropertyName("success")]
	public bool Success { get; set; } = true;
	[JsonPropertyName("error")]
	public string? Error { get; set; }
	[JsonPropertyName("accessCode")]
	public string? AccessCode { get; set; }
	[JsonPropertyName("blePinCode")]
	public string? BlePinCode { get; set; }
	[JsonPropertyName("keyId")]
	public int? KeyId { get; set; }

	public void AddResponse(ApiResponse response)
	{
		Success = response.Success;
		Error += response.Error;
	}

}
public class ReservationExtraDetail
{
	public string ReservationNumber { get; set; }
	public Guid LocationCode { get; set; }
	public Guid UserCode { get; set; }
	public string? BluetoothPinCode { get; set; }
	public string? AppAccessCode { get; set; }
	public int? KeyId { get; set; }
}