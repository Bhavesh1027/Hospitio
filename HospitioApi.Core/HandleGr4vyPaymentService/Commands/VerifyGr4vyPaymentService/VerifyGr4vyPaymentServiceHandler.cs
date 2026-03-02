using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Text;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.VerifyGr4vyPaymentService;
public record VerifyGr4vyPaymentServiceRequest(VerifyGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class VerifyGr4vyPaymentServiceHandler : IRequestHandler<VerifyGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    public VerifyGr4vyPaymentServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(VerifyGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var hospitioPaymentProcessor = await _db.PaymentProcessors.Where(p => p.Id == request.In.PaymentProcessorId && p.GRGroup != "digital-wallet").FirstOrDefaultAsync(cancellationToken);
        if (hospitioPaymentProcessor == null)
            return _response.Error($"No verified payment processor found for this service", AppStatusCodeError.InternalServerError500);

        string apiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}payment-services/verify";
        string token = _jwtService.GenerateJWTokenForGr4vy();

        var requestBody = new
        {
            fields = request.In.Fields,
            payment_service_definition_id = hospitioPaymentProcessor.GRID
        };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(apiUrl, content);

        var verify = new VerifyPaymentServiceOut
        {
            IsVerifySuccess = response.IsSuccessStatusCode,
        };

        return _response.Success(new VerifyGr4vyPaymentServiceOut(response.IsSuccessStatusCode ? "Verification payment service successful." : "Verification payment service unsuccessful.", verify));
    }
}
