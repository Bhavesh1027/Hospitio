using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerMerchantAccount.Queries.GetCustomerMerchantAccount;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack;
using HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;
using HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;
using HospitioApi.Core.HandleReplicateDataForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HospitioApi.Core.Services.Common;

public sealed class HospitioLocationDto
{
	public Guid LocationCode { get; set; }
	public Guid UserCode { get; set; }
	public string? AvantioAccomodationRefrence { get; set; }
	public string UniversalLocationName { get; set; }
	public bool? HasCenturionPlatform { get; set; }
	public bool? HasGeaPlatform { get; set; }
}

public class CommonDataBaseOprationService : ICommonDataBaseOprationService
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<CommonDataBaseOprationService> _logger;
	private readonly IHandlerResponseFactory _response;
	private readonly IJwtService _jwtService;
	private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
	private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;
	private readonly ChatWidgetLinksSettingsOptions _chatWidgetLinksSettings;
	private readonly EndpointSettings _endpointSettings;
	private readonly CenturionAPIGetTokenCredentialOptions _centurionAPIGetTokenCredential;


	public CommonDataBaseOprationService(IConfiguration configuration, ILogger<CommonDataBaseOprationService> logger, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions, IOptions<ChatWidgetLinksSettingsOptions> chatWidgetLinkSettingOptions
		, IOptions<EndpointSettings> endpointSettings, IOptions<CenturionAPIGetTokenCredentialOptions> centurionAPIGetTokenCredential)
	{
		_response = response;
		_logger = logger;
		_configuration = configuration;
		_jwtService = jwtService;
		_gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
		_middlewareApiSettingsOptions = middlewareApiSettingsOptions.Value;
		_chatWidgetLinksSettings = chatWidgetLinkSettingOptions.Value;
		_endpointSettings = endpointSettings.Value;
		_centurionAPIGetTokenCredential = centurionAPIGetTokenCredential.Value;
	}
	#region Customer
	public async Task<Customer> CustomersAdd(CreateCustomerIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		bool storedSuccessfully = false;
		byte guidType = 0;
		Guid guid = Guid.Empty;

		if (string.IsNullOrEmpty(request.CenturianHotelCode))
		{
			while (!storedSuccessfully)
			{
				Guid uniqueGuid = Guid.NewGuid();
				bool isExist = IsGuidUniqueInCustomers(uniqueGuid, _db);
				if (!isExist)
				{
					guid = uniqueGuid;
					Console.WriteLine("GUID stored successfully!");
					storedSuccessfully = true;
				}
			}
			guidType = 2;
		}
		else
		{
			guid = Guid.Parse(request.CenturianHotelCode);
			guidType = 1;
		}

		var customer = new Customer()
		{
			BusinessTypeId = request.BusinessTypeId,
			BusinessName = request.BusinessName,
			NoOfRooms = request.NoOfRooms,
			PhoneCountry = request.PhoneCountry,
			PhoneNumber = request.PhoneNumber,
			IsActive = request.IsActive,
			Guid = guid,
			GuidType = guidType,
			ProductId = request.ProductId,
			Email = request.CustomerUserIn.Email,
			WhatsappCountry = request.WatsappCountry,
			WhatsappNumber = request.WatsappNumber,
			ViberCountry = request.ViberCountry,
			ViberNumber = request.ViberNumber,
			TelegramCounty = request.TelegramCounty,
			TelegramNumber = request.TelegramNumber,
			Vat = request.VatNumber
		};
		await _db.Customers.AddAsync(customer, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);

		customer.WidgetChatId = CryptoExtension.Encrypt(customer.Id.ToString(), (UserTypeEnum.Customer).ToString());
		await _db.SaveChangesAsync(cancellationToken);
		return customer;

	}

	public async Task<Customer> ERPCustomersAdd(CreateERPCustomerIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		try
		{
			Guid guidResult = Guid.Parse(request.PylonUniqueCustomerId);
			bool isCenturianCustomer = await CheckCustomerIsCenturian(guidResult);

			byte guidType = 0;
			Guid guid = Guid.Empty;

			#region when remove a isCenturianCustomer field from the payload
			if (!isCenturianCustomer)
			{
				guid = Guid.Parse(request.PylonUniqueCustomerId);
				guidType = 2;
			}
			else
			{
				guid = Guid.Parse(request.PylonUniqueCustomerId);
				guidType = 1;
			}
			#endregion

			var businessType = await _db.BusinessTypes.Where(b => b.BizType == request.BusinessType).FirstOrDefaultAsync(cancellationToken);
			var product = await _db.Products.Where(p => p.Name == request.ServicePack).FirstOrDefaultAsync(cancellationToken);

			var customer = new Customer()
			{
				BusinessTypeId = businessType?.Id,
				BusinessName = request.CompanyName,
				NoOfRooms = request.NoOfRooms,
				PhoneCountry = request.PhoneCountry,
				PhoneNumber = request.Mobile,
				IsActive = true,
				Guid = guid,
				GuidType = guidType,
				ProductId = product?.Id,
				Email = request.Email,
				//Vat = request.VAT,
				SubscriptionExpirationDate = DateTime.UtcNow.AddDays((double)request.ExpirationInDay),
				//Postalcode = request.PostalCode,
				//Country = request.Country,
				IsPylonGenerated = true
			};
			await _db.Customers.AddAsync(customer, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);

			customer.WidgetChatId = CryptoExtension.Encrypt(customer.Id.ToString(), (UserTypeEnum.Customer).ToString());
			await _db.SaveChangesAsync(cancellationToken);
			return customer;
		}
		catch (Exception ex)
		{
			_logger.LogError("Error Occured When Call AddERPCustomer Service :" + ex);
			throw;
		}

	}

	public static bool IsGuidUniqueInCustomerRoomNames(Guid guidString, ApplicationDbContext _db)
	{
		bool exist = _db.CustomerRoomNames.Where(e => e.Guid == guidString).Any();
		return exist;
	}
	public static bool IsGuidUniqueInCustomers(Guid guidString, ApplicationDbContext _db)
	{
		bool exist = _db.Customers.Where(e => e.Guid == guidString).Any();
		return exist;
	}

	public async Task<Customer> ERPCustomerServiceUpdate(UpdateERPCustomer request, Customer customer, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		customer.IsActive = true;
		customer.ProductId = request.ServicePackId;
		customer.SubscriptionExpirationDate = DateTime.UtcNow.AddDays((double)request.ExpirationInDay);
		await _db.SaveChangesAsync(cancellationToken);

		return customer;
	}

	public async Task<Customer> CustomersUpdate(UpdateCustomerIn request, Customer customer, ApplicationDbContext _db, CancellationToken cancellationToken, ISendEmail _mail)
	{
		if (customer.WidgetChatId == null)
		{
			customer.WidgetChatId = CryptoExtension.Encrypt(customer.Id.ToString(), (UserTypeEnum.Customer).ToString());
		}

		#region ChatWidgetMailContent
		if (!string.IsNullOrEmpty(request.Cname) && customer.Cname != request.Cname && customer.Email != null)
		{
			string body = $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Integration Instructions for Chat Widget</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0; border-radius: 10px;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f2ddf2f2; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2); border-radius: 10px;'>
        <table width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace: 0; mso-table-rspace: 0; background-color: #f8f8f8; border-radius: 10px;'>
            <tr>
                <td style='padding: 20px 0; text-align: center; background-color: #5400cf; border-radius: 10px;'>
                    <img src='https://geastorage2023.blob.core.windows.net/hospitiodatastore/hospitio-logo.png' width='150' height='38.46' alt='Hospitio Logo' />
                </td>
            </tr>
        </table>

        <div style='background-color: #fff; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2); border-radius: 10px; padding: 20px;'>
            <p style='color: #333;'>Dear {customer.BusinessName},</p>

            <p style='color: #333;'>I hope this message finds you well. We are excited to inform you about a new feature that will enhance the communication experience on your website - the Chat Widget!</p>

  <p style='color: #333;'>To integrate the Chat Widget into your website, please follow these simple steps:</p>
            <ol>
        <li>Copy the following script tag:</li>
    </ol>
<pre>
        <code>
            &lt;script
              id=""chat-widget-config""
              data-button-color=""blue""
              data-button-background-color=""grey""
              src=""{_chatWidgetLinksSettings.ChatWidgetJs}fixed-button.js?apiKey={customer.WidgetChatId}""
              defer
              type=""text/javascript""
            &gt;&lt;/script&gt;
        </code>
    </pre>
            <p style='color: #333;'>Paste the script tag into the &lt;body&gt; section of your website's HTML code. This will ensure that the Chat Widget is correctly loaded and visible to your visitors.</p>

            <p style='color: #333;'>For any queries, kindly contact us</p>

            <p style='color: #333;'>Thank you for choosing Hospitio. We look forward to providing an improved communication experience for your website visitors.</p>

             <span style='color: #5400cf;'>The Hospitio Team</span></p>
        </div>
    </div>
</body>
</html>";



			SendEmailOptions sendEmail = new SendEmailOptions();
			sendEmail.Subject = "Important: Integration Instructions for Your Website's Chat Widget";
			sendEmail.Addresslist = customer.Email;
			sendEmail.IsHTML = true;
			sendEmail.Body = body;
			sendEmail.IsNoReply = true;

			await _mail.ExecuteAsync(sendEmail, cancellationToken);
		}
		#endregion

		if (request.Email != customer.Email || request.PMSId != customer.PMSId || request.PMSAPIAuthUsername != customer.PMSAPIAuthUsername || request.PMSAPIAuthPassword != customer.PMSAPIAuthPassword || request.CheckInPolicy != customer.CheckInPolicy || request.CheckOutPolicy != customer.CheckOutPolicy)
		{
			#region middleware
			var client = new HttpClient();

			string firstApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/token";

			var requestBody = new
			{
				username = _middlewareApiSettingsOptions.UserName,
				password = _middlewareApiSettingsOptions.Password,
			};

			var requestBodyJson = JsonConvert.SerializeObject(requestBody);
			var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

			// Send a POST request to obtain the token
			var response = await client.PostAsync(firstApiUrl, requestContent);

			string? token = null;
			if (response.IsSuccessStatusCode)
			{
				// Read the token from the response
				var responseContent = await response.Content.ReadAsStringAsync();
				var responseObject = JObject.Parse(responseContent);

				if (responseObject != null && responseObject["token"] != null)
				{
					token = responseObject["token"].Value<string>();
				}
			}

			if (!string.IsNullOrEmpty(token))
			{
				HttpClient httpClient = new HttpClient();
				string middlewareApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/users";
				httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

				PMS pmsValue = (PMS)(request.PMSId ?? 1);

				var middlewareRequestBody = new
				{
					userCode = customer.Guid,
					email = request.Email,
					pMS = pmsValue.ToString(),
					pMSAPIAuthUsername = request.PMSAPIAuthUsername,
					pMSAPIAuthPassword = request.PMSAPIAuthPassword,
					checkInPolicy = request.CheckInPolicy,
					checkOutPolicy = request.CheckOutPolicy,
				};

				var middlewareRequestBodyJson = JsonConvert.SerializeObject(middlewareRequestBody);
				requestContent = new StringContent(middlewareRequestBodyJson, Encoding.UTF8, "application/json");

				response = await httpClient.PostAsync(middlewareApiUrl, requestContent);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					Console.WriteLine("Middleware API response: " + result);
				}
				else
				{
					Console.WriteLine("Failed to call the middleware API. Status code: " + response.StatusCode);
				}
			}
			#endregion
		}

		customer.BusinessTypeId = request.BusinessTypeId;
		customer.BusinessName = request.BusinessName;
		customer.NoOfRooms = request.NoOfRooms;
		customer.TimeZone = request.TimeZone;
		customer.WhatsappCountry = request.WhatsappCountry;
		customer.WhatsappNumber = request.WhatsappNumber;
		customer.Cname = request.Cname;
		customer.ClientDoamin = request.ClientDoamin;
		customer.Email = request.Email;
		customer.Messenger = request.Messenger;
		customer.SmsTitle = request.SmsTitle;
		customer.ViberCountry = request.ViberCountry;
		customer.ViberNumber = request.ViberNumber;
		customer.TelegramCounty = request.TelegramCounty;
		customer.TelegramNumber = request.TelegramNumber;
		customer.PhoneCountry = request.PhoneCountry;
		customer.PhoneNumber = request.PhoneNumber;
		customer.BusinessStartTime = request.BusinessStartTime;
		customer.BusinessCloseTime = request.BusinessCloseTime;
		customer.DoNotDisturbGuestStartTime = request.DoNotDisturbGuestStartTime;
		customer.DoNotDisturbGuestEndTime = request.DoNotDisturbGuestEndTime;
		customer.StaffAlertsOffduty = request.StaffAlertsOffduty;
		customer.NoMessageToGuestWhileQuiteTime = request.NoMessageToGuestWhileQuiteTime;
		customer.IncomingTranslationLangage = request.IncomingTranslationLangage;
		customer.NoTranslateWords = request.NoTranslateWords;
		customer.CurrencyCode = request.CurrencyCode;
		customer.IsActive = request.IsActive;
		customer.Latitude = request.Latitude;
		customer.Longitude = request.Longitude;
		customer.IsTwoWayComunication = request.IsTwoWayComunication;
		customer.PMSId = request.PMSId;
		customer.PMSAPIAuthUsername = request.PMSAPIAuthUsername;
		customer.PMSAPIAuthPassword = request.PMSAPIAuthPassword;
		customer.CheckInPolicy = request.CheckInPolicy;
		customer.CheckOutPolicy = request.CheckOutPolicy;
		customer.PMSAPIAuthUsername = request.PMSAPIAuthUsername;
		customer.PMSAPIAuthPassword = request.PMSAPIAuthPassword;
		customer.EmbededEmail = request.EmbededEmail;
		await _db.SaveChangesAsync(cancellationToken);
		return customer;
	}
	#endregion

	#region CustomerRoomNames
	public async Task<List<CustomerRoomName>> CustomerRoomNamesUpdate(List<UpdateCustomerRoomNamesIn> request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken, UserTypeEnum UserType)
	{
		var customerRoomNamesResponse = new List<CustomerRoomName>();
		var customerRoomNames = new List<CustomerRoomName>();
		var hospitioLocationDtos = new List<HospitioLocationDto>();
		var customer = await _db.Customers.Where(c => c.Id == CustomerId).FirstOrDefaultAsync(cancellationToken);

		foreach (var item in request)
		{
			var hospitioLocationDto = new HospitioLocationDto();
			if (item.Id > 0)
			{
				var customerRoom = await _db.CustomerRoomNames.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
				if (customerRoom != null)
				{
					customerRoom.CustomerId = CustomerId;
					customerRoom.IsActive = item.IsActive;
					if (UserType == UserTypeEnum.Hospitio)
					{
						customerRoom.CreatedFrom = (byte)UserTypeEnum.Hospitio;
					}
					else if (UserType == UserTypeEnum.Customer)
					{
						customerRoom.CreatedFrom = (byte)UserTypeEnum.Customer;
						item.LocationType = item.LocationType ?? 2;

						if (item.LocationType == 1)
						{
							if (Guid.Parse(item.CenturionLocationCode) != customerRoom.Guid)
							{
								hospitioLocationDto.LocationCode = customerRoom.Guid;
								hospitioLocationDto.UserCode = customer.Guid;
								hospitioLocationDto.UniversalLocationName = customerRoom.Name;
								hospitioLocationDto.HasCenturionPlatform = false;
								hospitioLocationDto.HasGeaPlatform = false;
								hospitioLocationDto.AvantioAccomodationRefrence = item.Gui;
								hospitioLocationDtos.Add(hospitioLocationDto);
							}
							hospitioLocationDto.LocationCode = Guid.Parse(item.CenturionLocationCode);
							hospitioLocationDto.UserCode = customer.Guid;
							hospitioLocationDto.UniversalLocationName = item.Name;
							hospitioLocationDto.HasCenturionPlatform = item.LocationType == 1 ? true : false;
							hospitioLocationDto.HasGeaPlatform = item.LocationType == 2 || item.LocationType == 1 ? true : false;
							hospitioLocationDto.AvantioAccomodationRefrence = item.Gui;
							hospitioLocationDtos.Add(hospitioLocationDto);
							customerRoom.Guid = Guid.Parse(item.CenturionLocationCode);
							customerRoom.GuidType = item.LocationType;
							customerRoom.Name = item.Name;
							customerRoom.AvantioAccomodationRefrence = item.Gui;
						}
						if (item.LocationType == 2)
						{
							hospitioLocationDto.LocationCode = Guid.Parse(item.CenturionLocationCode);
							hospitioLocationDto.UserCode = customer.Guid;
							hospitioLocationDto.UniversalLocationName = item.Name;
							hospitioLocationDto.HasCenturionPlatform = item.LocationType == 1 ? true : false;
							hospitioLocationDto.HasGeaPlatform = item.LocationType == 2 || item.LocationType == 1 ? true : false;
							hospitioLocationDto.AvantioAccomodationRefrence = item.Gui;
							hospitioLocationDtos.Add(hospitioLocationDto);
							customerRoom.Name = item.Name;
							customerRoom.AvantioAccomodationRefrence = item.Gui;
						}

					}

					customerRoomNamesResponse.Add(customerRoom);
					await _db.SaveChangesAsync(cancellationToken);
				}
			}
			else
			{
				bool storedSuccessfully = false;
				byte GuidType = item.LocationType ?? 2;
				Guid guid = Guid.Empty;

				if (GuidType == 2)
				{
					while (!storedSuccessfully)
					{
						Guid uniqueGuid = Guid.NewGuid();
						bool isExist = IsGuidUniqueInCustomerRoomNames(uniqueGuid, _db);
						if (!isExist)
						{
							guid = uniqueGuid;
							Console.WriteLine("GUID stored successfully!");
							storedSuccessfully = true;
						}
					}
				}
				else
				{
					guid = Guid.Parse(item.CenturionLocationCode);
				}

				var customerRoomName = new CustomerRoomName()
				{
					CustomerId = CustomerId,
					Name = item.Name,
					IsActive = item.IsActive,
				};
				if (UserType == UserTypeEnum.Hospitio)
				{
					customerRoomName.CreatedFrom = (byte)UserTypeEnum.Hospitio;
				}
				else if (UserType == UserTypeEnum.Customer)
				{
					customerRoomName.CreatedFrom = (byte)UserTypeEnum.Customer;
					customerRoomName.Guid = guid;
					customerRoomName.GuidType = GuidType;
					customerRoomName.AvantioAccomodationRefrence = item.Gui;
					hospitioLocationDto.LocationCode = guid;
					hospitioLocationDto.UserCode = customer.Guid;
					hospitioLocationDto.UniversalLocationName = item.Name;
					hospitioLocationDto.HasCenturionPlatform = GuidType == 1 ? true : false;
					hospitioLocationDto.HasGeaPlatform = GuidType == 2 || GuidType == 1 ? true : false;
					hospitioLocationDto.AvantioAccomodationRefrence = item.Gui;
					hospitioLocationDtos.Add(hospitioLocationDto);
				}
				customerRoomNames.Add(customerRoomName);
				customerRoomNamesResponse.Add(customerRoomName);
			}
		}
		if (customerRoomNames.Any())
		{
			await _db.CustomerRoomNames.AddRangeAsync(customerRoomNames, cancellationToken);
		}

		await _db.SaveChangesAsync(cancellationToken);

		#region middleware
		using var client = new HttpClient();

		string firstApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/token";

		var requestBody = new
		{
			username = _middlewareApiSettingsOptions.UserName,
			password = _middlewareApiSettingsOptions.Password,
		};

		var requestBodyJson = JsonConvert.SerializeObject(requestBody);
		var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

		// Send a POST request to obtain the token
		var response = await client.PostAsync(firstApiUrl, requestContent);

		string? token = null;
		if (response.IsSuccessStatusCode)
		{
			// Read the token from the response
			var responseContent = await response.Content.ReadAsStringAsync();
			var responseObject = JObject.Parse(responseContent);

			if (responseObject != null && responseObject["token"] != null)
			{
				token = responseObject["token"].Value<string>();
			}
		}

		if (!string.IsNullOrEmpty(token))
		{
			HttpClient httpClient = new HttpClient();
			string middlewareApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/locations";
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

			var middlewareRequestBodyJson = JsonConvert.SerializeObject(hospitioLocationDtos);
			requestContent = new StringContent(middlewareRequestBodyJson, Encoding.UTF8, "application/json");

			response = await httpClient.PostAsync(middlewareApiUrl, requestContent);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				Console.WriteLine("Middleware API response: " + result);
			}
			else
			{
				Console.WriteLine("Failed to call the middleware API. Status code: " + response.StatusCode);
			}
		}
		#endregion

		return customerRoomNamesResponse;
	}
	#endregion

	#region CustomersUser
	public async Task<CustomerUser> CustomersUserAdd(CustomerUserIn request, int customerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerUser = new CustomerUser()
		{
			CustomerId = customerId,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			Title = request.Title,
			UserName = request.UserName,
			Password = CryptoExtension.Encrypt(request.Password, customerId.ToString()),
			IsActive = request.IsActive,
			CustomerLevelId = (int)Shared.Enums.UserLevel.SuperAdmin
		};

		await _db.CustomerUsers.AddAsync(customerUser, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
		return customerUser;
	}
	#endregion

	#region CustomersReceptionDelete
	//public async Task CustomersReceptionDelete(CustomerGuestAppReceptionCategory customerReceptionCategory, ApplicationDbContext _db, CancellationToken cancellationToken)
	//{
	//    List<CustomerGuestAppReceptionItem> customerReceptionItems = _db.CustomerGuestAppReceptionItems.Where(e => e.CustomerGuestAppReceptionCategoryId == customerReceptionCategory.Id).ToList();

	//    if (customerReceptionItems.Count > 0)
	//    {
	//        _db.CustomerGuestAppReceptionItems.RemoveRange(customerReceptionItems);
	//    }

	//    _db.CustomerGuestAppReceptionCategories.Remove(customerReceptionCategory);
	//    await _db.SaveChangesAsync(cancellationToken);
	//}
	#endregion

	#region CustomersCategoryItemExtraDelete
	public async Task CustomersCategoryItemExtraDelete(List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> customerGuestAppEnhanceYourStayCategoryItems, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		_db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.RemoveRange(customerGuestAppEnhanceYourStayCategoryItems);
		await _db.SaveChangesAsync(cancellationToken);
	}
	#endregion

	#region CustomersHouseKeepingDelete
	//public async Task CustomersHouseKeepingDelete(CustomerGuestAppHousekeepingCategory customerHouseKeepingCategory, ApplicationDbContext _db, CancellationToken cancellationToken)
	//{
	//    List<CustomerGuestAppHousekeepingItem> customerHouseKeepingItems = _db.CustomerGuestAppHousekeepingItems.Where(e => e.CustomerGuestAppHousekeepingCategoryId == customerHouseKeepingCategory.Id).ToList();

	//    if (customerHouseKeepingItems.Count > 0)
	//    {
	//        _db.CustomerGuestAppHousekeepingItems.RemoveRange(customerHouseKeepingItems);
	//    }

	//    _db.CustomerGuestAppHousekeepingCategories.Remove(customerHouseKeepingCategory);
	//    await _db.SaveChangesAsync(cancellationToken);
	//}
	#endregion

	#region CustomersHouseKeepingItemsUpdate
	public async Task<List<CustomerGuestAppHousekeepingItem>> CustomersHouseKeepingItemsUpdate(List<UpdateCustomerHouseKeepingItemIn> request, int customerHouseKeepingCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var cutomerHouseKeepingitems = new List<CustomerGuestAppHousekeepingItem>();

		foreach (var item in request)
		{
			if (item.Id > 0)
			{
				var updateCustomerHouseKeepingItem = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);

				//if (updateCustomerHouseKeepingItem is null)
				//{
				//    return _response.Error("No items availbale for given HouseKeeping Service", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
				//}

				//updateCustomerHouseKeepingItem.CustomerId = item.CustomerId;
				updateCustomerHouseKeepingItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
				updateCustomerHouseKeepingItem.CustomerGuestAppHousekeepingCategoryId = customerHouseKeepingCategoryId;
				//updateCustomerHouseKeepingItem.CategoryName = item.CategoryName;
				updateCustomerHouseKeepingItem.Name = item.Name;
				updateCustomerHouseKeepingItem.ItemsMonth = item.ItemsMonth;
				updateCustomerHouseKeepingItem.ItemsDay = item.ItemsDay;
				updateCustomerHouseKeepingItem.ItemsMinute = item.ItemsMinute;
				updateCustomerHouseKeepingItem.ItemsHour = item.ItemsHour;
				updateCustomerHouseKeepingItem.QuantityBar = item.QuantityBar;
				updateCustomerHouseKeepingItem.ItemLocation = item.ItemLocation;
				updateCustomerHouseKeepingItem.Comment = item.Comment;
				updateCustomerHouseKeepingItem.IsPriceEnable = item.IsPriceEnable;
				updateCustomerHouseKeepingItem.Price = item.Price;
				updateCustomerHouseKeepingItem.Currency = item.Currency;

				cutomerHouseKeepingitems.Add(updateCustomerHouseKeepingItem);
			}
			else
			{
				var houseKeepingItem = new CustomerGuestAppHousekeepingItem()
				{
					//CustomerId = item.CustomerId,
					CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
					CustomerGuestAppHousekeepingCategoryId = customerHouseKeepingCategoryId,
					Name = item.Name,
					ItemsMonth = item.ItemsMonth,
					ItemsDay = item.ItemsDay,
					ItemsMinute = item.ItemsMinute,
					ItemsHour = item.ItemsHour,
					QuantityBar = item.QuantityBar,
					ItemLocation = item.ItemLocation,
					Comment = item.Comment,
					IsPriceEnable = item.IsPriceEnable,
					Price = item.Price,
					Currency = item.Currency
				};
				await _db.CustomerGuestAppHousekeepingItems.AddAsync(houseKeepingItem, cancellationToken);
				cutomerHouseKeepingitems.Add(houseKeepingItem);
			}
		}


		await _db.SaveChangesAsync(cancellationToken);
		return cutomerHouseKeepingitems;
	}
	#endregion

	#region CustomerGuestAppEnhanceYourStay
	public async Task<List<CustomerGuestAppEnhanceYourStayCategory>> CustomerGuestAppEnhanceYourStay(CreateCustomerEnhanceYourStayIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var In = request;
		var customerGuestAppEnhanceYourStayCategories = new List<CustomerGuestAppEnhanceYourStayCategory>();
		var customerGuestAppEnhanceYourStayItems = new List<CustomerGuestAppEnhanceYourStayItem>();
		var customerGuestAppEnhanceYourStays = new List<CustomerGuestAppEnhanceYourStayCategory>();
		if (In.categoryNames != null)
		{
			//foreach (var nestIn in In.categoryNames)
			//{
			var nestIn = In.categoryNames;
			if (In.categoryNames.CategoryId > 0)
			{
				var objec = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(e => e.Id == nestIn.CategoryId).FirstOrDefaultAsync(cancellationToken);
				if (objec != null)
				{
					//objec.CategoryName = nestIn.Name;
					//objec.DisplayOrder = nestIn.CategoryDisplayOrder;
					//objec.IsPublish = nestIn.IsPublish;
					objec.JsonData = JsonConvert.SerializeObject(nestIn);
					customerGuestAppEnhanceYourStayCategories.Add(objec);
				}
				if (nestIn.categoryItems != null)
				{
					foreach (var categoryItem in nestIn.categoryItems)
					{
						var customerGuestAppEnhanceYourStayItem = await _db.CustomerGuestAppEnhanceYourStayItems.Where(e => e.Id == categoryItem.CategoryItemId).FirstOrDefaultAsync(cancellationToken);
						if (customerGuestAppEnhanceYourStayItem.JsonData != null)
						{
							var result = System.Text.Json.JsonSerializer.Deserialize<UpdateCustomerEnhanceYourStayItemIn>(customerGuestAppEnhanceYourStayItem.JsonData);

							result.IsActive = categoryItem.IsActive;
							result.DisplayOrder = categoryItem.ItemDisplayOrder;
							result.IsDeleted = categoryItem.IsDeleted;
							customerGuestAppEnhanceYourStayItem.JsonData = JsonConvert.SerializeObject(result);

						}
						else
						{
							if (categoryItem.IsDeleted == true)
							{
								_db.CustomerGuestAppEnhanceYourStayItems.Remove(customerGuestAppEnhanceYourStayItem);
								await _db.SaveChangesAsync(cancellationToken);
							}
							else
							{
								customerGuestAppEnhanceYourStayItem.DisplayOrder = categoryItem.ItemDisplayOrder;
								customerGuestAppEnhanceYourStayItem.IsActive = categoryItem.IsActive;
								customerGuestAppEnhanceYourStayItems.Add(customerGuestAppEnhanceYourStayItem);
							}
						}

					}
				}
			}
			else
			{
				var customerGuestAppEnhanceYourStayCategory = new CustomerGuestAppEnhanceYourStayCategory()
				{
					CustomerGuestAppBuilderId = In.CustomerGuestAppBuilderId,
					CustomerId = In.CustomerId,
					CategoryName = nestIn.Name,
					IsActive = true,
					DisplayOrder = nestIn.CategoryDisplayOrder,
					IsPublish = false,
					JsonData = null
				};
				customerGuestAppEnhanceYourStays.Add(customerGuestAppEnhanceYourStayCategory);
				customerGuestAppEnhanceYourStayCategories.Add(customerGuestAppEnhanceYourStayCategory);

			}
			//}
			if (customerGuestAppEnhanceYourStays != null)
			{
				await _db.CustomerGuestAppEnhanceYourStayCategories.AddRangeAsync(customerGuestAppEnhanceYourStays, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		return customerGuestAppEnhanceYourStayCategories;
	}
	#endregion

	#region CustomersGuestAppEnhanceYourStayCategoryItemExtra
	public async Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersGuestAppEnhanceYourStayCategoryItemExtra(CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerGuestAppEnhanceYourStayCategoryItems = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
		var customerGuests = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
		if (request.createCustomerGuestAppEnhanceYourStayCategoryItems != null)
		{
			foreach (var nestIn in request.createCustomerGuestAppEnhanceYourStayCategoryItems)
			{
				if (nestIn.Id.HasValue && nestIn.Id.Value > 0)
				{
					var obj = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(e => e.Id == nestIn.Id.Value).FirstOrDefaultAsync(cancellationToken);

					if (obj != null)
					{
						obj.CustomerGuestAppEnhanceYourStayItemId = request.CustomerGuestAppEnhanceYourStayItemId;
						obj.QueType = nestIn.QueType;
						obj.Questions = nestIn.Questions;
						obj.OptionValues = nestIn.OptionValues;
						customerGuestAppEnhanceYourStayCategoryItems.Add(obj);
					}
					//else
					//{
					//    continue;
					//}
				}
				else
				{
					var customerGuest = new CustomerGuestAppEnhanceYourStayCategoryItemsExtra()
					{
						CustomerGuestAppEnhanceYourStayItemId = request.CustomerGuestAppEnhanceYourStayItemId,
						QueType = nestIn.QueType,
						Questions = nestIn.Questions,
						OptionValues = nestIn.OptionValues,
						IsActive = true
					};
					customerGuests.Add(customerGuest);
					customerGuestAppEnhanceYourStayCategoryItems.Add(customerGuest);
				}
			}
			if (customerGuests.Any())
			{
				await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.AddRangeAsync(customerGuests, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);

		}
		return customerGuestAppEnhanceYourStayCategoryItems;

	}
	#endregion

	#region CustomersRoomServiceAdd
	public async Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceAdd(CreateCustomerRoomServiceCategoryIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerRoomServiceCategory = new CustomerGuestAppRoomServiceCategory
		{
			CustomerId = request.CustomerId,
			CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
			CategoryName = request.CategoryName,
			DisplayOrder = request.DisplayOrder,
		};
		await _db.CustomerGuestAppRoomServiceCategories.AddAsync(customerRoomServiceCategory, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
		return customerRoomServiceCategory;
	}

	public async Task<List<CustomerGuestAppRoomServiceItem>> CustomersRoomServiceItemsAdd(List<CreateCustomerRoomServiceItemIn> request, int customerRoomServiceCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var cutomerRoomServiceitems = new List<CustomerGuestAppRoomServiceItem>();

		foreach (var item in request)
		{
			var roomServiceItem = new CustomerGuestAppRoomServiceItem
			{
				CustomerId = item.CustomerId,
				CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
				CustomerGuestAppRoomServiceCategoryId = customerRoomServiceCategoryId,
				Name = item.Name,
				ItemsMonth = item.ItemsMonth,
				ItemsDay = item.ItemsDay,
				ItemsMinute = item.ItemsMinute,
				ItemsHour = item.ItemsHour,
				QuantityBar = item.QuantityBar,
				ItemLocation = item.ItemLocation,
				Comment = item.Comment,
				IsPriceEnable = item.IsPriceEnable,
				Price = item.Price,
				Currency = item.Currency,
				DisplayOrder = item.DisplayOrder,
			};

			cutomerRoomServiceitems.Add(roomServiceItem);
		}

		await _db.CustomerGuestAppRoomServiceItems.AddRangeAsync(cutomerRoomServiceitems, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
		return cutomerRoomServiceitems;
	}
	#endregion

	#region CustomersRoomServiceDelete
	public async Task CustomersRoomServiceDelete(CustomerGuestAppRoomServiceCategory customerRoomServiceCategory, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		List<CustomerGuestAppRoomServiceItem> customerRoomServiceItems = _db.CustomerGuestAppRoomServiceItems.Where(e => e.CustomerGuestAppRoomServiceCategoryId == customerRoomServiceCategory.Id).ToList();

		if (customerRoomServiceItems.Count > 0)
		{
			_db.CustomerGuestAppRoomServiceItems.RemoveRange(customerRoomServiceItems);
		}

		_db.CustomerGuestAppRoomServiceCategories.Remove(customerRoomServiceCategory);
		await _db.SaveChangesAsync(cancellationToken);
	}

	#endregion

	#region CustomersRoomServiceCategoryUpdate
	public async Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceCategoryUpdate(CustomerGuestAppRoomServiceCategory existingData, UpdateCustomerRoomServiceCategoryIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		//existingData.CustomerId = request.CustomerId;
		existingData.CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId;
		existingData.CategoryName = request.CategoryName;
		await _db.SaveChangesAsync(cancellationToken);
		return existingData;
	}
	#endregion

	#region CustomersRoomServiceItemsUpdate
	public async Task<List<CustomerGuestAppRoomServiceItem>> CustomersRoomServiceItemsUpdate(List<UpdateCustomerRoomServiceItemIn> request, int customerRoomServiceCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var cutomerRoomServiceitems = new List<CustomerGuestAppRoomServiceItem>();

		foreach (var item in request)
		{
			if (item.Id > 0)
			{
				var updateCustomerRoomServiceItem = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);

				//if(updateCustomerRoomServiceItem is null)
				//{
				//    return _response.Error("No items availbale for given RoomService", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
				//}
				//updateCustomerRoomServiceItem.CustomerId = item.CustomerId;
				updateCustomerRoomServiceItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
				updateCustomerRoomServiceItem.CustomerGuestAppRoomServiceCategoryId = customerRoomServiceCategoryId;
				updateCustomerRoomServiceItem.Name = item.Name;
				updateCustomerRoomServiceItem.ItemsMonth = item.ItemsMonth;
				updateCustomerRoomServiceItem.ItemsDay = item.ItemsDay;
				updateCustomerRoomServiceItem.ItemsMinute = item.ItemsMinute;
				updateCustomerRoomServiceItem.ItemsHour = item.ItemsHour;
				updateCustomerRoomServiceItem.QuantityBar = item.QuantityBar;
				updateCustomerRoomServiceItem.ItemLocation = item.ItemLocation;
				updateCustomerRoomServiceItem.Comment = item.Comment;
				updateCustomerRoomServiceItem.IsPriceEnable = item.IsPriceEnable;
				updateCustomerRoomServiceItem.Price = item.Price;
				updateCustomerRoomServiceItem.Currency = item.Currency;

				cutomerRoomServiceitems.Add(updateCustomerRoomServiceItem);
			}
			else
			{
				var roomServiceItem = new CustomerGuestAppRoomServiceItem()
				{
					//CustomerId = item.CustomerId,
					CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
					CustomerGuestAppRoomServiceCategoryId = customerRoomServiceCategoryId,
					Name = item.Name,
					ItemsMonth = item.ItemsMonth,
					ItemsDay = item.ItemsDay,
					ItemsMinute = item.ItemsMinute,
					ItemsHour = item.ItemsHour,
					QuantityBar = item.QuantityBar,
					ItemLocation = item.ItemLocation,
					Comment = item.Comment,
					IsPriceEnable = item.IsPriceEnable,
					Price = item.Price,
					Currency = item.Currency
				};
				await _db.CustomerGuestAppRoomServiceItems.AddAsync(roomServiceItem, cancellationToken);
				cutomerRoomServiceitems.Add(roomServiceItem);
			}
		}


		await _db.SaveChangesAsync(cancellationToken);
		return cutomerRoomServiceitems;
	}
	#endregion

	#region NotificationsAdd
	public async Task<Notification> NotificationsAdd(CreateNotificationsIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var notification = new Notification()
		{
			Country = request.Country,
			City = request.City,
			Postalcode = request.Postalcode,
			BusinessTypeId = request.BusinessTypeId == 0 ? null : request.BusinessTypeId,
			ProductId = request.ProductId == 0 ? null : request.ProductId,
			Title = request.Title,
			Message = request.Message
		};
		await _db.Notifications.AddAsync(notification, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
		return notification;

	}
	#endregion

	#region CustomersConciergeDelete
	public async Task CustomersConciergeDelete(CustomerGuestAppConciergeCategory customerConciergeCategory, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		List<CustomerGuestAppConciergeItem> customerConciergeItems = _db.CustomerGuestAppConciergeItems.Where(e => e.CustomerGuestAppConciergeCategoryId == customerConciergeCategory.Id).ToList();

		if (customerConciergeItems.Count > 0)
		{
			_db.CustomerGuestAppConciergeItems.RemoveRange(customerConciergeItems);
		}

		_db.CustomerGuestAppConciergeCategories.Remove(customerConciergeCategory);
		await _db.SaveChangesAsync(cancellationToken);
	}
	#endregion

	#region CustomersConciergeItemsUpdateV2
	public async Task<List<CustomerGuestAppConciergeItem>> CustomersConciergeItemsUpdateV2(List<UpdateCustomerConciergeItemIn> request, int customerConciergeCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var cutomerConciergeItemsResponse = new List<CustomerGuestAppConciergeItem>();
		var cutomerConciergeNewItems = new List<CustomerGuestAppConciergeItem>();


		foreach (var item in request)
		{
			if (item.Id > 0)
			{
				var updateConciergeItem = await _db.CustomerGuestAppConciergeItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
				if (updateConciergeItem != null)
				{
					//updateConciergeItem.CustomerId = item.CustomerId;
					updateConciergeItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
					updateConciergeItem.CustomerGuestAppConciergeCategoryId = customerConciergeCategoryId;
					updateConciergeItem.Name = item.Name;
					updateConciergeItem.ItemsMonth = item.ItemsMonth;
					updateConciergeItem.ItemsDay = item.ItemsDay;
					updateConciergeItem.ItemsMinute = item.ItemsMinute;
					updateConciergeItem.ItemsHour = item.ItemsHour;
					updateConciergeItem.QuantityBar = item.QuantityBar;
					updateConciergeItem.ItemLocation = item.ItemLocation;
					updateConciergeItem.Comment = item.Comment;
					updateConciergeItem.IsPriceEnable = item.IsPriceEnable;
					updateConciergeItem.Price = item.Price;
					updateConciergeItem.Currency = item.Currency;
					cutomerConciergeItemsResponse.Add(updateConciergeItem);
				}


			}
			else
			{
				var conciergeNewItem = new CustomerGuestAppConciergeItem()
				{
					//CustomerId = item.CustomerId,
					CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
					CustomerGuestAppConciergeCategoryId = customerConciergeCategoryId,
					Name = item.Name,
					ItemsMonth = item.ItemsMonth,
					ItemsDay = item.ItemsDay,
					ItemsMinute = item.ItemsMinute,
					ItemsHour = item.ItemsHour,
					QuantityBar = item.QuantityBar,
					ItemLocation = item.ItemLocation,
					Comment = item.Comment,
					IsPriceEnable = item.IsPriceEnable,
					Price = item.Price,
					Currency = item.Currency
				};
				cutomerConciergeNewItems.Add(conciergeNewItem);
				//await _db.CustomerGuestAppConciergeItems.AddAsync(conciergeItem, cancellationToken);
				cutomerConciergeItemsResponse.Add(conciergeNewItem);
			}
		}
		if (cutomerConciergeNewItems.Any())
		{
			await _db.CustomerGuestAppConciergeItems.AddRangeAsync(cutomerConciergeNewItems, cancellationToken);
		}
		await _db.SaveChangesAsync(cancellationToken);
		return cutomerConciergeItemsResponse;
	}
	#endregion

	#region NotificationsHistoryAdd
	public async Task<List<NotificationHistory>> NotificationsHistoryAdd(CreateNotificationsIn request, ApplicationDbContext _db, List<UserNotification> GetUserForNotification, int Id, CancellationToken cancellationToken, int UserType)
	{
		List<NotificationHistory> notificationHistories = new List<NotificationHistory>();

		foreach (var item in GetUserForNotification)
		{
			NotificationHistory notificationHistory1 = new NotificationHistory();
			notificationHistory1.NotificationId = Id;
			notificationHistory1.UserId = item.UserId;
			notificationHistory1.IsActive = true;
			notificationHistory1.UserType = (byte)UserType;

			notificationHistories.Add(notificationHistory1);
		}

		await _db.NotificationHistorys.AddRangeAsync(notificationHistories, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
		return notificationHistories;
	}
	#endregion

	#region CustomersConciergeMultipleAddWithItems
	public async Task<List<CustomerGuestAppConciergeCategory>> CustomersConciergeMultipleAddWithItems(List<CreateCustomerConciergeCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerConciergeCategoriesResponse = new List<CustomerGuestAppConciergeCategory>();

		foreach (var category in request)
		{
			var customerConciergeCategory = new CustomerGuestAppConciergeCategory
			{
				CustomerId = category.CustomerId,
				CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
				CategoryName = category.CategoryName,
				DisplayOrder = category.DisplayOrder
			};
			await _db.CustomerGuestAppConciergeCategories.AddAsync(customerConciergeCategory, cancellationToken);
			customerConciergeCategoriesResponse.Add(customerConciergeCategory);
			await _db.SaveChangesAsync(cancellationToken);

			if (category.CustomerConciergeItems.Any())
			{
				var cutomerConciergeitems = new List<CustomerGuestAppConciergeItem>();
				foreach (var item in category.CustomerConciergeItems)
				{
					var conciergeItem = new CustomerGuestAppConciergeItem
					{
						CustomerId = item.CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppConciergeCategoryId = customerConciergeCategory.Id,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder,
					};
					cutomerConciergeitems.Add(conciergeItem);
				}
				await _db.CustomerGuestAppConciergeItems.AddRangeAsync(cutomerConciergeitems, cancellationToken);
				await _db.SaveChangesAsync(cancellationToken);
			}
		}
		return customerConciergeCategoriesResponse;
	}

	#endregion

	#region CustomersConciergeMultipleUpdateWithItems
	public async Task<CustomerGuestAppConciergeCategory> CustomersConciergeMultipleUpdateWithItems(UpdateCustomerConciergeCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerConciergeCategoriesResponse = new CustomerGuestAppConciergeCategory();

		// Do Not RemoVe This Code..

		//var getConciergeCategories = await _db.CustomerGuestAppConciergeCategories.Where(e => e.CustomerId == CustomerId).ToListAsync(cancellationToken);
		//foreach (var item in getConciergeCategories)
		//{
		//    var deleteConciergeCategories = request.Where(e => e.Id == item.Id).ToList();
		//    if (deleteConciergeCategories == null || deleteConciergeCategories.Count == 0)
		//    {
		//        _db.CustomerGuestAppConciergeCategories.Remove(item);
		//    }
		//}
		//foreach (var category in request)
		//{
		var categoryId = request.Id;
		if (categoryId > 0)
		{
			var updateConciergeCategory = await _db.CustomerGuestAppConciergeCategories.Where(e => e.Id == categoryId).FirstOrDefaultAsync(cancellationToken);
			if (updateConciergeCategory != null)
			{
				//updateConciergeCategory.CustomerId = CustomerId;
				//updateConciergeCategory.CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId;
				//updateConciergeCategory.CategoryName = request.CategoryName;
				//updateConciergeCategory.DisplayOrder = request.DisplayOrder;
				//updateConciergeCategory.IsActive = request.IsActive;
				if (request.IsDeleted == null)
				{
					request.IsDeleted = false;
				}
				updateConciergeCategory.IsPublish = updateConciergeCategory.IsPublish;
				updateConciergeCategory.JsonData = JsonConvert.SerializeObject(request);
				//customerConciergeCategoriesResponse.Add(updateConciergeCategory);
				await _db.SaveChangesAsync(cancellationToken);
				categoryId = updateConciergeCategory.Id;
				customerConciergeCategoriesResponse = updateConciergeCategory;
			}
		}
		else
		{
			var customerConciergeCategory = new CustomerGuestAppConciergeCategory
			{
				CustomerId = CustomerId,
				CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
				CategoryName = request.CategoryName,
				DisplayOrder = request.DisplayOrder,
				IsActive = request.IsActive,
				IsPublish = false,
				JsonData = null
			};
			await _db.CustomerGuestAppConciergeCategories.AddAsync(customerConciergeCategory, cancellationToken);
			//customerConciergeCategoriesResponse.Add(customerConciergeCategory);
			await _db.SaveChangesAsync(cancellationToken);
			customerConciergeCategoriesResponse = customerConciergeCategory;
			categoryId = customerConciergeCategory.Id;

		}

		if (request.CustomerConciergeItems.Any())
		{

			// Do Not RemoVe This Code..

			var cutomerConciergeNewItems = new List<CustomerGuestAppConciergeItem>();
			var getConciergeCategoryItem = await _db.CustomerGuestAppConciergeItems.Where(e => e.CustomerGuestAppConciergeCategoryId == categoryId).ToListAsync(cancellationToken);
			foreach (var item in getConciergeCategoryItem)
			{
				var deleteConciergeCategoryItem = request.CustomerConciergeItems.Where(e => e.Id == item.Id).ToList();
				if (deleteConciergeCategoryItem == null || deleteConciergeCategoryItem.Count == 0)
				{
					_db.CustomerGuestAppConciergeItems.Remove(item);
				}
			}
			foreach (var item in request.CustomerConciergeItems)
			{
				if (item.Id > 0)
				{
					var updateConciergeItem = await _db.CustomerGuestAppConciergeItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
					if (updateConciergeItem != null)
					{
						//updateConciergeItem.CustomerId = CustomerId;
						//updateConciergeItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
						//updateConciergeItem.CustomerGuestAppConciergeCategoryId = categoryId;
						//updateConciergeItem.Name = item.Name;
						//updateConciergeItem.ItemsMonth = item.ItemsMonth;
						//updateConciergeItem.ItemsDay = item.ItemsDay;
						//updateConciergeItem.ItemsMinute = item.ItemsMinute;
						//updateConciergeItem.ItemsHour = item.ItemsHour;
						//updateConciergeItem.QuantityBar = item.QuantityBar;
						//updateConciergeItem.ItemLocation = item.ItemLocation;
						//updateConciergeItem.Comment = item.Comment;
						//updateConciergeItem.IsPriceEnable = item.IsPriceEnable;
						//updateConciergeItem.Price = item.Price;
						//updateConciergeItem.Currency = item.Currency;
						//updateConciergeItem.DisplayOrder = item.DisplayOrder;
						//updateConciergeItem.IsActive = item.IsActive;
						if (item.IsDeleted == null)
						{
							item.IsDeleted = false;
						}
						updateConciergeItem.IsPublish = updateConciergeItem.IsPublish;
						updateConciergeItem.JsonData = JsonConvert.SerializeObject(item);
					}
				}
				else
				{
					var conciergeNewItem = new CustomerGuestAppConciergeItem()
					{
						CustomerId = CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppConciergeCategoryId = categoryId,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder,
						IsActive = item.IsActive,
						IsPublish = false,
						JsonData = null
					};
					cutomerConciergeNewItems.Add(conciergeNewItem);
				}
			}
			if (cutomerConciergeNewItems.Any())
			{
				await _db.CustomerGuestAppConciergeItems.AddRangeAsync(cutomerConciergeNewItems, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		//}

		return customerConciergeCategoriesResponse;
	}
	#endregion

	#region CustomersHouseKeepingMultipleAddWithItems
	public async Task<List<CustomerGuestAppHousekeepingCategory>> CustomersHouseKeepingMultipleAddWithItems(List<CreateCustomerHouseKeepingCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerHousekeepingCategoriesResponse = new List<CustomerGuestAppHousekeepingCategory>();

		foreach (var category in request)
		{
			var customerHousekeepingCategory = new CustomerGuestAppHousekeepingCategory
			{
				CustomerId = category.CustomerId,
				CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
				CategoryName = category.CategoryName,
				DisplayOrder = category.DisplayOrder
			};
			await _db.CustomerGuestAppHousekeepingCategories.AddAsync(customerHousekeepingCategory, cancellationToken);
			customerHousekeepingCategoriesResponse.Add(customerHousekeepingCategory);
			await _db.SaveChangesAsync(cancellationToken);

			if (category.CustomerHouseKeepingItems.Any())
			{
				var cutomerHousekeepingitems = new List<CustomerGuestAppHousekeepingItem>();
				foreach (var item in category.CustomerHouseKeepingItems)
				{
					var conciergeItem = new CustomerGuestAppHousekeepingItem
					{
						CustomerId = item.CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppHousekeepingCategoryId = customerHousekeepingCategory.Id,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder
					};
					cutomerHousekeepingitems.Add(conciergeItem);
				}
				await _db.CustomerGuestAppHousekeepingItems.AddRangeAsync(cutomerHousekeepingitems, cancellationToken);
				await _db.SaveChangesAsync(cancellationToken);
			}
		}

		return customerHousekeepingCategoriesResponse;
	}
	#endregion

	#region CustomersHouseKeepingMultipleUpdateWithItems
	public async Task<CustomerGuestAppHousekeepingCategory> CustomersHouseKeepingMultipleUpdateWithItems(UpdateCustomerHouseKeepingCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerHousekeepingCategoriesResponse = new CustomerGuestAppHousekeepingCategory();
		//var getHousekeepingCategories = await _db.CustomerGuestAppHousekeepingCategories.Where(e => e.CustomerId == CustomerId).ToListAsync(cancellationToken);
		//foreach (var item in getHousekeepingCategories)
		//{
		//    var deleteHousekeepingCategories = request.Where(e => e.Id == item.Id).ToList();
		//    if (deleteHousekeepingCategories == null || deleteHousekeepingCategories.Count == 0)
		//    {
		//        _db.CustomerGuestAppHousekeepingCategories.Remove(item);
		//    }
		//}
		//foreach (var category in request)
		//{
		var categoryId = request.Id;
		if (categoryId > 0)
		{
			var updateHousekeepingCategory = await _db.CustomerGuestAppHousekeepingCategories.Where(e => e.Id == categoryId).FirstOrDefaultAsync(cancellationToken);
			if (updateHousekeepingCategory != null)
			{
				//updateHousekeepingCategory.CustomerId = CustomerId;
				//updateHousekeepingCategory.CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId;
				//updateHousekeepingCategory.CategoryName = category.CategoryName;
				//updateHousekeepingCategory.DisplayOrder = category.DisplayOrder;
				//updateHousekeepingCategory.IsActive = category.IsActive;
				updateHousekeepingCategory.IsPublish = updateHousekeepingCategory.IsPublish;
				updateHousekeepingCategory.JsonData = JsonConvert.SerializeObject(request);
				//customerHousekeepingCategoriesResponse.Add(updateHousekeepingCategory);
				await _db.SaveChangesAsync(cancellationToken);
				categoryId = updateHousekeepingCategory.Id;
			}
		}
		else
		{
			var customerHousekeepingCategory = new CustomerGuestAppHousekeepingCategory
			{
				CustomerId = CustomerId,
				CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
				CategoryName = request.CategoryName,
				DisplayOrder = request.DisplayOrder,
				IsActive = request.IsActive,
				IsPublish = false,
				JsonData = null
			};
			await _db.CustomerGuestAppHousekeepingCategories.AddAsync(customerHousekeepingCategory, cancellationToken);
			//customerHousekeepingCategoriesResponse.Add(customerHousekeepingCategory);
			await _db.SaveChangesAsync(cancellationToken);
			categoryId = customerHousekeepingCategory.Id;
		}


		if (request.CustomerHouseKeepingItems.Any())
		{
			var cutomerHousekeepingNewItems = new List<CustomerGuestAppHousekeepingItem>();
			var getHousekeepingCategoryItem = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.CustomerGuestAppHousekeepingCategoryId == categoryId).ToListAsync(cancellationToken);
			foreach (var item in getHousekeepingCategoryItem)
			{
				var deleteHousekeepingCategoryItem = request.CustomerHouseKeepingItems.Where(e => e.Id == item.Id).ToList();
				if (deleteHousekeepingCategoryItem == null || deleteHousekeepingCategoryItem.Count == 0)
				{
					_db.CustomerGuestAppHousekeepingItems.Remove(item);
				}
			}
			foreach (var item in request.CustomerHouseKeepingItems)
			{
				if (item.Id > 0)
				{
					var updateHousekeepingItem = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
					if (updateHousekeepingItem != null)
					{
						//updateHousekeepingItem.CustomerId = CustomerId;
						//updateHousekeepingItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
						//updateHousekeepingItem.CustomerGuestAppHousekeepingCategoryId = categoryId;
						//updateHousekeepingItem.Name = item.Name;
						//updateHousekeepingItem.ItemsMonth = item.ItemsMonth;
						//updateHousekeepingItem.ItemsDay = item.ItemsDay;
						//updateHousekeepingItem.ItemsMinute = item.ItemsMinute;
						//updateHousekeepingItem.ItemsHour = item.ItemsHour;
						//updateHousekeepingItem.QuantityBar = item.QuantityBar;
						//updateHousekeepingItem.ItemLocation = item.ItemLocation;
						//updateHousekeepingItem.Comment = item.Comment;
						//updateHousekeepingItem.IsPriceEnable = item.IsPriceEnable;
						//updateHousekeepingItem.Price = item.Price;
						//updateHousekeepingItem.Currency = item.Currency;
						//updateHousekeepingItem.DisplayOrder = item.DisplayOrder;
						//updateHousekeepingItem.IsActive = item.IsActive;
						updateHousekeepingItem.IsPublish = updateHousekeepingItem.IsPublish;
						updateHousekeepingItem.JsonData = JsonConvert.SerializeObject(item);
					}
				}
				else
				{
					var houseKeepingNewItem = new CustomerGuestAppHousekeepingItem()
					{
						CustomerId = CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppHousekeepingCategoryId = categoryId,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder,
						IsActive = item.IsActive,
						IsPublish = false,
						JsonData = null
					};
					cutomerHousekeepingNewItems.Add(houseKeepingNewItem);
				}
			}
			if (cutomerHousekeepingNewItems.Any())
			{
				await _db.CustomerGuestAppHousekeepingItems.AddRangeAsync(cutomerHousekeepingNewItems, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		//}
		return customerHousekeepingCategoriesResponse;
	}
	#endregion

	#region CustomersReceptionMultipleAddWithItems
	public async Task<List<CustomerGuestAppReceptionCategory>> CustomersReceptionMultipleAddWithItems(List<CreateCustomerReceptionCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerReceptionCategoriesResponse = new List<CustomerGuestAppReceptionCategory>();

		foreach (var category in request)
		{
			var customerReceptionCategory = new CustomerGuestAppReceptionCategory
			{
				CustomerId = category.CustomerId,
				CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
				CategoryName = category.CategoryName,
				DisplayOrder = category.DisplayOrder
			};
			await _db.CustomerGuestAppReceptionCategories.AddAsync(customerReceptionCategory, cancellationToken);
			customerReceptionCategoriesResponse.Add(customerReceptionCategory);
			await _db.SaveChangesAsync(cancellationToken);

			if (category.CustomerReceptionItems.Any())
			{
				var cutomerReceptionitems = new List<CustomerGuestAppReceptionItem>();
				foreach (var item in category.CustomerReceptionItems)
				{
					var receptionItem = new CustomerGuestAppReceptionItem
					{
						CustomerId = item.CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppReceptionCategoryId = customerReceptionCategory.Id,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder
					};
					cutomerReceptionitems.Add(receptionItem);
				}
				await _db.CustomerGuestAppReceptionItems.AddRangeAsync(cutomerReceptionitems, cancellationToken);
				await _db.SaveChangesAsync(cancellationToken);
			}
		}
		return customerReceptionCategoriesResponse;
	}
	#endregion

	#region CustomersReceptionMultipleUpdateWithItems
	public async Task<CustomerGuestAppReceptionCategory> CustomersReceptionMultipleUpdateWithItems(UpdateCustomerReceptionCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerReceptionCategoriesResponse = new CustomerGuestAppReceptionCategory();

		//var getReceptionCategory = await _db.CustomerGuestAppReceptionCategories.Where(e => e.CustomerId == CustomerId).ToListAsync(cancellationToken);
		//foreach (var item in getReceptionCategory)
		//{
		//    var deleteReceptionCategory = request.Where(e => e.Id == item.Id).ToList();
		//    if (deleteReceptionCategory == null || deleteReceptionCategory.Count == 0)
		//    {
		//        _db.CustomerGuestAppReceptionCategories.Remove(item);
		//    }
		//}
		//foreach (var category in request)
		//{
		var categoryId = request.Id;
		if (categoryId > 0)
		{
			var updateReceptionCategory = await _db.CustomerGuestAppReceptionCategories.Where(e => e.Id == categoryId).FirstOrDefaultAsync(cancellationToken);
			if (updateReceptionCategory != null)
			{
				//updateReceptionCategory.CustomerId = CustomerId;
				//updateReceptionCategory.CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId;
				//updateReceptionCategory.CategoryName = category.CategoryName;
				//updateReceptionCategory.DisplayOrder = category.DisplayOrder;
				//updateReceptionCategory.IsActive = category.IsActive;
				updateReceptionCategory.IsPublish = updateReceptionCategory.IsPublish;
				updateReceptionCategory.JsonData = JsonConvert.SerializeObject(request);
				//customerReceptionCategoriesResponse.Add(updateReceptionCategory);
				await _db.SaveChangesAsync(cancellationToken);
				categoryId = updateReceptionCategory.Id;
			}
		}
		else
		{
			var customerReceptionCategory = new CustomerGuestAppReceptionCategory
			{
				CustomerId = CustomerId,
				CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
				CategoryName = request.CategoryName,
				DisplayOrder = request.DisplayOrder,
				IsActive = request.IsActive,
				IsPublish = false,
				JsonData = null
			};
			await _db.CustomerGuestAppReceptionCategories.AddAsync(customerReceptionCategory, cancellationToken);
			//customerReceptionCategoriesResponse.Add(customerReceptionCategory);
			await _db.SaveChangesAsync(cancellationToken);
			categoryId = customerReceptionCategory.Id;

		}

		if (request.CustomerReceptionItems.Any())
		{
			var getReceptionCategoryItem = await _db.CustomerGuestAppReceptionItems.Where(e => e.CustomerId == CustomerId && e.CustomerGuestAppReceptionCategoryId == categoryId).ToListAsync(cancellationToken);
			var customerReceptionCategoriesResponseItem = new List<CustomerGuestAppReceptionCategory>();
			foreach (var item in getReceptionCategoryItem)
			{
				var deleteReceptionCategoryItem = request.CustomerReceptionItems.Where(e => e.Id == item.Id).ToList();
				if (deleteReceptionCategoryItem == null || deleteReceptionCategoryItem.Count == 0)
				{
					_db.CustomerGuestAppReceptionItems.Remove(item);
				}
			}
			var cutomerReceptionNewItems = new List<CustomerGuestAppReceptionItem>();
			foreach (var item in request.CustomerReceptionItems)
			{
				if (item.Id > 0)
				{
					var updateReceptionItem = await _db.CustomerGuestAppReceptionItems.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
					if (updateReceptionItem != null)
					{
						//updateReceptionItem.CustomerId = CustomerId;
						//updateReceptionItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
						//updateReceptionItem.CustomerGuestAppReceptionCategoryId = categoryId;
						//updateReceptionItem.Name = item.Name;
						//updateReceptionItem.ItemsMonth = item.ItemsMonth;
						//updateReceptionItem.ItemsDay = item.ItemsDay;
						//updateReceptionItem.ItemsMinute = item.ItemsMinute;
						//updateReceptionItem.ItemsHour = item.ItemsHour;
						//updateReceptionItem.QuantityBar = item.QuantityBar;
						//updateReceptionItem.ItemLocation = item.ItemLocation;
						//updateReceptionItem.Comment = item.Comment;
						//updateReceptionItem.IsPriceEnable = item.IsPriceEnable;
						//updateReceptionItem.Price = item.Price;
						//updateReceptionItem.Currency = item.Currency;
						//updateReceptionItem.DisplayOrder = item.DisplayOrder;
						//updateReceptionItem.IsActive = item.IsActive;
						updateReceptionItem.IsPublish = updateReceptionItem.IsPublish;
						updateReceptionItem.JsonData = JsonConvert.SerializeObject(item);
					}
				}
				else
				{
					var receptionNewItem = new CustomerGuestAppReceptionItem()
					{
						CustomerId = CustomerId,
						CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
						CustomerGuestAppReceptionCategoryId = categoryId,
						Name = item.Name,
						ItemsMonth = item.ItemsMonth,
						ItemsDay = item.ItemsDay,
						ItemsMinute = item.ItemsMinute,
						ItemsHour = item.ItemsHour,
						QuantityBar = item.QuantityBar,
						ItemLocation = item.ItemLocation,
						Comment = item.Comment,
						IsPriceEnable = item.IsPriceEnable,
						Price = item.Price,
						Currency = item.Currency,
						DisplayOrder = item.DisplayOrder,
						IsActive = item.IsActive,
						IsPublish = false,
						JsonData = null
					};
					cutomerReceptionNewItems.Add(receptionNewItem);
				}
			}
			if (cutomerReceptionNewItems.Any())
			{
				await _db.CustomerGuestAppReceptionItems.AddRangeAsync(cutomerReceptionNewItems, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		//}

		return customerReceptionCategoriesResponse;
	}
	#endregion

	#region RoomService 
	public async Task<List<CreateCustomerRoomServiceCategoryIn>> CustomerRoomServiceMultipleAddWithItems(CreateCustomerRoomServiceIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var createCustomerRoomServiceCategoryRes = new List<CreateCustomerRoomServiceCategoryIn>();

		foreach (var category in request.CustomerRoomServiceCategories)
		{
			var customerRoomServiceCategory = new CustomerGuestAppRoomServiceCategory
			{
				CustomerId = category.CustomerId,
				CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
				CategoryName = category.CategoryName,
				DisplayOrder = category.DisplayOrder
			};
			await _db.CustomerGuestAppRoomServiceCategories.AddAsync(customerRoomServiceCategory, cancellationToken);
			createCustomerRoomServiceCategoryRes.Add(category);
			await _db.SaveChangesAsync(cancellationToken);

			var cutomerRoomServiceitems = new List<CustomerGuestAppRoomServiceItem>();

			foreach (var item in category.CustomerRoomServiceItems)
			{
				var roomServiceItem = new CustomerGuestAppRoomServiceItem
				{
					CustomerId = item.CustomerId,
					CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
					CustomerGuestAppRoomServiceCategoryId = customerRoomServiceCategory.Id,
					Name = item.Name,
					ItemsMonth = item.ItemsMonth,
					ItemsDay = item.ItemsDay,
					ItemsMinute = item.ItemsMinute,
					ItemsHour = item.ItemsHour,
					QuantityBar = item.QuantityBar,
					ItemLocation = item.ItemLocation,
					Comment = item.Comment,
					IsPriceEnable = item.IsPriceEnable,
					Price = item.Price,
					Currency = item.Currency,
					DisplayOrder = item.DisplayOrder,
				};
				cutomerRoomServiceitems.Add(roomServiceItem);
			}
			await _db.CustomerGuestAppRoomServiceItems.AddRangeAsync(cutomerRoomServiceitems, cancellationToken);
		}

		await _db.SaveChangesAsync(cancellationToken);
		return createCustomerRoomServiceCategoryRes;
	}


	public async Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceMultipleUpdateWithItems(UpdateCustomerRoomServiceCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var upCustRoomServiceCategoryIn = new CustomerGuestAppRoomServiceCategory();
		//var getRoomServiceCategory = await _db.CustomerGuestAppRoomServiceCategories.Where(e => e.CustomerId == CustomerId).ToListAsync(cancellationToken);
		//foreach (var item in getRoomServiceCategory)
		//{
		//    var deleteRoomServiceCategory = request.Where(e => e.Id == item.Id).ToList();
		//    if (deleteRoomServiceCategory == null || deleteRoomServiceCategory.Count == 0)
		//    {
		//        _db.CustomerGuestAppRoomServiceCategories.Remove(item);
		//    }
		//}
		//foreach (var category in request)
		//{
		var categoryId = request.Id;
		if (categoryId > 0)
		{
			var updateRoomServiceCategory = await _db.CustomerGuestAppRoomServiceCategories.Where(e => e.Id == categoryId).SingleOrDefaultAsync(cancellationToken);
			if (updateRoomServiceCategory != null)
			{
				//updateRoomServiceCategory.CustomerId = CustomerId;
				//updateRoomServiceCategory.CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId;
				//updateRoomServiceCategory.CategoryName = category.CategoryName;
				//updateRoomServiceCategory.DisplayOrder = category.DisplayOrder;
				//updateRoomServiceCategory.IsActive = category.IsActive;
				updateRoomServiceCategory.IsPublish = updateRoomServiceCategory.IsPublish;
				updateRoomServiceCategory.JsonData = JsonConvert.SerializeObject(request);
				await _db.SaveChangesAsync(cancellationToken);
				categoryId = updateRoomServiceCategory.Id;
			}
		}
		else
		{
			var customerGuestAppRoomServiceCategory = new CustomerGuestAppRoomServiceCategory
			{
				CustomerId = CustomerId,
				CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
				CategoryName = request.CategoryName,
				DisplayOrder = request.DisplayOrder,
				IsActive = request.IsActive,
				IsPublish = false,
				JsonData = null
			};
			await _db.CustomerGuestAppRoomServiceCategories.AddAsync(customerGuestAppRoomServiceCategory, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
			categoryId = customerGuestAppRoomServiceCategory.Id;
			//upCustRoomServiceCategoryIn.Add(customerGuestAppRoomServiceCategory);
		}

		var cutomerRoomServiceNewItems = new List<CustomerGuestAppRoomServiceItem>();

		foreach (var item in request.CustomerRoomServiceItems)
		{
			var getRoomServiceCategoryItem = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.CustomerGuestAppRoomServiceCategoryId == categoryId).ToListAsync(cancellationToken);
			foreach (var CategoryItem in getRoomServiceCategoryItem)
			{
				var deleteRoomServiceCategoryItem = request.CustomerRoomServiceItems.Where(e => e.Id == CategoryItem.Id).ToList();
				if (deleteRoomServiceCategoryItem == null || deleteRoomServiceCategoryItem.Count == 0)
				{
					_db.CustomerGuestAppRoomServiceItems.Remove(CategoryItem);
				}
			}
			if (item.Id > 0)
			{
				var updateRoomServiceItem = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.Id == item.Id).SingleOrDefaultAsync(cancellationToken);
				if (updateRoomServiceItem != null)
				{
					//updateRoomServiceItem.CustomerId = CustomerId;
					//updateRoomServiceItem.CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId;
					//updateRoomServiceItem.CustomerGuestAppRoomServiceCategoryId = categoryId;
					//updateRoomServiceItem.Name = item.Name;
					//updateRoomServiceItem.ItemsMonth = item.ItemsMonth;
					//updateRoomServiceItem.ItemsDay = item.ItemsDay;
					//updateRoomServiceItem.ItemsMinute = item.ItemsMinute;
					//updateRoomServiceItem.ItemsHour = item.ItemsHour;
					//updateRoomServiceItem.QuantityBar = item.QuantityBar;
					//updateRoomServiceItem.ItemLocation = item.ItemLocation;
					//updateRoomServiceItem.Comment = item.Comment;
					//updateRoomServiceItem.IsPriceEnable = item.IsPriceEnable;
					//updateRoomServiceItem.Price = item.Price;
					//updateRoomServiceItem.Currency = item.Currency;
					//updateRoomServiceItem.DisplayOrder = item.DisplayOrder;
					//updateRoomServiceItem.IsActive = item.IsActive;
					updateRoomServiceItem.IsPublish = updateRoomServiceItem.IsPublish;
					updateRoomServiceItem.JsonData = JsonConvert.SerializeObject(item);
				}
			}
			else
			{
				var roomServiceNewItem = new CustomerGuestAppRoomServiceItem()
				{
					CustomerId = CustomerId,
					CustomerGuestAppBuilderId = item.CustomerGuestAppBuilderId,
					CustomerGuestAppRoomServiceCategoryId = categoryId,
					Name = item.Name,
					ItemsMonth = item.ItemsMonth,
					ItemsDay = item.ItemsDay,
					ItemsMinute = item.ItemsMinute,
					ItemsHour = item.ItemsHour,
					QuantityBar = item.QuantityBar,
					ItemLocation = item.ItemLocation,
					Comment = item.Comment,
					IsPriceEnable = item.IsPriceEnable,
					Price = item.Price,
					Currency = item.Currency,
					DisplayOrder = item.DisplayOrder,
					IsActive = item.IsActive,
					IsPublish = false,
					JsonData = null
				};
				cutomerRoomServiceNewItems.Add(roomServiceNewItem);
			}
		}
		if (cutomerRoomServiceNewItems.Any())
		{
			await _db.CustomerGuestAppRoomServiceItems.AddRangeAsync(cutomerRoomServiceNewItems, cancellationToken);
		}
		await _db.SaveChangesAsync(cancellationToken);
		//}

		return upCustRoomServiceCategoryIn;
	}

	#endregion

	#region CustomerPropertyServiceImage And CustomerPropertyService
	public async Task<CustomerPropertyService> CustomerPropertyServiceAdd(CreateCustomerPropertyServiceIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerPropertService = new CustomerPropertyService();
		if (request.Id > 0)
		{
			customerPropertService = await _db.CustomerPropertyServices.Where(e => e.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
			if (customerPropertService != null)
			{
				//customerPropertService.CustomerPropertyInformationId = request.CustomerPropertyInformationId;
				//customerPropertService.Name = request.Name;
				//customerPropertService.Icon = request.Icon;
				//customerPropertService.Description = request.Description;
				//customerPropertService.IsActive = request.IsActive;
				customerPropertService.IsPublish = customerPropertService.IsPublish;
				customerPropertService.JsonData = JsonConvert.SerializeObject(request);
			}
		}
		else
		{
			customerPropertService = new CustomerPropertyService
			{
				CustomerPropertyInformationId = request.CustomerPropertyInformationId,
				Description = request.Description,
				Icon = request.Icon,
				Name = request.Name,
				IsActive = true,
				IsPublish = false,
				JsonData = null
			};
			if (customerPropertService != null)
			{
				await _db.CustomerPropertyServices.AddAsync(customerPropertService, cancellationToken);
			}
		}
		await _db.SaveChangesAsync(cancellationToken);
		return customerPropertService;
	}

	public async Task<List<CustomerPropertyServiceImage>> CustomerPropertyServiceImageAdd(List<CustomerPropertyServiceImageIn> request, int Id, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerPropertyServiceImageResponse = new List<CustomerPropertyServiceImage>();
		var customerPropertyServiceImage = new List<CustomerPropertyServiceImage>();

		foreach (var item in request)
		{
			//var serviceImages = await _db.CustomerPropertyServiceImages.Where(e => e.CustomerPropertyServiceId == Id).ToListAsync(cancellationToken);
			//if (serviceImages != null)
			//{
			//    while (serviceImages.Count > 0)
			//    {
			//        var sk = serviceImages.Last();
			//        serviceImages.Remove(sk);
			//        _db.CustomerPropertyServiceImages.Remove(sk);
			//    }
			//}
			//var customerPropertyserviceImage = new CustomerPropertyServiceImage()
			//{
			//    CustomerPropertyServiceId = Id,
			//    ServiceImages = item.ServiceImages,
			//    IsActive = true
			//};
			//customerPropertyServiceImage.Add(customerPropertyserviceImage);
			//customerPropertyServiceImageResponse.Add(customerPropertyserviceImage);
			if (item.Id > 0)
			{
				var serviceImage = await _db.CustomerPropertyServiceImages.Where(e => e.Id == item.Id).FirstOrDefaultAsync(cancellationToken);
				if (serviceImage != null)
				{
					serviceImage.IsPublish = serviceImage.IsPublish;
					serviceImage.JsonData = JsonConvert.SerializeObject(item);
				}
				customerPropertyServiceImageResponse.Add(serviceImage);
			}
			else
			{
				var customerPropertyserviceImage = new CustomerPropertyServiceImage()
				{
					CustomerPropertyServiceId = Id,
					ServiceImages = item.ServiceImages,
					IsActive = true,
					IsPublish = false,
					JsonData = null
				};
				await _db.CustomerPropertyServiceImages.AddAsync(customerPropertyserviceImage, cancellationToken);
				customerPropertyServiceImageResponse.Add(customerPropertyserviceImage);
			}

		}
		//if (customerPropertyServiceImage.Any())
		//{
		//    await _db.CustomerPropertyServiceImages.AddRangeAsync(customerPropertyServiceImage, cancellationToken);
		//}

		await _db.SaveChangesAsync(cancellationToken);
		return customerPropertyServiceImageResponse;
	}
	#endregion

	#region CustomersEnhanceYourStayCategoryItemExtraAdd
	public async Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersEnhanceYourStayCategoryItemExtraAdd(List<CreateEnhanceYourStayCategoryItemExtraIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerGuestAppEnhanceYourStayCategoryItems = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
		var customerGuests = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
		if (request != null)
		{
			foreach (var nestIn in request)
			{
				var customerGuest = new CustomerGuestAppEnhanceYourStayCategoryItemsExtra()
				{
					CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId,
					QueType = nestIn.QueType,
					Questions = nestIn.Questions,
					OptionValues = nestIn.OptionValues,
					IsActive = true,
					IsPublish = false,
					JsonData = null
				};
				customerGuests.Add(customerGuest);
				customerGuestAppEnhanceYourStayCategoryItems.Add(customerGuest);
			}
			if (customerGuests.Any())
			{
				await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.AddRangeAsync(customerGuests, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		return customerGuestAppEnhanceYourStayCategoryItems;
	}
	#endregion

	#region CustomersEnhanceYourStayCategoryItemExtraUpdate
	public async Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersEnhanceYourStayCategoryItemExtraUpdate(List<UpdateEnhanceYourStayCategoryItemExtraIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerGuestAppEnhanceYourStayCategoryItems = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
		var customerGuests = new List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();

		var getEnhanceYourStayCategoryItemsExtras = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(e => e.CustomerGuestAppEnhanceYourStayItemId == CustomerGuestAppEnhanceYourStayItemId).ToListAsync(cancellationToken);
		foreach (var item in getEnhanceYourStayCategoryItemsExtras)
		{
			var deleteEnhanceYourStayCategoryItemsExtras = request.Where(e => e.Id == item.Id).ToList();
			if (deleteEnhanceYourStayCategoryItemsExtras == null || deleteEnhanceYourStayCategoryItemsExtras.Count == 0)
			{
				_db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Remove(item);
			}
		}

		foreach (var nestIn in request)
		{
			if (nestIn.Id > 0)
			{
				var obj = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(e => e.Id == nestIn.Id).FirstOrDefaultAsync(cancellationToken);

				if (obj != null)
				{
					//obj.CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId;
					//obj.QueType = nestIn.QueType;
					//obj.Questions = nestIn.Questions;
					//obj.OptionValues = nestIn.OptionValues;

					obj.IsPublish = nestIn.IsPublish;
					obj.JsonData = JsonConvert.SerializeObject(nestIn);

					customerGuestAppEnhanceYourStayCategoryItems.Add(obj);
				}
			}
			else
			{
				var customerGuest = new CustomerGuestAppEnhanceYourStayCategoryItemsExtra()
				{
					CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId,
					QueType = nestIn.QueType,
					Questions = nestIn.Questions,
					OptionValues = nestIn.OptionValues,
					IsActive = true,
					IsPublish = false,
					JsonData = null
				};
				customerGuests.Add(customerGuest);
				customerGuestAppEnhanceYourStayCategoryItems.Add(customerGuest);
			}
		}
		if (customerGuests.Any())
		{
			await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.AddRangeAsync(customerGuests, cancellationToken);
		}
		await _db.SaveChangesAsync(cancellationToken);

		return customerGuestAppEnhanceYourStayCategoryItems;
	}
	#endregion

	#region CustomersEnhanceYourStayItemImageAdd
	public async Task<List<CustomerGuestAppEnhanceYourStayItemsImage>> CustomersEnhanceYourStayItemImageAdd(List<CreateEnhanceYourStayItemAttachementIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var stayImgIn = request;
		List<CustomerGuestAppEnhanceYourStayItemsImage> enhanceYourStayImagesResponse = new List<CustomerGuestAppEnhanceYourStayItemsImage>();
		List<CustomerGuestAppEnhanceYourStayItemsImage> listOfStayItems = new();
		var existingItems = await _db.CustomerGuestAppEnhanceYourStayItemsImages
   .Where(e => e.CustomerGuestAppEnhanceYourStayItemId == CustomerGuestAppEnhanceYourStayItemId)
   .ToListAsync(cancellationToken);
		if (existingItems.Any())
		{
			foreach (var existingItem in existingItems)
			{
				_db.CustomerGuestAppEnhanceYourStayItemsImages.Remove(existingItem);
				await _db.SaveChangesAsync(cancellationToken);
			}
		}
		if (stayImgIn.Any())
		{
			foreach (var attachment in stayImgIn)
			{
				var EnhanceYourStayItemImage = new CustomerGuestAppEnhanceYourStayItemsImage
				{
					CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId,
					ItemsImages = attachment.ItemsImage,
					DisaplayOrder = attachment.DisplayOrder,
					IsActive = true,
					IsPublish = false,
					JsonData = null
				};
				listOfStayItems.Add(EnhanceYourStayItemImage);
				enhanceYourStayImagesResponse.Add(EnhanceYourStayItemImage);
			}
			await _db.CustomerGuestAppEnhanceYourStayItemsImages.AddRangeAsync(listOfStayItems, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
		}
		return enhanceYourStayImagesResponse;
	}
	#endregion

	#region CustomersEnhanceYourStayItemImageUpdate
	public async Task<List<CustomerGuestAppEnhanceYourStayItemsImage>> CustomersEnhanceYourStayItemImageUpdate(List<UpdateEnhanceYourStayItemAttachementIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var stayImgIn = request;
		List<CustomerGuestAppEnhanceYourStayItemsImage> enhanceYourStayImagesResponse = new List<CustomerGuestAppEnhanceYourStayItemsImage>();
		List<CustomerGuestAppEnhanceYourStayItemsImage> listOfStayItems = new();

		var existingItems = await _db.CustomerGuestAppEnhanceYourStayItemsImages
	.Where(e => e.CustomerGuestAppEnhanceYourStayItemId == CustomerGuestAppEnhanceYourStayItemId)
	.ToListAsync(cancellationToken);

		if (existingItems.Any())
		{
			foreach (var existingItem in existingItems)
			{
				_db.CustomerGuestAppEnhanceYourStayItemsImages.Remove(existingItem);
				await _db.SaveChangesAsync(cancellationToken);
			}
		}

		if (stayImgIn.Any())
		{
			foreach (var attachment in stayImgIn)
			{
				if (attachment.Id > 0)
				{
					var updateItemImage = await _db.CustomerGuestAppEnhanceYourStayItemsImages.Where(e => e.Id == attachment.Id).FirstOrDefaultAsync(cancellationToken);
					if (updateItemImage != null)
					{
						//updateItemImage.CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId;
						//updateItemImage.ItemsImages = attachment.ItemsImage;
						//updateItemImage.DisaplayOrder = attachment.DisplayOrder;
						//updateItemImage.IsActive = attachment.IsActive;

						updateItemImage.IsPublish = attachment.IsPublish;
						updateItemImage.JsonData = JsonConvert.SerializeObject(attachment);
						enhanceYourStayImagesResponse.Add(updateItemImage);
					}
				}
				else
				{
					var EnhanceYourStayItemImage = new CustomerGuestAppEnhanceYourStayItemsImage
					{
						CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId,
						ItemsImages = attachment.ItemsImage,
						DisaplayOrder = attachment.DisplayOrder,
						IsActive = true,
						IsPublish = false,
						JsonData = null
					};
					listOfStayItems.Add(EnhanceYourStayItemImage);
					enhanceYourStayImagesResponse.Add(EnhanceYourStayItemImage);
				}

			}
			if (listOfStayItems.Any())
			{
				await _db.CustomerGuestAppEnhanceYourStayItemsImages.AddRangeAsync(listOfStayItems, cancellationToken);
			}
			await _db.SaveChangesAsync(cancellationToken);
		}
		return enhanceYourStayImagesResponse;
	}
	#endregion

	#region CustomerEnhanceYourStayItemAdd
	public async Task<CustomerGuestAppEnhanceYourStayItem> CustomerEnhanceYourStayItemAdd(CreateCustomerEnhanceYourStayItemIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{

		var customerEnhanceYourStayItem = new CustomerGuestAppEnhanceYourStayItem
		{
			CustomerId = request.CustomerId,
			CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId,
			CustomerGuestAppBuilderCategoryId = request.CustomerGuestAppBuilderCategoryId,
			Badge = request.Badge,
			ShortDescription = request.ShortDescription,
			LongDescription = request.LongDescription,
			ButtonType = request.ButtonType,
			ButtonText = request.ButtonText,
			ChargeType = request.ChargeType,
			Discount = request.Discount,
			Price = request.Price,
			Currency = request.Currency,
			IsPublish = false,
			IsActive = true,
			JsonData = null
		};

		await _db.CustomerGuestAppEnhanceYourStayItems.AddAsync(customerEnhanceYourStayItem, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);

		return customerEnhanceYourStayItem;
	}
	#endregion

	#region CustomerEnhanceYourStayItemUpdate
	public async Task<CustomerGuestAppEnhanceYourStayItem> CustomerEnhanceYourStayItemUpdate(UpdateCustomerEnhanceYourStayItemIn request, CustomerGuestAppEnhanceYourStayItem existingItem, ApplicationDbContext _db, CancellationToken cancellationToken)
	{

		//existingItem.CustomerId = request.CustomerId;
		//existingItem.CustomerGuestAppBuilderId = request.CustomerGuestAppBuilderId;
		//existingItem.CustomerGuestAppBuilderCategoryId = request.CustomerGuestAppBuilderCategoryId;
		//existingItem.Badge = request.Badge;
		//existingItem.ShortDescription = request.ShortDescription;
		//existingItem.LongDescription = request.LongDescription;
		//existingItem.ButtonType = request.ButtonType;
		//existingItem.ButtonText = request.ButtonText;
		//existingItem.ChargeType = request.ChargeType;
		//existingItem.Discount = request.Discount;
		//existingItem.Price = request.Price;
		//existingItem.Currency = request.Currency;
		existingItem.IsPublish = request.IsPublish;
		existingItem.JsonData = JsonConvert.SerializeObject(request);

		await _db.SaveChangesAsync(cancellationToken);

		return existingItem;
	}
	#endregion

	#region CustomerPropertyExtra and CustomerPropertyExtraDetails
	public async Task<CustomerPropertyExtra> CustomerPropertyExtraAddEdit(CustomerPropertyExtrasIn request, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerPropertyExtra = new CustomerPropertyExtra();

		var PropertyExtraId = request.Id;
		if (PropertyExtraId > 0)
		{
			customerPropertyExtra = await _db.CustomerPropertyExtras.Where(e => e.Id == PropertyExtraId).FirstOrDefaultAsync(cancellationToken);
			if (customerPropertyExtra != null)
			{
				//customerPropertyExtra.CustomerPropertyInformationId = request.CustomerPropertyInformationId;
				//customerPropertyExtra.ExtraType = request.ExtraType;
				//customerPropertyExtra.Name = request.Name;
				customerPropertyExtra.IsPublish = customerPropertyExtra.IsPublish;
				customerPropertyExtra.JsonData = JsonConvert.SerializeObject(request);
				//customerPropertyExtra.DisplayOrder = request.DisplayOrder;
				await _db.SaveChangesAsync(cancellationToken);
			}
		}
		else
		{
			customerPropertyExtra = new CustomerPropertyExtra()
			{
				CustomerPropertyInformationId = request.CustomerPropertyInformationId,
				ExtraType = request.ExtraType,
				Name = request.Name,
				IsActive = true,
				IsPublish = false,
				JsonData = null,
				DisplayOrder = request.DisplayOrder
			};
			await _db.CustomerPropertyExtras.AddAsync(customerPropertyExtra);
		}
		await _db.SaveChangesAsync(cancellationToken);
		return customerPropertyExtra!;
	}

	public async Task<List<CustomerPropertyExtraDetails>> CustomerPropertyExtraDetailsAddEdit(List<CustomerPropertyExtraDetailsIn> request, int Id, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customerPropertyExtraDetailsResponse = new List<CustomerPropertyExtraDetails>();
		var customerPropertyExtraDetails = new List<CustomerPropertyExtraDetails>();

		foreach (var item in request)
		{
			var PropertyExtraDetailId = item.Id;
			if (PropertyExtraDetailId > 0)
			{
				var customerPropertyExtraDetailupdate = await _db.CustomerPropertyExtraDetails.Where(e => e.Id == PropertyExtraDetailId).FirstOrDefaultAsync(cancellationToken);
				if (customerPropertyExtraDetailupdate != null)
				{
					//customerPropertyExtraDetailupdate.CustomerPropertyExtraId = Id;
					//customerPropertyExtraDetailupdate.Description = item.Description;
					//customerPropertyExtraDetailupdate.Link = item.Link;
					customerPropertyExtraDetailupdate.IsPublish = customerPropertyExtraDetailupdate.IsPublish;
					customerPropertyExtraDetailupdate.JsonData = JsonConvert.SerializeObject(item);
					customerPropertyExtraDetailsResponse.Add(customerPropertyExtraDetailupdate);
					await _db.SaveChangesAsync(cancellationToken);
				}
			}
			else
			{
				var customerPropertyExtraDetail = new CustomerPropertyExtraDetails()
				{
					CustomerPropertyExtraId = Id,
					Description = item.Description,
					Latitude = item.Latitude,
					Longitude = item.Longitude,
					IsActive = true,
					IsPublish = false,
					JsonData = null
				};
				customerPropertyExtraDetails.Add(customerPropertyExtraDetail);
				customerPropertyExtraDetailsResponse.Add(customerPropertyExtraDetail);
			}
		}
		if (customerPropertyExtraDetails.Any())
		{
			await _db.CustomerPropertyExtraDetails.AddRangeAsync(customerPropertyExtraDetails, cancellationToken);
		}

		await _db.SaveChangesAsync(cancellationToken);
		return customerPropertyExtraDetailsResponse;
	}
	#endregion

	#region GetPaymentServiceCredential
	public async Task<CustomerPaymentProcessorCredentials> GetPaymentProcessorCredential(int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var customer = await _db.Customers.Include(c => c.BusinessType).Where(c => c.Id == CustomerId).FirstOrDefaultAsync(cancellationToken);
		var existingMerchant = await _db.CustomerPaymentProcessorCredentials.Include(c => c.Customer).ThenInclude(c => c.BusinessType)
		   .FirstOrDefaultAsync(c => c.CustomerId == CustomerId);

		if (existingMerchant != null)
		{
			return existingMerchant;
		}

		var uniqutwostring = GenerateUniqueID(20);

		var biztype = customer.BusinessType.BizType.Substring(0, 2);
		string customermerchantidwithguid = $"{biztype}{uniqutwostring}";
		string customermerchantid = $"{biztype}{CustomerId}";


		var gr4vyData = await CallGr4vyApi(customermerchantid, customermerchantidwithguid);

		var newMerchant = new CustomerPaymentProcessorCredentials
		{
			CustomerId = CustomerId,
			MerchantId = gr4vyData.Id,
			IsActive = true,
		};

		await _db.CustomerPaymentProcessorCredentials.AddAsync(newMerchant, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);

		return newMerchant;
	}

	private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
	private string GenerateUniqueID(int length)
	{
		// We chose an encoding that fits 6 bits into every character,
		// so we can fit length*6 bits in total.
		// Each byte is 8 bits, so...
		int sufficientBufferSizeInBytes = (length * 6 + 7) / 8;

		var buffer = new byte[sufficientBufferSizeInBytes];
		random.GetBytes(buffer);
		string generatedId = Convert.ToBase64String(buffer).Substring(0, length);

		// Validate the generated ID against the regex pattern
		string regexPattern = "^[a-zA-Z0-9-]+$";
		if (!Regex.IsMatch(generatedId, regexPattern))
		{
			// Regenerate the ID until it matches the regex pattern
			return GenerateUniqueID(length);
		}

		return generatedId;
	}

	private async Task<CustomerMerchantAccountOut> CallGr4vyApi(string? customerId, string customerguid)
	{
		string token = _jwtService.GenerateJWTokenForGr4vy();

		var uniqueId = customerId;

		var requestBody = new
		{
			id = customerguid,
			display_name = customerId
		};

		var jsonRequest = JsonConvert.SerializeObject(requestBody);
		var requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

		using (var httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", "default");

			string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + "merchant-accounts";

			var response = await httpClient.PostAsync(apiUrl, requestContent);

			response.EnsureSuccessStatusCode();

			var responseBody = await response.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<CustomerMerchantAccountOut>(responseBody);
			return data;
		}
	}
	#endregion

	#region GetCustomerGuestBuyer
	public async Task<CustomerGuest> GetGuestBuyer(int GuestId, string merchantid, string token, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var guest = await _db.CustomerGuests.Where(c => c.Id == GuestId).FirstOrDefaultAsync(cancellationToken);

		if (guest != null && guest.GRBuyerId != null)
		{
			return guest;
		}
		var uniqutwostring = GenerateUniqueID(20);

		var firstname = guest.Firstname.Substring(0, 2);
		string guestbuyeridwithguid = $"{firstname}{uniqutwostring}";
		string guestbuyerid = $"{guest.Firstname} {guest.Lastname.Substring(0, 1)}.";

		AddGuestBuyerIn addGuestBuyerIn = new AddGuestBuyerIn();
		addGuestBuyerIn.external_identifier = guestbuyeridwithguid;
		addGuestBuyerIn.display_name = guestbuyerid;
		addGuestBuyerIn.billing_details.first_name = guest.Firstname;
		addGuestBuyerIn.billing_details.last_name = guest.Lastname;
		addGuestBuyerIn.billing_details.email_address = guest.Email;
		addGuestBuyerIn.billing_details.phone_number = guest.PhoneNumber;

		var gr4vyData = await CallCreateBuyerApi(addGuestBuyerIn, token, merchantid);

		guest.GRBuyerId = gr4vyData.id;
		guest.Country = guest.PhoneCountry ?? "US";
		_db.CustomerGuests.Update(guest);
		await _db.SaveChangesAsync(cancellationToken);

		return guest;
	}

	public async Task<CustomerGuest> GetAdminGuestBuyer(int GuestId, string merchantid, string token, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		var guest = await _db.CustomerGuests.Where(c => c.Id == GuestId).FirstOrDefaultAsync(cancellationToken);

		if (guest != null && guest.GRAdminBuyerId != null)
		{
			return guest;
		}
		var uniqutwostring = GenerateUniqueID(20);

		var firstname = guest.Firstname.Substring(0, 2);
		string guestbuyeridwithguid = $"{firstname}{uniqutwostring}";
		string guestbuyerid = $"{guest.Firstname} {guest.Lastname.Substring(0, 1)}.";

		AddGuestBuyerIn addGuestBuyerIn = new AddGuestBuyerIn();
		addGuestBuyerIn.external_identifier = guestbuyeridwithguid;
		addGuestBuyerIn.display_name = guestbuyerid;
		addGuestBuyerIn.billing_details.first_name = guest.Firstname;
		addGuestBuyerIn.billing_details.last_name = guest.Lastname;
		addGuestBuyerIn.billing_details.email_address = guest.Email;
		addGuestBuyerIn.billing_details.phone_number = guest.PhoneNumber;

		var gr4vyData = await CallCreateBuyerApi(addGuestBuyerIn, token, merchantid);

		guest.GRAdminBuyerId = gr4vyData.id;
		guest.Country = guest.PhoneCountry ?? "US";
		_db.CustomerGuests.Update(guest);
		await _db.SaveChangesAsync(cancellationToken);

		return guest;
	}

	private async Task<AddGuestBuyerOut> CallCreateBuyerApi(AddGuestBuyerIn addGuestBuyerIn, string token, string merchantid)
	{
		var payload = SerializeCreateBuyerPayload(addGuestBuyerIn);
		var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

		using (var httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", merchantid);

			string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + "buyers";

			var response = await httpClient.PostAsync(apiUrl, requestContent);

			response.EnsureSuccessStatusCode();

			var responseBody = await response.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<AddGuestBuyerOut>(responseBody);
			return data;
		}
	}
	private static string SerializeCreateBuyerPayload(AddGuestBuyerIn? guestBuyerIn)
	{
		return JsonConvert.SerializeObject(guestBuyerIn);
	}
	#endregion


	public async Task<List<ReplicateDateModelForOldData>> GetOldGuestAppBuilderData(int id, CancellationToken cancellationtoken, IDapperRepository _dappers)
	{

		var spParams = new DynamicParameters();
		spParams.Add("BuilderId", id, DbType.Int32);

		var ReplicatedDataOuts = await _dappers.GetAllJsonData<ReplicateDateModelForOldData>("[dbo].[GetGuestAppBuilder]", spParams, cancellationtoken, CommandType.StoredProcedure);

		return ReplicatedDataOuts;
	}

	public async Task<List<CustomerGuestAppBuildersOutId>> GetNewGusestAppBuilderData(int id, CancellationToken cancellationtoken, IDapperRepository _dappers)
	{

		var spParams = new DynamicParameters();
		spParams.Add("BuilderId", id, DbType.Int32);

		var ReplicatedDataOuts = await _dappers.GetAllJsonData<CustomerGuestAppBuildersOutId>("[dbo].[GetGuestAppBuilder]", spParams, cancellationtoken, CommandType.StoredProcedure);

		return ReplicatedDataOuts;
	}

	public async Task<bool> DeleteGuestAppBuilderData(List<ReplicateDateModelForOldData> OldDataForBuilder, ApplicationDbContext _db, CancellationToken cancellationToken)
	{
		try
		{

			if (OldDataForBuilder != null)
			{
				foreach (var item in OldDataForBuilder)
				{
					// PropertyInformation
					if (item.CustomerPropertyinfo != null)
					{

						foreach (var customerPropertyInfo in item.CustomerPropertyinfo)
						{


							if (customerPropertyInfo.CustomerPropertyServices != null)
							{
								// PropertyService
								foreach (var customerPropertyService in customerPropertyInfo.CustomerPropertyServices)
								{

									if (customerPropertyService.CustomerPropertyServiceImages != null)
									{

										foreach (var customerPropertyServiceImage in customerPropertyService.CustomerPropertyServiceImages)
										{
											if (customerPropertyService.Id != null)
											{
												var propertyServicedeleteremove = await _db.CustomerPropertyServiceImages.Where(i => i.CustomerPropertyServiceId == customerPropertyService.Id).ToListAsync();
												foreach (var item1 in propertyServicedeleteremove)
												{
													_db.CustomerPropertyServiceImages.Remove(item1);


												}
												//await _db.SaveChangesAsync(cancellationToken);

											}

										}
										if (customerPropertyInfo.Id != null)
										{
											var propertyServiceDeleteData = await _db.CustomerPropertyServices.Where(i => i.CustomerPropertyInformationId == customerPropertyInfo.Id).ToListAsync();
											foreach (var item1 in propertyServiceDeleteData)
											{

												_db.CustomerPropertyServices.Remove(item1);
											}
											//await _db.SaveChangesAsync(cancellationToken);

										}
									}


								}
							}
							//DisplayOrder
							if (customerPropertyInfo.DisplayOrderForPropertyInfo != null)
							{
								foreach (var DisplayOrderForPropertyInfo in customerPropertyInfo.DisplayOrderForPropertyInfo)
								{

									if (customerPropertyInfo.Id != null)
									{

										var propertyServiceDeleteData = await _db.ScreenDisplayOrderAndStatuses.Where(i => i.RefrenceId == customerPropertyInfo.Id && i.ScreenName == 1).ToListAsync();
										foreach (var item1 in propertyServiceDeleteData)
										{

											_db.ScreenDisplayOrderAndStatuses.Remove(item1);
										}

									}

								}
							}
							// PropertyGalary
							if (customerPropertyInfo.CustomerPropertyGallery != null)
							{
								foreach (var CustomerPropertyGalarys in customerPropertyInfo.CustomerPropertyGallery)
								{
									if (customerPropertyInfo.Id != null)
									{
										var propertygalary = await _db.CustomerPropertyGalleries.Where(i => i.CustomerPropertyInformationId == customerPropertyInfo.Id).ToListAsync();
										foreach (var item1 in propertygalary)
										{

											_db.CustomerPropertyGalleries.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}

								}
							}
							// propertyExtra
							if (customerPropertyInfo.CustomerPropertyExtras != null)
							{
								foreach (var CustomerPropertyExtra in customerPropertyInfo.CustomerPropertyExtras)
								{

									if (CustomerPropertyExtra.CustomerPropertyExtraDetails != null)
									{

										foreach (var CustomerExtraDetails in CustomerPropertyExtra.CustomerPropertyExtraDetails)
										{
											if (CustomerPropertyExtra.Id != null)
											{
												var propertyExtra = await _db.CustomerPropertyExtraDetails.Where(i => i.CustomerPropertyExtraId == CustomerPropertyExtra.Id).ToListAsync();
												foreach (var item1 in propertyExtra)
												{

													_db.CustomerPropertyExtraDetails.Remove(item1);
												}
												//await _db.SaveChangesAsync(cancellationToken);
											}

										}
									}
									if (customerPropertyInfo.Id != null)
									{
										var propertyExtras = await _db.CustomerPropertyExtras.Where(i => i.CustomerPropertyInformationId == customerPropertyInfo.Id).ToListAsync();
										foreach (var item1 in propertyExtras)
										{

											_db.CustomerPropertyExtras.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}
								}
							}
							//proprtyEmergency
							if (customerPropertyInfo.CustomerPropertyEmergencyNo != null)
							{
								foreach (var CustomerPropertyEmergency in customerPropertyInfo.CustomerPropertyEmergencyNo)
								{
									if (customerPropertyInfo.Id != null)
									{
										var propertyEmargencyNo = await _db.CustomerPropertyEmergencyNumbers.Where(i => i.CustomerPropertyInformationId == customerPropertyInfo.Id).ToListAsync();
										foreach (var item1 in propertyEmargencyNo)
										{

											_db.CustomerPropertyEmergencyNumbers.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}
								}
							}

						}
						if (item.Id != null)
						{
							var PropertyInfoDatas = await _db.CustomerPropertyInformations.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();
							foreach (var item1 in PropertyInfoDatas)
							{

								_db.CustomerPropertyInformations.Remove(item1);
							}
							//await _db.SaveChangesAsync(cancellationToken);
						}
					}
					//Enhanchyourstay
					if (item.CustomerGuestAppEnhanceYourStayCategories != null)
					{
						foreach (var customerEnhanchYourStay in item.CustomerGuestAppEnhanceYourStayCategories)
						{

							if (customerEnhanchYourStay.CustomerGuestAppEnhanceYourStayItems != null)
							{
								foreach (var EnhanchYourStayItem in customerEnhanchYourStay.CustomerGuestAppEnhanceYourStayItems)
								{
									if (EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayItemsImages != null)
									{
										foreach (var EnhanchYourStayImage in EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayItemsImages)
										{

											if (EnhanchYourStayItem.Id != null)
											{
												var enhanchRemoveData = await _db.CustomerGuestAppEnhanceYourStayItemsImages.Where(i => i.CustomerGuestAppEnhanceYourStayItemId == EnhanchYourStayItem.Id).ToListAsync();
												foreach (var item1 in enhanchRemoveData)
												{
													_db.CustomerGuestAppEnhanceYourStayItemsImages.Remove(item1);

												}
												//await _db.SaveChangesAsync(cancellationToken);
											}
										}
									}

									//EnhanchYourStayExtra
									if (EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayCategoryItemsExtras != null)
									{
										foreach (var EnhanchYourStayCategoryExtra in EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayCategoryItemsExtras)
										{

											if (EnhanchYourStayItem.Id != null)
											{
												var enhanchRemoveData = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(i => i.CustomerGuestAppEnhanceYourStayItemId == EnhanchYourStayItem.Id).ToListAsync();
												foreach (var item1 in enhanchRemoveData)
												{

													_db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Remove(item1);
												}
												//await _db.SaveChangesAsync(cancellationToken);
											}


										}
									}
									if (customerEnhanchYourStay.Id != null)
									{
										var EnhanchyourStaysItemDataRemove = await _db.CustomerGuestAppEnhanceYourStayItems.Where(i => i.CustomerGuestAppBuilderCategoryId == customerEnhanchYourStay.Id).ToListAsync();
										foreach (var item1 in EnhanchyourStaysItemDataRemove)
										{
											_db.CustomerGuestAppEnhanceYourStayItems.Remove(item1);

										}
										//await _db.SaveChangesAsync(cancellationToken);
									}
								}
							}

							if (item.Id != null)
							{
								var EnhanchyourStaysItemDataRemoveFor = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();
								foreach (var item1 in EnhanchyourStaysItemDataRemoveFor)
								{

									_db.CustomerGuestAppEnhanceYourStayCategories.Remove(item1);
								}
								//await _db.SaveChangesAsync(cancellationToken);
							}
						}
					}
					//HouseKeeping
					if (item.CustomerGuestAppHousekeepingCategories != null)
					{
						foreach (var customerHouseKeeping in item.CustomerGuestAppHousekeepingCategories)
						{

							if (customerHouseKeeping.HouseItem != null)
							{

								foreach (var customerHouseitems in customerHouseKeeping.HouseItem)
								{


									if (customerHouseKeeping.Id != null)
									{
										var HouseitemsDatacategory = await _db.CustomerGuestAppHousekeepingItems.Where(i => i.CustomerGuestAppHousekeepingCategoryId == customerHouseKeeping.Id).ToListAsync();
										foreach (var item1 in HouseitemsDatacategory)
										{

											_db.CustomerGuestAppHousekeepingItems.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}

								}
							}
							if (item.Id != null)
							{
								var HouseitemsDatacategoryremove = await _db.CustomerGuestAppHousekeepingCategories.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();
								foreach (var item1 in HouseitemsDatacategoryremove)
								{

									_db.CustomerGuestAppHousekeepingCategories.Remove(item1);
								}
								//await _db.SaveChangesAsync(cancellationToken);
							}

						}
					}
					//reception
					if (item.CustomerGuestAppReceptionCategories != null)
					{
						foreach (var CustomerReceptionCatagory in item.CustomerGuestAppReceptionCategories)
						{

							if (CustomerReceptionCatagory.ReceptionItem != null)
							{
								foreach (var CustomerReceptionItem in CustomerReceptionCatagory.ReceptionItem)
								{
									if (CustomerReceptionCatagory.Id != null)
									{
										var receptioncategoryData = await _db.CustomerGuestAppReceptionItems.Where(i => i.CustomerGuestAppReceptionCategoryId == CustomerReceptionCatagory.Id).ToListAsync();
										foreach (var item1 in receptioncategoryData)
										{
											_db.CustomerGuestAppReceptionItems.Remove(item1);

										}

										//await _db.SaveChangesAsync(cancellationToken);
									}

								}
							}
							if (item.Id != null)
							{
								var receptioncategorydata = await _db.CustomerGuestAppReceptionCategories.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();
								foreach (var item1 in receptioncategorydata)
								{

									_db.CustomerGuestAppReceptionCategories.Remove(item1);
								}
								//await _db.SaveChangesAsync(cancellationToken);
							}


						}
					}
					//roomservice
					if (item.CustomerGuestAppRoomServiceCategories != null)
					{
						foreach (var roomcatagories in item.CustomerGuestAppRoomServiceCategories)
						{

							if (roomcatagories.RoomItem != null)
							{
								foreach (var customerroomItems in roomcatagories.RoomItem)
								{
									if (roomcatagories.Id != null)
									{
										var roomservice = await _db.CustomerGuestAppRoomServiceItems.Where(i => i.CustomerGuestAppRoomServiceCategoryId == roomcatagories.Id).ToListAsync();
										foreach (var item1 in roomservice)
										{

											_db.CustomerGuestAppRoomServiceItems.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}

								}
								if (item.Id != null)
								{
									var roomservicedata = await _db.CustomerGuestAppRoomServiceCategories.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();

									foreach (var item1 in roomservicedata)
									{
										_db.CustomerGuestAppRoomServiceCategories.Remove(item1);

									}
									//await _db.SaveChangesAsync(cancellationToken);
								}
							}
						}
					}
					//concirage
					if (item.CustomerGuestAppConciergeCategories != null)
					{
						foreach (var CustomerConcirage in item.CustomerGuestAppConciergeCategories)
						{

							if (CustomerConcirage.Conciergeitem != null)
							{
								foreach (var ConcirageItems in CustomerConcirage.Conciergeitem)
								{

									if (CustomerConcirage.Id != null)
									{
										var concirageitem = await _db.CustomerGuestAppConciergeItems.Where(i => i.CustomerGuestAppConciergeCategoryId == CustomerConcirage.Id).ToListAsync();
										foreach (var item1 in concirageitem)
										{

											_db.CustomerGuestAppConciergeItems.Remove(item1);
										}
										//await _db.SaveChangesAsync(cancellationToken);
									}
								}
							}
							if (item.Id != null)
							{
								var concirageitem = await _db.CustomerGuestAppConciergeCategories.Where(i => i.CustomerGuestAppBuilderId == item.Id).ToListAsync();
								foreach (var item1 in concirageitem)
								{

									_db.CustomerGuestAppConciergeCategories.Remove(item1);
								}
								//await _db.SaveChangesAsync(cancellationToken);
							}

						}
					}

				}
				await _db.SaveChangesAsync(cancellationToken);

				return true;

			}
			else
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			return false;
		}

	}

	public async Task<bool> AddGuestAppBuilderData(List<CustomerGuestAppBuildersOutId> newBuilderData, ApplicationDbContext _db, CancellationToken cancellationToken, ReplicateDataIn In)
	{
		try
		{

			if (newBuilderData != null)
			{
				foreach (var item in newBuilderData)
				{
					if (In.NewBuilderId > 0)
					{
						var BuliderData = await _db.CustomerGuestAppBuilders.Where(i => i.Id == In.NewBuilderId).FirstOrDefaultAsync(cancellationToken);

						if (BuliderData != null)
						{
							var jsonObj = new CreateCustomerGuestAppBuilderIn()
							{
								CustomerId = item.CustomerId,
								CustomerRoomNameId = item.CustomerRoomNameId,
								Message = item.Message,
								SecondaryMessage = item.SecondaryMessage,
								LocalExperience = item.LocalExperience,
								Ekey = item.Ekey,
								PropertyInfo = item.PropertyInfo,
								EnhanceYourStay = item.EnhanceYourStay,
								Reception = item.Reception,
								Housekeeping = item.Housekeeping,
								RoomService = item.RoomService,
								Concierge = item.Concierge,
								TransferServices = item.TransferServices,
								OnlineCheckIn = item.OnlineCheckIn,
								IsActive = item.IsActive,
							};
							BuliderData.IsPublish = false;
							BuliderData.JsonData = JsonConvert.SerializeObject(jsonObj);
						}

						await _db.SaveChangesAsync(cancellationToken);

					}


					// guestDisplayOrder

					if (item.DisplayOrderForGuestBuilder != null)
					{
						foreach (var DisplayOrderForGuestBuilder in item.DisplayOrderForGuestBuilder)
						{
							var DisplayOrderGuestId = await _db.ScreenDisplayOrderAndStatuses.Where(i => i.RefrenceId == In.NewBuilderId && i.ScreenName == 2).FirstOrDefaultAsync();
							if (DisplayOrderGuestId != null)
							{

								DisplayOrderGuestId.ScreenName = DisplayOrderForGuestBuilder.ScreenName;
								DisplayOrderGuestId.JsonData = DisplayOrderForGuestBuilder.JsonData;
								DisplayOrderGuestId.IsActive = DisplayOrderForGuestBuilder.IsActive;
								DisplayOrderGuestId.IsPublish = false;

								await _db.SaveChangesAsync(cancellationToken);

							}

						}
					}
					// PropertyInfo
					if (item.CustomerPropertyinfo != null)
					{

						foreach (var customerPropertyInfo in item.CustomerPropertyinfo)
						{
							var CustomerProprty = new CustomerPropertyInformation()
							{
								CustomerId = customerPropertyInfo.CustomerId,
								CustomerGuestAppBuilderId = In.NewBuilderId,
								WifiPassword = customerPropertyInfo.WifiPassword,
								WifiUsername = customerPropertyInfo.WifiUsername,
								Overview = customerPropertyInfo.Overview,
								CheckInPolicy = customerPropertyInfo.CheckInPolicy,
								TermsAndConditions = customerPropertyInfo.TermsAndConditions,
								Street = customerPropertyInfo.Street,
								StreetNumber = customerPropertyInfo.StreetNumber,
								City = customerPropertyInfo.City,
								Postalcode = customerPropertyInfo.Postalcode,
								Country = customerPropertyInfo.Country,
								IsPublish = false,
								Latitude = customerPropertyInfo.Latitude,
								Longitude = customerPropertyInfo.Longitude,
								IsActive = customerPropertyInfo.IsActive
							};
							var PropertyInfoData = await _db.CustomerPropertyInformations.AddAsync(CustomerProprty);
							await _db.SaveChangesAsync(cancellationToken);

							if (customerPropertyInfo.CustomerPropertyServices != null)
							{
								// PropertyService
								foreach (var customerPropertyService in customerPropertyInfo.CustomerPropertyServices)
								{
									var propertyService = new CustomerPropertyService()
									{
										CustomerPropertyInformationId = CustomerProprty.Id,
										Name = customerPropertyService.Name,
										Icon = customerPropertyService.Icon,
										Description = customerPropertyService.Description,
										IsPublish = false,

									};
									var PropertyServiceData = await _db.CustomerPropertyServices.AddAsync(propertyService);
									await _db.SaveChangesAsync(cancellationToken);

									if (customerPropertyService.CustomerPropertyServiceImages != null)
									{

										foreach (var customerPropertyServiceImage in customerPropertyService.CustomerPropertyServiceImages)
										{
											var PropServiceImage = new CustomerPropertyServiceImage()
											{
												CustomerPropertyServiceId = propertyService.Id,
												ServiceImages = customerPropertyServiceImage.ServiceImages,
												IsPublish = false,
												IsActive = customerPropertyServiceImage.IsActive,
											};

											var PropServiceImageData = await _db.CustomerPropertyServiceImages.AddAsync(PropServiceImage);
										}
										await _db.SaveChangesAsync(cancellationToken);
									}


								}
							}
							//DisplayOrder
							if (customerPropertyInfo.DisplayOrderForPropertyInfo != null)
							{
								foreach (var DisplayOrderForPropertyInfo in customerPropertyInfo.DisplayOrderForPropertyInfo)
								{

									var DisplayOrder = new ScreenDisplayOrderAndStatus()
									{
										ScreenName = DisplayOrderForPropertyInfo.ScreenName,
										JsonData = DisplayOrderForPropertyInfo.JsonData,
										RefrenceId = CustomerProprty.Id,
										IsActive = DisplayOrderForPropertyInfo.IsActive
									};
									var DisplayOrderData = _db.ScreenDisplayOrderAndStatuses.AddAsync(DisplayOrder);
									await _db.SaveChangesAsync(cancellationToken);


								}
							}
							// PropertyGalary
							if (customerPropertyInfo.CustomerPropertyGallery != null)
							{
								foreach (var CustomerPropertyGalarys in customerPropertyInfo.CustomerPropertyGallery)
								{

									var CustomerGalary = new CustomerPropertyGallery()
									{
										CustomerPropertyInformationId = CustomerProprty.Id,
										PropertyImage = CustomerPropertyGalarys.PropertyImage,
										IsPublish = false,
										IsActive = CustomerPropertyGalarys.IsActive,
									};

									var CustomerGalaryData = await _db.CustomerPropertyGalleries.AddAsync(CustomerGalary);
									await _db.SaveChangesAsync(cancellationToken);


								}
							}
							// propertyExtra
							if (customerPropertyInfo.CustomerPropertyExtras != null)
							{
								foreach (var CustomerPropertyExtra in customerPropertyInfo.CustomerPropertyExtras)
								{
									var CustomerExtra = new CustomerPropertyExtra()
									{
										CustomerPropertyInformationId = CustomerProprty.Id,
										ExtraType = CustomerPropertyExtra.ExtraType,
										Name = CustomerPropertyExtra.Name,
										IsPublish = false,
										IsActive = CustomerPropertyExtra.IsActive,
										DisplayOrder = CustomerPropertyExtra.DisplayOrder
									};
									var CustomerExtraData = await _db.CustomerPropertyExtras.AddAsync(CustomerExtra);
									await _db.SaveChangesAsync(cancellationToken);

									if (CustomerPropertyExtra.CustomerPropertyExtraDetails != null)
									{

										foreach (var CustomerExtraDetails in CustomerPropertyExtra.CustomerPropertyExtraDetails)
										{
											var CustomerExtraDetailes = new CustomerPropertyExtraDetails()
											{
												CustomerPropertyExtraId = CustomerExtra.Id,
												Description = CustomerExtraDetails.Description,
												Latitude = CustomerExtraDetails.Latitude,
												Longitude = CustomerExtraDetails.Longitude,
												IsPublish = false,
												IsActive = CustomerExtraDetails.IsActive

											};
											var CustomerExtraDetailesData = await _db.CustomerPropertyExtraDetails.AddAsync(CustomerExtraDetailes);
										}
										await _db.SaveChangesAsync(cancellationToken);
									}
								}
							}
							//proprtyEmergency
							if (customerPropertyInfo.CustomerPropertyEmergencyNo != null)
							{
								foreach (var CustomerPropertyEmergency in customerPropertyInfo.CustomerPropertyEmergencyNo)
								{
									var EmergencyNo = new CustomerPropertyEmergencyNumber()
									{
										CustomerPropertyInformationId = CustomerProprty.Id,
										Name = CustomerPropertyEmergency.Name,
										PhoneNumber = CustomerPropertyEmergency.PhoneNumber,
										IsPublish = false,
										IsActive = CustomerPropertyEmergency.IsActive,
										DisplayOrder = CustomerPropertyEmergency.DisplayOrder

									};
									var EmergencyNoData = await _db.CustomerPropertyEmergencyNumbers.AddAsync(EmergencyNo);
									await _db.SaveChangesAsync(cancellationToken);
								}
							}

						}
					}
					//Enhanchyourstay

					if (item.CustomerGuestAppEnhanceYourStayCategories != null)
					{
						foreach (var customerEnhanchYourStay in item.CustomerGuestAppEnhanceYourStayCategories)
						{
							var EnhanchYourStay = new CustomerGuestAppEnhanceYourStayCategory()
							{
								CustomerGuestAppBuilderId = In.NewBuilderId,
								CustomerId = customerEnhanchYourStay.CustomerId,
								CategoryName = customerEnhanchYourStay.CategoryName,
								DisplayOrder = customerEnhanchYourStay.DisplayOrder,
								IsPublish = false,
								IsActive = customerEnhanchYourStay.IsActive,
							};
							var EnhanchYourStayData = await _db.CustomerGuestAppEnhanceYourStayCategories.AddAsync(EnhanchYourStay);
							await _db.SaveChangesAsync(cancellationToken);

							if (customerEnhanchYourStay.CustomerGuestAppEnhanceYourStayItems != null)
							{
								foreach (var EnhanchYourStayItem in customerEnhanchYourStay.CustomerGuestAppEnhanceYourStayItems)
								{
									//enhanchyourStayItem
									var EnhanchyourStaysItem = new CustomerGuestAppEnhanceYourStayItem()
									{
										CustomerGuestAppBuilderId = In.NewBuilderId,
										CustomerId = EnhanchYourStayItem.CustomerId,
										CustomerGuestAppBuilderCategoryId = EnhanchYourStay.Id,
										Badge = EnhanchYourStayItem.Badge,
										ShortDescription = EnhanchYourStayItem.ShortDescription,
										LongDescription = EnhanchYourStayItem.LongDescription,
										ButtonType = EnhanchYourStayItem.ButtonType,
										ButtonText = EnhanchYourStayItem.ButtonText,
										ChargeType = EnhanchYourStayItem.ChargeType,
										Discount = EnhanchYourStayItem.Discount,
										Price = EnhanchYourStayItem.Price,
										IsActive = EnhanchYourStayItem.IsActive,
										Currency = EnhanchYourStayItem.Currency,
										DisplayOrder = EnhanchYourStayItem.DisplayOrder,
										IsPublish = false,

									};
									var EnhanchyourStaysItemData = await _db.CustomerGuestAppEnhanceYourStayItems.AddAsync(EnhanchyourStaysItem);
									await _db.SaveChangesAsync(cancellationToken);

									//EnhanchYourStayItemImage
									if (EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayItemsImages != null)
									{
										foreach (var EnhanchYourStayImage in EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayItemsImages)
										{

											var EnhanchYourStayItemImage = new CustomerGuestAppEnhanceYourStayItemsImage()
											{
												CustomerGuestAppEnhanceYourStayItemId = EnhanchyourStaysItem.Id,
												IsActive = EnhanchYourStayImage.IsActive,
												ItemsImages = EnhanchYourStayImage.ItemsImages,
												DisaplayOrder = EnhanchYourStayImage.DisaplayOrder,
												IsPublish = false,

											};
											var EnhanchYourStayItemImageData = await _db.CustomerGuestAppEnhanceYourStayItemsImages.AddAsync(EnhanchYourStayItemImage);
										}
										await _db.SaveChangesAsync(cancellationToken);
									}

									//EnhanchYourStayExtra
									if (EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayCategoryItemsExtras != null)
									{
										foreach (var EnhanchYourStayCategoryExtra in EnhanchYourStayItem.CustomerGuestAppEnhanceYourStayCategoryItemsExtras)
										{
											var EnhanchItemExtras = new CustomerGuestAppEnhanceYourStayCategoryItemsExtra()
											{
												CustomerGuestAppEnhanceYourStayItemId = EnhanchyourStaysItem.Id,
												QueType = EnhanchYourStayCategoryExtra.QueType,
												Questions = EnhanchYourStayCategoryExtra.Questions,
												OptionValues = EnhanchYourStayCategoryExtra.OptionValues,
												IsPublish = false,
												IsActive = EnhanchYourStayCategoryExtra.IsActive
											};

											var EnhanchItemExtrasData = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.AddAsync(EnhanchItemExtras);
										}
										await _db.SaveChangesAsync(cancellationToken);
									}


								}
							}

						}
					}

					//Reception 

					if (item.CustomerGuestAppReceptionCategories != null)
					{
						foreach (var CustomerReceptionCatagory in item.CustomerGuestAppReceptionCategories)
						{
							var ReceptionCatagory = new CustomerGuestAppReceptionCategory()
							{
								CustomerGuestAppBuilderId = In.NewBuilderId,
								CustomerId = CustomerReceptionCatagory.CustomerId,
								CategoryName = CustomerReceptionCatagory.CategoryName,
								DisplayOrder = CustomerReceptionCatagory.DisplayOrder,
								IsPublish = false,
								IsActive = CustomerReceptionCatagory.IsActive
							};
							var ReceptionCatagoryData = await _db.CustomerGuestAppReceptionCategories.AddAsync(ReceptionCatagory);
							await _db.SaveChangesAsync(cancellationToken);

							if (CustomerReceptionCatagory.ReceptionItem != null)
							{
								foreach (var CustomerReceptionItem in CustomerReceptionCatagory.ReceptionItem)
								{
									var ReceptionsItem = new CustomerGuestAppReceptionItem()
									{
										CustomerId = CustomerReceptionItem.CustomerId,
										CustomerGuestAppBuilderId = In.NewBuilderId,
										CustomerGuestAppReceptionCategoryId = ReceptionCatagory.Id,
										Name = CustomerReceptionItem.Name,
										ItemsMonth = CustomerReceptionItem.ItemsMonth,
										ItemsDay = CustomerReceptionItem.ItemsDay,
										ItemsMinute = CustomerReceptionItem.ItemsMinute,
										ItemsHour = CustomerReceptionItem.ItemsHour,
										QuantityBar = CustomerReceptionItem.QuantityBar,
										ItemLocation = CustomerReceptionItem.ItemLocation,
										Comment = CustomerReceptionItem.Comment,
										IsPriceEnable = CustomerReceptionItem.IsPriceEnable,
										Price = CustomerReceptionItem.Price,
										Currency = CustomerReceptionItem.Currency,
										DisplayOrder = CustomerReceptionItem.DisplayOrder,
										IsPublish = false,
										IsActive = CustomerReceptionItem.IsActive

									};
									var ReceptionsItemData = await _db.CustomerGuestAppReceptionItems.AddAsync(ReceptionsItem);


								}
								await _db.SaveChangesAsync(cancellationToken);
							}


						}
					}

					if (item.CustomerGuestAppHousekeepingCategories != null)
					{
						foreach (var customerHouseKeeping in item.CustomerGuestAppHousekeepingCategories)
						{
							var HousKeepingCategory = new CustomerGuestAppHousekeepingCategory()
							{
								CustomerGuestAppBuilderId = In.NewBuilderId,
								CustomerId = customerHouseKeeping.CustomerId,
								CategoryName = customerHouseKeeping.CategoryName,
								DisplayOrder = customerHouseKeeping.DisplayOrder,
								IsPublish = false,
								IsActive = customerHouseKeeping.IsActive,
							};

							var HousKeepingCategoryData = await _db.CustomerGuestAppHousekeepingCategories.AddAsync(HousKeepingCategory);
							await _db.SaveChangesAsync(cancellationToken);

							if (customerHouseKeeping.HouseItem != null)
							{

								foreach (var customerHouseitems in customerHouseKeeping.HouseItem)
								{
									var Houseitems = new CustomerGuestAppHousekeepingItem()
									{
										CustomerId = customerHouseitems.CustomerId,
										CustomerGuestAppBuilderId = In.NewBuilderId,
										CustomerGuestAppHousekeepingCategoryId = HousKeepingCategory.Id,
										Name = customerHouseitems.Name,
										ItemsMonth = customerHouseitems.ItemsMonth,
										ItemsDay = customerHouseitems.ItemsDay,
										ItemsMinute = customerHouseitems.ItemsMinute,
										ItemsHour = customerHouseitems.ItemsHour,
										QuantityBar = customerHouseitems.QuantityBar,
										ItemLocation = customerHouseitems.ItemLocation,
										Comment = customerHouseitems.Comment,
										IsPriceEnable = customerHouseitems.IsPriceEnable,
										Price = customerHouseitems.Price,
										Currency = customerHouseitems.Currency,
										DisplayOrder = customerHouseitems.DisplayOrder,
										IsPublish = false,
										IsActive = customerHouseitems.IsActive,

									};
									var HouseitemsData = await _db.CustomerGuestAppHousekeepingItems.AddAsync(Houseitems);
								}
								await _db.SaveChangesAsync(cancellationToken);
							}

						}
					}

					if (item.CustomerGuestAppRoomServiceCategories != null)
					{
						foreach (var roomcatagories in item.CustomerGuestAppRoomServiceCategories)
						{
							var roomCategories = new CustomerGuestAppRoomServiceCategory()
							{
								CustomerGuestAppBuilderId = In.NewBuilderId,
								CustomerId = roomcatagories.CustomerId,
								CategoryName = roomcatagories.CategoryName,
								DisplayOrder = roomcatagories.DisplayOrder,
								IsPublish = false,
								IsActive = roomcatagories.IsActive,

							};
							var roomCategoriesData = await _db.CustomerGuestAppRoomServiceCategories.AddAsync(roomCategories);
							await _db.SaveChangesAsync(cancellationToken);

							if (roomcatagories.RoomItem != null)
							{
								foreach (var customerroomItems in roomcatagories.RoomItem)
								{
									var roomItems = new CustomerGuestAppRoomServiceItem()
									{
										CustomerId = customerroomItems.CustomerId,
										CustomerGuestAppBuilderId = In.NewBuilderId,
										CustomerGuestAppRoomServiceCategoryId = roomCategories.Id,
										Name = customerroomItems.Name,
										ItemsMonth = customerroomItems.ItemsMonth,
										ItemsDay = customerroomItems.ItemsDay,
										ItemsMinute = customerroomItems.ItemsMinute,
										ItemsHour = customerroomItems.ItemsHour,
										QuantityBar = customerroomItems.QuantityBar,
										ItemLocation = customerroomItems.ItemLocation,
										Comment = customerroomItems.Comment,
										IsPriceEnable = customerroomItems.IsPriceEnable,
										Price = customerroomItems.Price,
										Currency = customerroomItems.Currency,
										DisplayOrder = customerroomItems.DisplayOrder,
										IsPublish = false,
										IsActive = customerroomItems.IsActive,

									};

									var roomItemsData = await _db.CustomerGuestAppRoomServiceItems.AddAsync(roomItems);
								}
								await _db.SaveChangesAsync(cancellationToken);
							}
						}
					}

					if (item.CustomerGuestAppConciergeCategories != null)
					{
						foreach (var CustomerConcirage in item.CustomerGuestAppConciergeCategories)
						{
							var ConcirageCategory = new CustomerGuestAppConciergeCategory()
							{
								CustomerGuestAppBuilderId = In.NewBuilderId,
								CustomerId = CustomerConcirage.CustomerId,
								CategoryName = CustomerConcirage.CategoryName,
								DisplayOrder = CustomerConcirage.DisplayOrder,
								IsPublish = false,
								IsActive = CustomerConcirage.IsActive,

							};
							var ConcirageCategoryData = await _db.CustomerGuestAppConciergeCategories.AddAsync(ConcirageCategory);
							await _db.SaveChangesAsync(cancellationToken);

							if (CustomerConcirage.Conciergeitem != null)
							{
								foreach (var ConcirageItems in CustomerConcirage.Conciergeitem)
								{

									var CustomerConcirageItems = new CustomerGuestAppConciergeItem()
									{
										CustomerId = ConcirageItems.CustomerId,
										CustomerGuestAppBuilderId = In.NewBuilderId,
										CustomerGuestAppConciergeCategoryId = ConcirageCategory.Id,
										Name = ConcirageItems.Name,
										ItemsMonth = ConcirageItems.ItemsMonth,
										ItemsDay = ConcirageItems.ItemsDay,
										ItemsMinute = ConcirageItems.ItemsMinute,
										ItemsHour = ConcirageItems.ItemsHour,
										QuantityBar = ConcirageItems.QuantityBar,
										ItemLocation = ConcirageItems.ItemLocation,
										Comment = ConcirageItems.Comment,
										IsPriceEnable = ConcirageItems.IsPriceEnable,
										Price = ConcirageItems.Price,
										Currency = ConcirageItems.Currency,
										DisplayOrder = ConcirageItems.DisplayOrder,
										IsPublish = false,
										IsActive = ConcirageItems.IsActive,

									};
									var CustomerConcirageItemsData = await _db.CustomerGuestAppConciergeItems.AddAsync(CustomerConcirageItems);
								}
								await _db.SaveChangesAsync(cancellationToken);
							}


						}
					}


				}


			}

			return true;
		}
		catch (Exception ex)
		{
			return false;
		}


	}

	#region CustomerLogoResize
	public async Task<string> ResizeImageFromUrlAsync(string imageUrl, int newWidth, int newHeight, IUserFilesService _userFilesService)
	{
		try
		{
			using (WebClient client = new WebClient())
			{
				using (Stream stream = client.OpenRead(imageUrl))
				{
					using (var sourceImage = System.Drawing.Image.FromStream(stream))
					{
						using (var newImage = new Bitmap(newWidth, newHeight))
						{
							using (var graphics = Graphics.FromImage(newImage))
							{
								graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
								graphics.SmoothingMode = SmoothingMode.HighQuality;
								graphics.CompositingQuality = CompositingQuality.HighQuality;
								graphics.DrawImage(sourceImage, 0, 0, newWidth, newHeight);
							}

							using (var memoryStream = new MemoryStream())
							{
								newImage.Save(memoryStream, ImageFormat.Jpeg); // You can choose a different format if needed

								// Create an IFormFile from the MemoryStream
								IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, "image", "resized_image.jpg");
								string documentName = ((UploadDocumentType)5).ToString();
								// Upload the resized image to your service
								var webFile = await _userFilesService.UploadWebFileOnGivenPathAsync(formFile, documentName, CancellationToken.None, true);

								return webFile.TempSasUri;
							}
						}
					}
				}
			}

		}
		catch (Exception ex)
		{
			// Handle any exceptions (e.g., invalid URL or image format)
			return null;
		}
	}

	#endregion

	public async Task<bool> CheckCustomerIsCenturian(Guid guid)
	{
		try
		{
			string accessToken = await GetAccessToken();

			string apiUrl = $"{_endpointSettings.Centurion.Url}/api/Gea/isCenturionCustomer?userCode={guid}";

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
				response.EnsureSuccessStatusCode();

				string responseBody = await response.Content.ReadAsStringAsync();
				dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
				bool isCenturionCustomer = responseData.isCenturionCustomer;
				return isCenturionCustomer;
			}
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError($"Error Occurred When Calling Centurion API (/api/Gea/isCenturionCustomer): {ex.Message}");
			throw;
		}
		catch (JsonException ex)
		{
			_logger.LogError($"Error Occurred When Parsing JSON In API (/api/Gea/isCenturionCustomer): {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error Occurred When Calling Centurion API (/api/Gea/isCenturionCustomer): {ex.Message}");
			throw;
		}
	}
	private async Task<string> GetAccessToken()
	{
		try
		{
			var requestPayload = new
			{
				email = _centurionAPIGetTokenCredential.email,
				password = _centurionAPIGetTokenCredential.password
			};
			string jsonPayload = JsonConvert.SerializeObject(requestPayload);

			string apiUrl = $"{_endpointSettings.Centurion.Url}/api/Gea/Token";

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = await httpClient.PostAsync(apiUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
				response.EnsureSuccessStatusCode();

				string responseBody = await response.Content.ReadAsStringAsync();
				dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
				return responseData.token;
			}
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError($"Error Occurred When Calling Centurion API (/api/Gea/Token): {ex.Message}");
			throw;
		}
		catch (JsonException ex)
		{
			_logger.LogError($"Error Occurred When Parsing JSON In API (/api/Gea/Token): {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error Occurred When Calling Centurion API (/api/Gea/Token): {ex.Message}");
			throw;
		}
	}
}



