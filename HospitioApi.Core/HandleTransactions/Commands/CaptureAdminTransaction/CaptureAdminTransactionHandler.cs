using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Text;

namespace HospitioApi.Core.HandleTransactions.Commands.CaptureAdminTransaction;
public record CaptureAdminTransactionRequest(CaptureAdminTransactionIn In) : IRequest<AppHandlerResponse>;
public class CaptureAdminTransactionHandler : IRequestHandler<CaptureAdminTransactionRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient _httpClient;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly IJwtService _jwtService;
    private readonly ICommonDataBaseOprationService _common;
    public CaptureAdminTransactionHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, IJwtService jwtService, ICommonDataBaseOprationService common)
    {
        _db = db;
        _response = response;
        _httpClient = new HttpClient();
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _jwtService = jwtService;
        _common = common;
    }
    public async Task<AppHandlerResponse> Handle(CaptureAdminTransactionRequest request, CancellationToken cancellationToken)
    {
        string CaptureApiUrl = _gr4VyApiSettingsOptions.BaseUrl + "transactions/{transaction_id}/capture";
        string apiUrl = CaptureApiUrl.Replace("{transaction_id}", request.In.Transaction_Id);

        string token = _jwtService.GenerateJWTokenForGr4vy();

        var requestBody = new
        {
            amount = request.In.Amount,
        };

        var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Add("X-GR4VY-MERCHANT-ACCOUNT-ID", "default");
        var response = await _httpClient.PostAsync(apiUrl, httpContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var capturedTransaction = JsonConvert.DeserializeObject<CapturedAdminTransactionOut>(responseBody);

            return _response.Success(new CaptureAdminTransactionOut("Create ticket category successful.", capturedTransaction));
        }
        else
        {
            return _response.Error(response.Content.ReadAsStringAsync().ToString(), AppStatusCodeError.InternalServerError500);
        }
    }
}
