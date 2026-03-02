using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Text;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.UpdateCustomerGr4vyPaymentService;
public record UpdateCustomerGr4vyPaymentServiceRequest(UpdateCustomerGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerGr4vyPaymentServiceHandler : IRequestHandler<UpdateCustomerGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly ICommonDataBaseOprationService _common;
    public UpdateCustomerGr4vyPaymentServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, ICommonDataBaseOprationService common)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _common = common;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var customerPaymentProcessor = await _db.CustomerPaymentProcessors.Where(p => p.Id == request.In.CustomerPaymentProcessorId).FirstOrDefaultAsync(cancellationToken);
        if (customerPaymentProcessor == null)
        {
            return _response.Error($"Payment service not found", AppStatusCodeError.Gone410);
        }

        int nonNullableCustomerId = request.In.CustomerId ?? 0;

        CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _common.GetPaymentProcessorCredential(nonNullableCustomerId, _db, cancellationToken);

        string apiUrl = request.In.IsDigitalWallet
            ? _gr4VyApiSettingsOptions.BaseUrl + $"digital-wallets/{customerPaymentProcessor.GRPaymentServiceId}"
            : _gr4VyApiSettingsOptions.BaseUrl + $"payment-services/{customerPaymentProcessor.GRPaymentServiceId}";

        string token = _jwtService.GenerateJWTokenForGr4vy();

        var paymentProcessors = await _db.CustomerPaymentProcessors
                .Where(pp => pp.PaymentProcessor.GRCategory == "card" && pp.PaymentProcessor.GRGroup == "payment-service" && pp.IsActive == true && pp.Id != request.In.CustomerPaymentProcessorId && pp.CustomerId == request.In.CustomerId)
                .ToListAsync(cancellationToken);

        dynamic updatepayloadforapi = null;
        if (request.In.IsDigitalWallet == false)
        {
            updatepayloadforapi = new
            {
                active = request.In.IsActive,
                accepted_currencies = request.In.paymentService.accepted_currencies,
                accepted_countries = request.In.paymentService.accepted_countries,
                fields = request.In.paymentService.fields,
            };
        }
        if (request.In.IsDigitalWallet == true)
        {
            updatepayloadforapi = new
            {
                domain_names = request.In.digitalWallet.domain_names,
                merchant_name = request.In.digitalWallet.merchant_name,
            };
        }

        var payload = request.In.IsDigitalWallet
                ? SerializeDigitalWalletPayload(updatepayloadforapi)
                : SerializePaymentProcessorPayload(updatepayloadforapi);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", customerPaymentProcessorCredentials.MerchantId);
        var response = await _httpClient.PutAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            PaymentServiceOut payment = new PaymentServiceOut();
            if (request.In.IsDigitalWallet == false)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var paymentService = JsonConvert.DeserializeObject<UpdateCustomerPaymentServiceOut>(responseBody);
                payment.GRWebhookURL = paymentService.Webhook_Url;
                customerPaymentProcessor.GRPaymentServiceId = paymentService.Id;
                customerPaymentProcessor.GRWebhookURL = paymentService.Webhook_Url;
                customerPaymentProcessor.IsActive = paymentService.Active;
                customerPaymentProcessor.GR3DSecureEnabled = paymentService.Three_D_Secure_Enabled;
                customerPaymentProcessor.GRAcceptedCurrencies = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Currencies);
                customerPaymentProcessor.GRAcceptedCountries = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Countries);
                customerPaymentProcessor.GRFields = ObjectStringify.ConvertObjectToString(request.In.paymentService.fields);
                customerPaymentProcessor.GRMerchantProfile = (paymentService.Merchant_Profile ?? "").ToString();
                customerPaymentProcessor.UpdateAt = DateTime.UtcNow;
                _db.CustomerPaymentProcessors.Update(customerPaymentProcessor);
                await _db.SaveChangesAsync(cancellationToken);

                foreach (var processor in paymentProcessors)
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", customerPaymentProcessorCredentials.MerchantId);
                    var grPaymentServiceId = processor.GRPaymentServiceId;
                    var apiupdateurl = _gr4VyApiSettingsOptions.BaseUrl + $"payment-services/{grPaymentServiceId}";

                    // Create a JSON payload to set active to false
                    var updatepayload = new
                    {
                        active = false
                    };
                    var updatecontent = new StringContent(JsonConvert.SerializeObject(updatepayload), Encoding.UTF8, "application/json");

                    // Send the PUT request to the update API
                    var updateresponse = await httpClient.PutAsync(apiupdateurl, updatecontent);

                    if (updateresponse.IsSuccessStatusCode)
                    {
                        processor.IsActive = false;
                        _db.CustomerPaymentProcessors.Update(processor);
                        await _db.SaveChangesAsync(cancellationToken);
                        Console.WriteLine($"Successfully updated GR Payment Service ID: {grPaymentServiceId}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update GR Payment Service ID: {grPaymentServiceId}");
                    }
                }

            }
            if (request.In.IsDigitalWallet == true)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var walletService = JsonConvert.DeserializeObject<UpdateCustomerDigitalWalletOut>(responseBody);

                var updatepayload = new
                {
                    domain_names = walletService.domain_names,
                    merchant_name = walletService.merchant_name,
                    merchant_url = walletService.merchant_url,
                    provider = walletService.provider,
                };

                customerPaymentProcessor.GRPaymentServiceId = walletService.id;
                customerPaymentProcessor.GRFields = ObjectStringify.ConvertObjectToString(updatepayload);
                customerPaymentProcessor.IsActive = request.In.IsActive;
                customerPaymentProcessor.UpdateAt = DateTime.UtcNow;

                _db.CustomerPaymentProcessors.Update(customerPaymentProcessor);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new UpdateCustomerGr4vyPaymentServiceOut("Update customer payment service successfully.", payment));
        }
        else
        {
            return _response.Error(response.Content.ReadAsStringAsync().ToString(), AppStatusCodeError.InternalServerError500);
        }
    }
    private static string SerializeDigitalWalletPayload(dynamic? digitalWallet)
    {
        return JsonConvert.SerializeObject(digitalWallet);
    }
    private static string SerializePaymentProcessorPayload(dynamic? paymentService)
    {
        return JsonConvert.SerializeObject(paymentService);
    }
}
