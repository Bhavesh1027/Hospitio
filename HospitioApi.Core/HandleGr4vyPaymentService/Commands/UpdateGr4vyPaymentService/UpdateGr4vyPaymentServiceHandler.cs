using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Text;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.UpdateGr4vyPaymentService;
public record UpdateGr4vyPaymentServiceRequest(UpdateGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class UpdateGr4vyPaymentServiceHandler : IRequestHandler<UpdateGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    public UpdateGr4vyPaymentServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(UpdateGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var hospitioPaymentProcessor = await _db.HospitioPaymentProcessors.Where(p => p.Id == request.In.HospitioPaymentProcessorId).FirstOrDefaultAsync(cancellationToken);
        if (hospitioPaymentProcessor == null)
        {
            return _response.Error($"Payment service not found", AppStatusCodeError.Gone410);
        }

        string apiUrl = request.In.IsDigitalWallet
            ? _gr4VyApiSettingsOptions.BaseUrl + $"digital-wallets/{hospitioPaymentProcessor.GRPaymentServiceId}"
            : _gr4VyApiSettingsOptions.BaseUrl + $"payment-services/{hospitioPaymentProcessor.GRPaymentServiceId}";

        string token = _jwtService.GenerateJWTokenForGr4vy();

        var paymentProcessors = await _db.HospitioPaymentProcessors
                .Where(pp => pp.PaymentProcessor.GRCategory == "card" && pp.PaymentProcessor.GRGroup == "payment-service" && pp.IsActive == true && pp.Id != request.In.HospitioPaymentProcessorId)
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
        var response = await _httpClient.PutAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            PaymentServiceOut payment = new PaymentServiceOut();
            if (request.In.IsDigitalWallet == false)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var paymentService = JsonConvert.DeserializeObject<UpdatePaymentServiceOut>(responseBody);
                payment.GRWebhookURL = paymentService.Webhook_Url;
                hospitioPaymentProcessor.GRPaymentServiceId = paymentService.Id;
                hospitioPaymentProcessor.GRWebhookURL = paymentService.Webhook_Url;
                hospitioPaymentProcessor.IsActive = paymentService.Active;
                hospitioPaymentProcessor.GR3DSecureEnabled = paymentService.Three_D_Secure_Enabled;
                hospitioPaymentProcessor.GRAcceptedCurrencies = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Currencies);
                hospitioPaymentProcessor.GRAcceptedCountries = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Countries);
                hospitioPaymentProcessor.GRFields = ObjectStringify.ConvertObjectToString(request.In.paymentService.fields);
                hospitioPaymentProcessor.GRMerchantProfile = (paymentService.Merchant_Profile ?? "").ToString();
                hospitioPaymentProcessor.UpdateAt = DateTime.UtcNow;
                _db.HospitioPaymentProcessors.Update(hospitioPaymentProcessor);
                await _db.SaveChangesAsync(cancellationToken);

                foreach (var processor in paymentProcessors)
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                        _db.HospitioPaymentProcessors.Update(processor);
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
                var walletService = JsonConvert.DeserializeObject<UpdateDigitalWalletOut>(responseBody);

                var updatepayload = new
                {
                    domain_names = walletService.domain_names,
                    merchant_name = walletService.merchant_name,
                    merchant_url = walletService.merchant_url,
                    provider = walletService.provider,
                };

                hospitioPaymentProcessor.GRPaymentServiceId = walletService.id;
                hospitioPaymentProcessor.GRFields = ObjectStringify.ConvertObjectToString(updatepayload);
                hospitioPaymentProcessor.IsActive = request.In.IsActive;
                hospitioPaymentProcessor.UpdateAt = DateTime.UtcNow;

                _db.HospitioPaymentProcessors.Update(hospitioPaymentProcessor);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new UpdateGr4vyPaymentServiceOut("Update payment service successfully.", payment));
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
