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

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.CreateCustomerGr4vyPaymentService;
public record CreateCustomerGr4vyPaymentServiceRequest(CreateCustomerGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerGr4vyPaymentServiceHandler : IRequestHandler<CreateCustomerGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly ICommonDataBaseOprationService _common;
    public CreateCustomerGr4vyPaymentServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, ICommonDataBaseOprationService common)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _common = common;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var customerPaymentProcessor = await _db.CustomerPaymentProcessors.Where(p => p.PaymentProcessorId == request.In.PaymentProcessorId && p.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (customerPaymentProcessor != null)
        {
            return _response.Error($"Payment service already configured for this customer", AppStatusCodeError.UnprocessableEntity422);
        }

        int nonNullableCustomerId = request.In.CustomerId ?? 0;

        CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _common.GetPaymentProcessorCredential(nonNullableCustomerId, _db, cancellationToken);

        string apiUrl = request.In.IsDigitalWallet
            ? _gr4VyApiSettingsOptions.BaseUrl + "digital-wallets"
            : _gr4VyApiSettingsOptions.BaseUrl + "payment-services";

        string token = _jwtService.GenerateJWTokenForGr4vy();

        if (request.In.IsDigitalWallet == false)
        {
            PaymentProcessor paymentProcessor = await _db.PaymentProcessors.Where(p => p.Id == request.In.PaymentProcessorId).FirstOrDefaultAsync(cancellationToken);
            request.In.paymentService.payment_service_definition_id = paymentProcessor.GRID;
            request.In.paymentService.display_name = paymentProcessor.GRName;
        }

        var paymentProcessors = await _db.CustomerPaymentProcessors
                .Where(pp => pp.PaymentProcessor.GRCategory == "card" && pp.PaymentProcessor.GRGroup == "payment-service" && pp.IsActive == true && pp.CustomerId == request.In.CustomerId)
                .ToListAsync(cancellationToken);

        var payload = request.In.IsDigitalWallet
                ? SerializeDigitalWalletPayload(request.In.digitalWallet)
                : SerializePaymentProcessorPayload(request.In.paymentService);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", customerPaymentProcessorCredentials.MerchantId);
        var response = await _httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            PaymentServiceOut payment = new PaymentServiceOut();
            if (request.In.IsDigitalWallet == false)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var paymentService = JsonConvert.DeserializeObject<AddCustomerPaymentServiceOut>(responseBody);
                payment.GRWebhookURL = paymentService.Webhook_Url;
                var newPaymentProcessor = new CustomerPaymentProcessor
                {
                    CustomerId = request.In.CustomerId,
                    PaymentProcessorId = request.In.PaymentProcessorId,
                    GRPaymentServiceId = paymentService.Id,
                    GRWebhookURL = paymentService.Webhook_Url,
                    IsActive = paymentService.Active,
                    GR3DSecureEnabled = paymentService.Three_D_Secure_Enabled,
                    GRAcceptedCurrencies = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Currencies),
                    GRAcceptedCountries = ObjectStringify.ConvertObjectToString(paymentService.Accepted_Countries),
                    GRFields = ObjectStringify.ConvertObjectToString(request.In.paymentService.fields),
                    GRIsDeleted = paymentService.Is_Deleted,
                    GRMerchantProfile = (paymentService.Merchant_Profile ?? "").ToString()
                };
                await _db.CustomerPaymentProcessors.AddAsync(newPaymentProcessor, cancellationToken);
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
                        Console.WriteLine($"Successfully updated customer GR Payment Service ID: {grPaymentServiceId}");
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
                var walletService = JsonConvert.DeserializeObject<AddCustomerDigitalWalletOut>(responseBody);

                var updatepayload = new
                {
                    domain_names = walletService.domain_names,
                    merchant_name = walletService.merchant_name,
                    merchant_url = walletService.merchant_url,
                    provider = walletService.provider,
                };

                var newPaymentProcessor = new CustomerPaymentProcessor
                {
                    CustomerId = request.In.CustomerId,
                    PaymentProcessorId = request.In.PaymentProcessorId,
                    GRPaymentServiceId = walletService.id,
                    GRFields = ObjectStringify.ConvertObjectToString(updatepayload),
                    IsActive = request.In.IsActive,
                };
                await _db.CustomerPaymentProcessors.AddAsync(newPaymentProcessor, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return _response.Success(new CreateCustomerGr4vyPaymentServiceOut("Create customer payment service successfully.", payment));
        }
        else
        {
            return _response.Error(response.Content.ReadAsStringAsync().ToString(), AppStatusCodeError.InternalServerError500);
        }
    }
    private static string SerializeDigitalWalletPayload(DigitalWallet? digitalWallet)
    {
        return JsonConvert.SerializeObject(digitalWallet);
    }
    private static string SerializePaymentProcessorPayload(PaymentService? paymentService)
    {
        return JsonConvert.SerializeObject(paymentService);
    }
}
