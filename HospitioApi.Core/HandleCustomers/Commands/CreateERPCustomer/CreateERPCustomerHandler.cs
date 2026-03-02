using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;
public record CreateERPCustomerRequest(CreateERPCustomerIn In) : IRequest<AppHandlerResponse>;
public class CreateERPCustomerHandler : IRequestHandler<CreateERPCustomerRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	private readonly ICommonDataBaseOprationService _commonRepository;
	private readonly ILogger<CreateERPCustomerHandler> _logger;
	private readonly IHostingEnvironment Environment;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly SMTPEmailSettingsOptions _smtpEmailSettings;
	private readonly ISendEmail _mail;
	private readonly CustomerCredentialSendEmailOptions _customerCredentialSendEmail;
	private readonly IVonageService _vonageService;
	private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;

	public CreateERPCustomerHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository, ILogger<CreateERPCustomerHandler> logger, IHostingEnvironment _environment, IHttpContextAccessor httpContextAccessor, IOptions<SMTPEmailSettingsOptions> smtpEmailSettings, ISendEmail sendEmail, IOptions<CustomerCredentialSendEmailOptions> customerCredentialSendEmail, IVonageService vonageService, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions)
	{
		_db = db;
		_response = response;
		_commonRepository = commonRepository;
		_logger = logger;
		Environment = _environment;
		_httpContextAccessor = httpContextAccessor;
		_smtpEmailSettings = smtpEmailSettings.Value;
		_mail = sendEmail;
		_customerCredentialSendEmail = customerCredentialSendEmail.Value;
		_vonageService = vonageService;
		_middlewareApiSettingsOptions = middlewareApiSettingsOptions.Value;
	}


	public async Task<AppHandlerResponse> Handle(CreateERPCustomerRequest request, CancellationToken cancellationToken)
	{
		try
		{
			string? UserUniqueId = null;

			string email = string.Empty;
			string userName = string.Empty;
			string password = string.Empty;

			if (request.In.PylonUniqueCustomerId != null)
			{
				if (!Guid.TryParse(request.In.PylonUniqueCustomerId, out Guid guidResult))
				{
					return _response.Error($"Invalid PylonCustomer ID. Please provide a valid GUID.", AppStatusCodeError.Conflict409);
				}
				var checkCenturianHotelCodeExist = await _db.Customers.Where(e => e.Guid == guidResult).FirstOrDefaultAsync(cancellationToken);
				if (checkCenturianHotelCodeExist != null)
				{
					//return _response.Error($"A customer with this PylonCustomer ID {request.In.PylonUniqueCustomerId} is already registered. Please log in or recover your account details.", AppStatusCodeError.Conflict409);
					if (request.In.NoOfRooms != null && request.In.NoOfRooms != 0 && request.In.NoOfRooms > checkCenturianHotelCodeExist.NoOfRooms)
					{
						checkCenturianHotelCodeExist.NoOfRooms = request.In.NoOfRooms;
						await _db.SaveChangesAsync(cancellationToken);
						UserUniqueId = checkCenturianHotelCodeExist.Guid.ToString();
					}
				}
				else
				{
					var checkExist = await _db.Customers.Where(e => e.BusinessName == request.In.CompanyName).FirstOrDefaultAsync(cancellationToken);
					if (checkExist != null)
					{
						return _response.Error($"The business name {request.In.CompanyName} already exists.", AppStatusCodeError.Conflict409);
					}

					var businessType = await _db.BusinessTypes.Where(b => b.BizType == request.In.BusinessType).FirstOrDefaultAsync(cancellationToken);
					if (businessType == null)
					{
						return _response.Error($"BusinessType {request.In.BusinessType} could not be found.", AppStatusCodeError.Gone410);
					}
					var product = await _db.Products.Where(p => p.Name == request.In.ServicePack).FirstOrDefaultAsync(cancellationToken);
					if (product == null)
					{
						return _response.Error($"Service Pack {request.In.ServicePack} could not be found.", AppStatusCodeError.Gone410);
					}

					var customer = await _commonRepository.ERPCustomersAdd(request.In, _db, cancellationToken);
					UserUniqueId = customer.Guid.ToString();

					var checkUserName = await _db.CustomerUsers.Where(e => e.UserName == request.In.Username).FirstOrDefaultAsync(cancellationToken);
					if (checkUserName != null)
					{
						return _response.Error($"The customer user name {request.In.Username} already exists.", AppStatusCodeError.Conflict409);
					}

					CustomerUserIn customerUserIn = new CustomerUserIn();
					customerUserIn.IsActive = true;
					customerUserIn.CustomerId = customer.Id;
					customerUserIn.FirstName = request.In.FirstName;
					customerUserIn.LastName = request.In.LastName;
					customerUserIn.Email = request.In.Email;
					customerUserIn.UserName = request.In.Username;
					customerUserIn.Password = request.In.Password;

					var customerUser = await _commonRepository.CustomersUserAdd(customerUserIn, customer.Id, _db, cancellationToken);

					if (customerUser != null)
					{
						userName = customerUser?.UserName ?? string.Empty;
						email = customerUser?.Email ?? string.Empty;
						password = customerUserIn.Password ?? string.Empty;

						//var portalBaseUri = new Uri(_httpContextAccessor.HttpContext!.Request.Headers.Referer);

						//string loginPageurl = $@"https://hospitio-customer-dev.appdemoserver.com/";

						string loginPageurl = _customerCredentialSendEmail.CustomerLoginPageURL;

						try
						{
							var fullEmailBody = PopulateEmailTemplate(loginPageurl, email, _smtpEmailSettings.FromEmail, userName, password);

							SendEmailOptions sendEmail = new SendEmailOptions();
							sendEmail.Subject = _customerCredentialSendEmail.Subject;
							sendEmail.Addresslist = email;
							sendEmail.IsHTML = true;
							sendEmail.Body = fullEmailBody;
							sendEmail.IsNoReply = _customerCredentialSendEmail.IsNoReply;

							await _mail.ExecuteAsync(sendEmail, cancellationToken);
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Error is Occured When Sending mail to Customer....");
							return _response.Error("Error is Occured When Sending mail to Customer" + ex.Message, AppStatusCodeError.InternalServerError500);
						}
					}

					#region middleware
					try
					{
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
							string middlewareApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/users";
							httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
							byte? GuidType = customer.GuidType ?? 2;
							var middlewareRequestBody = new
							{
								userCode = customer.Guid,
								hasCenturionPlatform = GuidType == 1 ? true : false,
								hasGeaPlatform = GuidType == 2 || GuidType == 1 ? true : false,
								email = customer.Email,
							};

							var middlewareRequestBodyJson = JsonConvert.SerializeObject(middlewareRequestBody);
							requestContent = new StringContent(middlewareRequestBodyJson, Encoding.UTF8, "application/json");

							response = await httpClient.PostAsync(middlewareApiUrl, requestContent);

							if (response.IsSuccessStatusCode)
							{
								var result = await response.Content.ReadAsStringAsync();
							}
							else
							{
								_logger.LogError("Failed to call the middleware API.Status code: " + response.StatusCode);
							}
						}
					}
					catch (HttpRequestException ex)
					{
						_logger.LogError(ex, "An HTTP error occurred when call a Middleware APIS.");
						return _response.Error("An HTTP error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
					catch (JsonException ex)
					{
						_logger.LogError(ex, "A JSON error occurred when call a Middleware APIS.");
						return _response.Error("A JSON error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);

					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error occurred when call a Middleware APIS.");
						return _response.Error("Error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
					#endregion

					try
					{
						var subaccount = await _vonageService.CreateVonageSubAccount(customer.BusinessName!, customer.Id);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while creating Vonage subaccount.");
						return _response.Error("Failed to create Vonage subaccount. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
					try
					{
						CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _commonRepository.GetPaymentProcessorCredential(customer.Id, _db, cancellationToken);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while getting payment processor credentials.");
						return _response.Error("Failed to get payment processor credentials. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
					try
					{
						var application = await _vonageService.CreateApplication(customer.BusinessName!, customer.Id);
					}
					catch (InvalidOperationException ex)
					{
						_logger.LogInformation(ex, "Vonage Application Creation is Failed");
						return _response.Error("Failed to create Vonage application. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while creating Vonage application.");
						return _response.Error("Failed to create Vonage application. " + ex.Message, AppStatusCodeError.InternalServerError500);
					}
				}
			}

			var customerOut = new CreatedERPCustomerOut()
			{
				PylonUniqueCustomerId = UserUniqueId
			};

			return _response.Success(new CreateERPCustomerOut("Create customer successful.", customerOut));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error Occured When Create ERP Customer....");
			return _response.Error("Error (Create ERP Customer) : " + ex.Message, AppStatusCodeError.InternalServerError500);
		}
	}
	private string PopulateEmailTemplate(string link, string toEmail, string fromEmail, string userName, string password)
	{

		string emailTemplate = string.Empty;
		string path = Path.Combine(this.Environment.WebRootPath, "html/") + _customerCredentialSendEmail.EmailTemplate;

		try
		{
			using (var reader = new StreamReader(path))
			{
				emailTemplate = reader.ReadToEnd();
			}

			if (!string.IsNullOrWhiteSpace(link))
			{
				/* We Insert Reset link Dynamic (passing by caller method) */
				string loginPageURL = $@"<a href=""{link}"" target=""_blank"" style=""
      text-decoration: none;
      display: inline-block;
      color: #ffffff;
      background-color: #5400cf;
      border-radius: 10px;
      width: auto;
      border-top: 0px solid transparent;
      font-weight: undefined;
      border-right: 0px solid transparent;
      border-bottom: 0px solid transparent;
      border-left: 0px solid transparent;
      padding-top: 10px;
      padding-bottom: 10px;
      font-family: Arial, Helvetica Neue, Helvetica, sans-serif;
      font-size: 16px;
      text-align: center;
      mso-border-alt: none;
      word-break: keep-all;
    "">
                            <span style=""
        padding-left: 40px;
        padding-right: 40px;
        font-size: 16px;
        display: inline-block;
        letter-spacing: normal;
      "">
                                <span style=""word-break: break-word; line-height: 32px"">Click To Login</span>
                            </span>
                        </a>";

				string toEmailBody = $@"<a href=""{toEmail}"">{toEmail}</a>";

				string fromEmailBody = $@"<a href=""{fromEmail}"">{fromEmail}</a>.";

				emailTemplate = emailTemplate.Replace("<!--{resetlinkTag}-->", loginPageURL);
				emailTemplate = emailTemplate.Replace("<!--{EmailTag}-->", toEmail);
				emailTemplate = emailTemplate.Replace("<!--{UserNameTag}-->", userName);
				emailTemplate = emailTemplate.Replace("<!--{PasswordTag}-->", password);
				emailTemplate = emailTemplate.Replace("<!--{fromEmailTag}-->", fromEmailBody);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while reading email template in reset password.");
		}
		return emailTemplate;
	}
}
