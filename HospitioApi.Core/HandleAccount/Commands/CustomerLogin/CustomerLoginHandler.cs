using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.CustomerLogin;

public record CustomerLoginHandlerRequest(CustomerLoginIn In)
	: IRequest<AppHandlerResponse>;

public class CustomerLoginHandler : IRequestHandler<CustomerLoginHandlerRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	private readonly IJwtService _jwtService;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;

	public CustomerLoginHandler(
		ApplicationDbContext db,
		IHandlerResponseFactory response,
		IJwtService jwtService,
		IHttpContextAccessor httpContextAccessor, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions)
	{
		_db = db;
		_response = response;
		_jwtService = jwtService;
		_httpContextAccessor = httpContextAccessor;
		_middlewareApiSettingsOptions = middlewareApiSettingsOptions.Value;
	}

	public async Task<AppHandlerResponse> Handle(CustomerLoginHandlerRequest request, CancellationToken cancellationToken)
	{
		//var customerUsers = await _db.CustomerUsers.Include(x => x.Customer).ThenInclude(x => x.Product).IgnoreQueryFilters()
		//    .Where(u => u.Email == request.In.Email && u.Password == request.In.Password).SingleOrDefaultAsync(cancellationToken);

		var customerUsers = await _db.CustomerUsers.Include(x => x.Customer).Include(p => p.CustomerLevel).Include(c => c.CustomerUsersPermissions).ThenInclude(p => p.CustomerPermission).IgnoreQueryFilters()
		   .Where(u => (u.Email == request.In.Email || u.UserName == request.In.Email)
			&& u.IsActive == true && u.DeletedAt == null && u.Customer.IsActive == true)
		   .SingleOrDefaultAsync(cancellationToken);

		if (customerUsers != null)
		{
			var encryptedPassword = CryptoExtension.Encrypt(request.In.Password, customerUsers.CustomerId.ToString());

			if (customerUsers == null || customerUsers.Password != encryptedPassword)
			{
				/** Do not expose to the client the fact that email does not exists */
				return _response.Error("Invalid CustomerLogin attempt.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
			}

			var customer = await _db.Customers.Where(c => c.Id == customerUsers.CustomerId).FirstOrDefaultAsync(cancellationToken);

			//if (customer?.SubscriptionExpirationDate != null)
			//{
			//    DateTime customerExpirationDate = customer.SubscriptionExpirationDate.Value;
			//    DateTime newExpirationDate = customerExpirationDate.AddDays(20);

			//    if (newExpirationDate < DateTime.UtcNow)
			//    {
			//        customer.IsActive = false;
			//        customerUsers.IsActive = false;

			//        await _db.SaveChangesAsync(cancellationToken);

			//        #region middleware
			//        using var client = new HttpClient();

			//        string firstApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/token";

			//        var requestBody = new
			//        {
			//            username = _middlewareApiSettingsOptions.UserName,
			//            password = _middlewareApiSettingsOptions.Password,
			//        };

			//        var requestBodyJson = JsonConvert.SerializeObject(requestBody);
			//        var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

			//        // Send a POST request to obtain the token
			//        var response = await client.PostAsync(firstApiUrl, requestContent);

			//        string? token = null;
			//        if (response.IsSuccessStatusCode)
			//        {
			//            // Read the token from the response
			//            var responseContent = await response.Content.ReadAsStringAsync();
			//            var responseObject = JObject.Parse(responseContent);

			//            if (responseObject != null && responseObject["token"] != null)
			//            {
			//                token = responseObject["token"].Value<string>();
			//            }
			//        }

			//        if (!string.IsNullOrEmpty(token))
			//        {
			//            HttpClient httpClient = new HttpClient();
			//            string middlewareApiUrl = _middlewareApiSettingsOptions.BaseUrl + "api/ChannelManager/users";
			//            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

			//            var middlewareRequestBody = new
			//            {
			//                userCode = customer.Guid,
			//                hasGeaPlatform = false
			//            };

			//            var middlewareRequestBodyJson = JsonConvert.SerializeObject(middlewareRequestBody);
			//            requestContent = new StringContent(middlewareRequestBodyJson, Encoding.UTF8, "application/json");

			//            response = await httpClient.PostAsync(middlewareApiUrl, requestContent);

			//            if (response.IsSuccessStatusCode)
			//            {
			//                var result = await response.Content.ReadAsStringAsync();
			//                Console.WriteLine("Middleware API response: " + result);
			//            }
			//            else
			//            {
			//                Console.WriteLine("Failed to call the middleware API. Status code: " + response.StatusCode);
			//            }
			//        }
			//        #endregion

			//        return _response.Error("Your software subscription has expired. Please renew your subscription.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);

			//    }
			//}

			var tokenExpiresUtc = _jwtService.GetRefreshTokenExpirationUtc();
			var hubconnectionExpiresUtc = _jwtService.GetHubConnectionTokenExpirationUtc();
			var refreshTokenValue = _jwtService.GenerateRefreshTokenValue(customerUsers.Id);

			var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

			var dbRefreshToken = new Data.Models.CustomerUserRefreshToken(customerUsers.Id, refreshTokenValue, tokenExpiresUtc, ipAddress);

			await _db.CustomerUserRefreshTokens.AddAsync(dbRefreshToken, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);

			var dtoAccessToken = await _jwtService.GenerateJWTokenForCustomerAsync(customerUsers, cancellationToken);
			var dtoChatHubConnectionAccessToken = await _jwtService.GenerateJWTokenForChatHubConnectionCustomerAsync(customerUsers, cancellationToken);
			var dtoRefreshToken = new CustomerUserRefreshToken(dbRefreshToken.Token, dbRefreshToken.Id, new(dbRefreshToken.ExpiresUtc));
			var dtoChatHubConnectionToken = new ChatHubConnectionToken(dtoChatHubConnectionAccessToken, new(hubconnectionExpiresUtc));
			bool isMuted = customerUsers.IsMuted;

			return _response.Success(new CustomerLoginOut("Customer login successful.", dtoAccessToken, dtoRefreshToken, dtoChatHubConnectionToken, isMuted));
		}

		return _response.Error("Invalid CustomerLogin attempt.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
	}
}