using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace HospitioApi.Core.HandleCustomerMerchantAccount.Queries.GetCustomerMerchantAccount;
public record GetCustomerMerchantAccountRequest(GetCustomerMerchantAccountIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerMerchantAccountHandler : IRequestHandler<GetCustomerMerchantAccountRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    public GetCustomerMerchantAccountHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions)
    {
        _dapper = dapper;
        _response = response;
        _db = db;
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerMerchantAccountRequest request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.Include(c => c.BusinessType).Where(c => c.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        var existingMerchant = await _db.CustomerPaymentProcessorCredentials.Include(c => c.Customer).ThenInclude(c => c.BusinessType)
           .FirstOrDefaultAsync(c => c.CustomerId == request.In.CustomerId);

        if (existingMerchant != null)
        {
            return _response.Error($"The merchant acount already exists.", AppStatusCodeError.Conflict409);
        }

        var uniqutwostring = GenerateUniqueID(20);

        var biztype = customer.BusinessType.BizType.Substring(0, 2);
        string customermerchantidwithguid = $"{biztype}{uniqutwostring}";
        string customermerchantid = $"{biztype}{request.In.CustomerId}";


        var gr4vyData = await CallGr4vyApi(customermerchantid, customermerchantidwithguid);

        var newMerchant = new CustomerPaymentProcessorCredentials
        {
            CustomerId = request.In.CustomerId,
            MerchantId = gr4vyData.Id,
            IsActive = true,
        };

        await _db.CustomerPaymentProcessorCredentials.AddAsync(newMerchant, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new GetCustomerMerchantAccountOut("Create customer merchant account successfully.", gr4vyData));
    }
    private async Task<CustomerMerchantAccountOut> CallGr4vyApi(string customerId, string customerguid)
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
    private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

    private string GenerateUniqueID(int length)
    {
        // We chose an encoding that fits 6 bits into every character,
        // so we can fit length*6 bits in total.
        // Each byte is 8 bits, so...
        int sufficientBufferSizeInBytes = (length * 6 + 7) / 8;

        var buffer = new byte[sufficientBufferSizeInBytes];
        random.GetBytes(buffer);
        return Convert.ToBase64String(buffer).Substring(0, length);
    }
}

