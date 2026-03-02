using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.HandleCustomers.Commands.DeleteCustomer;

public record DeleteCustomerRequest(DeleteCustomerIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly MiddlewareApiSettingsOptions _middlewareApiSettingsOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    public DeleteCustomerHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<MiddlewareApiSettingsOptions> middlewareApiSettingsOptions, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _response = response;
        _middlewareApiSettingsOptions = middlewareApiSettingsOptions.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {

        var CustomerData = await _db.Customers.Where(s => s.Id == request.In.Id).FirstOrDefaultAsync();

        if (CustomerData == null)
        {
            return _response.Error($"Customer could not be found.", AppStatusCodeError.Gone410);
        }

        _db.Customers.Remove(CustomerData);

        var CustomerUserData = await _db.CustomerUsers.Where(c => c.CustomerId == request.In.Id).ToListAsync();

        if (CustomerUserData != null || CustomerUserData.Count != 0)
        {
            foreach (var user in CustomerUserData)
            {
                _db.CustomerUsers.Remove(user);
                var channelsOfCustomerUser = await _db.Channels.Where(x => _db.ChannelUserTypeCustomerUser.Where(e => e.UserId == user.Id).Select(e => e.ChannelId).Contains(x.Id)).ToListAsync(cancellationToken);
                _db.Channels.RemoveRange(channelsOfCustomerUser);
            }
        }

        var CustomerRoomData = await _db.CustomerRoomNames.Where(c => c.CustomerId == request.In.Id).ToListAsync();

        if (CustomerRoomData != null || CustomerRoomData.Count != 0)
        {
            foreach (var room in CustomerRoomData)
            {
                _db.CustomerRoomNames.Remove(room);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

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

            var middlewareRequestBody = new
            {
                userCode = CustomerData.Guid,
                hasGeaPlatform = false
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

        return _response.Success(new DeleteCustomerOut("Delete customer successful.", new() { Id = request.In.Id }));
    }
}
