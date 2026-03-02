using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.CreateVonageRecordsReport
{
    public record CreateVonageRecordsReportRequest(CreateVonageRecordsReportIn In) : IRequest<AppHandlerResponse>;
    public class CreateVonageRecordsReportHandler : IRequestHandler<CreateVonageRecordsReportRequest, AppHandlerResponse>
    {
        private readonly IHandlerResponseFactory _response;
        private readonly ApplicationDbContext _db;
        private readonly IVonageService _vonageService;
        private readonly VonageSettingsOptions _vonageSettingsOptions;
        public CreateVonageRecordsReportHandler(IHandlerResponseFactory response, IVonageService vonageService, ApplicationDbContext db, IOptions<VonageSettingsOptions> vonageSettingsOptions)
        {
            _response = response;
            _vonageService = vonageService;
            _db = db;
            _vonageSettingsOptions = vonageSettingsOptions.Value;
        }
        public async Task<AppHandlerResponse> Handle(CreateVonageRecordsReportRequest request, CancellationToken cancellationToken)
        {
            string apiKey = string.Empty;
            if (request.In == null || string.IsNullOrEmpty(request.In.Product) || string.IsNullOrEmpty(request.In.Direction))
            {
                return _response.Error("For Run This Service Please Provide Necessary  Information", AppStatusCodeError.UnprocessableEntity422);
            }
            if (request.In.CustomerId != 0)
            {
                var vonageCredential = await _db.VonageCredentials.Where(x => x.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
                if (vonageCredential == null || vonageCredential.APIKey == null)
                {
                    return _response.Error("For This Customer Vonage Account is Not Found", AppStatusCodeError.UnprocessableEntity422);
                }
                apiKey = vonageCredential.APIKey;
            }
            else
            {
                apiKey = _vonageSettingsOptions.APIKey;
            }

            #region For Refrece
            //from = request.In.From,
            //to = request.In.To,
            //include_subaccounts = request.In.Include_subaccounts,
            //status = request.In.Status,
            //include_message = request.In.Include_message,
            //show_concatenated = request.In.Show_concatenated,
            //date_start = request.In.Date_start,
            //date_end = request.In.Date_end,
            //network = request.In.Network,
            #endregion

            Dictionary<string, object> body = new Dictionary<string, object>
            {
                    { "product", request.In.Product },
                    { "account_id", apiKey },
                    { "direction", request.In.Direction },
                    { "callback_url", $"{_vonageSettingsOptions.HospitioCallBackBaseURL}api/hospitio-admin/vonageRecords/recordStatus" },
            };

            if (request.In.Date_start != null)
            {
                body["date_start"] = request.In.Date_start.ToString("o");
            }

            if (request.In.Date_end != null)
            {
                body["date_end"] = request.In.Date_end.ToString("o");
            }

            if (!string.IsNullOrEmpty(request.In.From))
            {
                body["from"] = request.In.From;
            }

            if (!string.IsNullOrEmpty(request.In.To))
            {
                body["to"] = request.In.To;
            }
            //body["from"] = "919023728519";
            //body["to"] = "919106637194";
            VonageRecordsReportOut vonageRecordsReportOut = null;
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            string apiUrl = $"https://api.nexmo.com/v2/reports";

            using (var httpClient = new HttpClient())
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_vonageSettingsOptions.APIKey}:{_vonageSettingsOptions.APISecret}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    vonageRecordsReportOut = JsonConvert.DeserializeObject<VonageRecordsReportOut>(responseBody);
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return _response.Error($"{responseBody}", AppStatusCodeError.InternalServerError500);
                }
            }
            return _response.Success(new CreateVonageRecordsReportOut("Create ticket category successful.", vonageRecordsReportOut));
        }
    }
}
