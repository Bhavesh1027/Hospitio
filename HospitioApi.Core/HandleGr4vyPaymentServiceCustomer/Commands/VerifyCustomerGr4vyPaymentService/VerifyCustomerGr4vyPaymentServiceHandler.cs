using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.VerifyCustomerGr4vyPaymentService;
public record VerifyCustomerGr4vyPaymentServiceRequest(VerifyCustomerGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class VerifyCustomerGr4vyPaymentServiceHandler : IRequestHandler<VerifyCustomerGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly ICommonDataBaseOprationService _common;
    public VerifyCustomerGr4vyPaymentServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, ICommonDataBaseOprationService common)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _common = common;
    }
    public async Task<AppHandlerResponse> Handle(VerifyCustomerGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var hospitioPaymentProcessor = await _db.PaymentProcessors.Where(p => p.Id == request.In.PaymentProcessorId && p.GRGroup != "digital-wallet").FirstOrDefaultAsync(cancellationToken);
        if (hospitioPaymentProcessor == null)
            return _response.Error($"No verified payment processor found for this service", AppStatusCodeError.InternalServerError500);

        int nonNullableCustomerId = request.In.CustomerId ?? 0;

        CustomerPaymentProcessorCredentials customerPaymentProcessorCredentials = await _common.GetPaymentProcessorCredential(nonNullableCustomerId, _db, cancellationToken);

        string apiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}payment-services/verify";
        string token = _jwtService.GenerateJWTokenForGr4vy();

        var requestBody = new
        {
            fields = request.In.Fields,
            payment_service_definition_id = hospitioPaymentProcessor.GRID
        };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", customerPaymentProcessorCredentials.MerchantId);
        var response = await _httpClient.PostAsync(apiUrl, content);

        var verify = new VerifyCustomerPaymentServiceOut
        {
            IsVerifySuccess = response.IsSuccessStatusCode,
        };

        return _response.Success(new VerifyCustomerGr4vyPaymentServiceOut(response.IsSuccessStatusCode ? "Verification payment service successful." : "Verification payment service unsuccessful.", verify));
    }
}
