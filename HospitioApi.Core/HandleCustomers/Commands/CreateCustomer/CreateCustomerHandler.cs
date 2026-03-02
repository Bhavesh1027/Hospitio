using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
public record CreateCustomerRequest(CreateCustomerIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly IHostingEnvironment Environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SMTPEmailSettingsOptions _smtpEmailSettings;
    private readonly ISendEmail _mail;
    private readonly CustomerCredentialSendEmailOptions _customerCredentialSendEmail;
    private readonly IVonageService _vonageService;
    private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateCustomerHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository, ILogger<CreateCustomerHandler> logger, IHostingEnvironment _environment, IHttpContextAccessor httpContextAccessor, IOptions<SMTPEmailSettingsOptions> smtpEmailSettings, ISendEmail sendEmail, IOptions<CustomerCredentialSendEmailOptions> customerCredentialSendEmail, IVonageService vonageService, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions, IHttpClientFactory httpClientFactory)
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
        _httpClientFactory = httpClientFactory;
    }


    public async Task<AppHandlerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        string email = string.Empty;
        string userName = string.Empty;
        string password = string.Empty;
        var checkExist = await _db.Customers.Where(e => e.BusinessName == request.In.BusinessName).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The business name {request.In.BusinessName} already exists.", AppStatusCodeError.Conflict409);
        }

        if (request.In.CenturianHotelCode != null)
        {
            if (!Guid.TryParse(request.In.CenturianHotelCode, out Guid guidResult))
            {
                return _response.Error($"Invalid Centurian ID. Please provide a valid GUID.", AppStatusCodeError.Conflict409);
            }
            var checkCenturianHotelCodeExist = await _db.Customers.Where(e => e.Guid == guidResult).FirstOrDefaultAsync(cancellationToken);
            if (checkCenturianHotelCodeExist != null)
            {
                return _response.Error($"A customer with this Centurian ID {request.In.CenturianHotelCode} is already registered. Please log in or recover your account details.", AppStatusCodeError.Conflict409);
            }
        }

        var customer = await _commonRepository.CustomersAdd(request.In, _db, cancellationToken);

        if (request.In.CustomerUserIn != null)
        {
            var checkUserName = await _db.CustomerUsers.Where(e => e.UserName == request.In.CustomerUserIn.UserName).FirstOrDefaultAsync(cancellationToken);
            if (checkUserName != null)
            {
                return _response.Error($"The customer user name {request.In.CustomerUserIn.UserName} already exists.", AppStatusCodeError.Conflict409);
            }
            var customerUser = await _commonRepository.CustomersUserAdd(request.In.CustomerUserIn, customer.Id, _db, cancellationToken);


            userName = customerUser.UserName;
            email = customerUser.Email;
            password = request.In.CustomerUserIn.Password;


            var id = customerUser.Id;

        }


        CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _commonRepository.GetPaymentProcessorCredential(customer.Id, _db, cancellationToken);


        var portalBaseUri = new Uri(_httpContextAccessor.HttpContext!.Request.Headers.Referer);

        //string loginPageurl = $@"https://hospitio-customer-dev.appdemoserver.com/";
        string loginPageurl = _customerCredentialSendEmail.CustomerLoginPageURL;

        var fullEmailBody = PopulateEmailTemplate(loginPageurl, email, _smtpEmailSettings.FromEmail, userName, password);

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = _customerCredentialSendEmail.Subject;
        sendEmail.Addresslist = email;
        sendEmail.IsHTML = true;
        sendEmail.Body = fullEmailBody;
        sendEmail.IsNoReply = _customerCredentialSendEmail.IsNoReply;

        await _mail.ExecuteAsync(sendEmail, cancellationToken);

        #region middleware
        var client = _httpClientFactory.CreateClient();

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
            HttpClient httpClient = _httpClientFactory.CreateClient();

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
                Console.WriteLine("Middleware API response: " + result);
            }
            else
            {
                Console.WriteLine("Failed to call the middleware API. Status code: " + response.StatusCode);
            }
        }
        #endregion

        // create subaccount for customer
        var subaccount = await _vonageService.CreateVonageSubAccount(customer.BusinessName!, customer.Id);

        // create application for that subaccount
        var application = await _vonageService.CreateApplication(customer.BusinessName!, customer.Id);


        var customerOut = new CreatedCustomerOut()
        {
            CustomerId = customer.Id,
            BusinessName = customer.BusinessName!,
            UserUniqueId = customer.Guid.ToString(),
        };

        return _response.Success(new CreateCustomerOut("Create customer successful.", customerOut));
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

