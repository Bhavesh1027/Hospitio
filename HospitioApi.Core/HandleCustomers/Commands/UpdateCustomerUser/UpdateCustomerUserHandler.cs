using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerUser;
public record UpdateCustomerUserRequest(UpdateCustomerUserIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerUserHandler : IRequestHandler<UpdateCustomerUserRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	private readonly ICommonDataBaseOprationService _commonRepository; private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;

	public UpdateCustomerUserHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions)
	{
		_db = db;
		_response = response;
		_commonRepository = commonRepository;
		_middlewareApiSettingsOptions = middlewareApiSettingsOptions.Value;
	}

	public async Task<AppHandlerResponse> Handle(UpdateCustomerUserRequest request, CancellationToken cancellationToken)
	{
		var customer = new Customer();
		var customeruser = new CustomerUser();

		customer = await _db.Customers.Where(e => e.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
		customeruser = await _db.CustomerUsers.Where(e => e.UserName == request.In.UserName).FirstOrDefaultAsync(cancellationToken);

		if (customer == null)
		{
			return _response.Error($"Customer with Id {request.In.CustomerId} could not be found.", AppStatusCodeError.Gone410);
		}
		if (customeruser == null)
		{
			return _response.Error($"Customer with UserName {request.In.UserName} could not be found.", AppStatusCodeError.Gone410);
		}

		customeruser.FirstName = request.In.FirstName;
		customeruser.LastName = request.In.LastName;
		customeruser.Email = request.In.EmailAddress;
		customer.PhoneCountry = request.In.PhoneCountry;
		customer.PhoneNumber = request.In.PhoneNumber;
		customer.Country = request.In.Country;
		customer.IncomingTranslationLangage = request.In.IncomingTranslationLangage;
		customer.SubscriptionExpirationDate = request.In.SubscriptionExpirationDate;
		customer.IsActive = request.In.IsActive;
		if (request.In.Password != null && request.In.Password != string.Empty)
			customeruser.Password = CryptoExtension.Encrypt(request.In.Password, customer.Id.ToString());

		await _db.SaveChangesAsync(cancellationToken);

		var customerUsers = await _db.CustomerUsers.Where(x => x.CustomerId == customer.Id).Select(x => x.Id).ToListAsync(cancellationToken);
		foreach (var id in customerUsers)
		{
			var customerUser = await _db.CustomerUsers.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
			if (customerUser != null)
			{
				customerUser.IsActive = request.In.IsActive;
			}
			await _db.SaveChangesAsync(cancellationToken);
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
					hasGeaPlatform = request.In.IsActive,
				};

				var middlewareRequestBodyJson = JsonConvert.SerializeObject(middlewareRequestBody);
				requestContent = new StringContent(middlewareRequestBodyJson, Encoding.UTF8, "application/json");

				response = await httpClient.PostAsync(middlewareApiUrl, requestContent);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
				}
			}
		}
		catch (HttpRequestException ex)
		{
			return _response.Error("An HTTP error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);
		}
		catch (JsonException ex)
		{
			return _response.Error("A JSON error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);

		}
		catch (Exception ex)
		{
			return _response.Error("Error occurred when call a Middleware APIS. " + ex.Message, AppStatusCodeError.InternalServerError500);
		}
		#endregion


		return _response.Success(new UpdateCustomerUserOut("Customer update successful.", new() { CustomerId = customer.Id }));
	}
}
