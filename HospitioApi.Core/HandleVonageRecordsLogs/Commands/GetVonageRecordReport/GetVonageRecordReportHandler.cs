using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;
using System.Web;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordReport
{
    public record GetVonageRecordReportRequest(GetVonageRecordReportIn In) : IRequest<AppHandlerResponse>;

    public class GetVonageRecordReportHandler : IRequestHandler<GetVonageRecordReportRequest, AppHandlerResponse>
    {
        private readonly IHandlerResponseFactory _response;
        private readonly ApplicationDbContext _db;
        private readonly VonageSettingsOptions _vonageSettingsOptions;
        private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
        private readonly IUserFilesService _userFilesService;
        private readonly IHubContext<ChatHub> _hubContext;
        public GetVonageRecordReportHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<VonageSettingsOptions> vonageSettingsOptions, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount, IUserFilesService userFilesService, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _response = response;
            _vonageSettingsOptions = vonageSettingsOptions.Value;
            _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
            _userFilesService = userFilesService;
            _hubContext = hubContext;
        }

        public async Task<AppHandlerResponse> Handle(GetVonageRecordReportRequest request, CancellationToken cancellationToken)
        {
            VonageRecordReports vonageRecordReports = null;
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
            string apiUrl = $"https://api.nexmo.com/v2/reports/records";
            var uriBuilder = new UriBuilder(apiUrl);

            string startDate = request.In.Date_start.ToString("o");
            string endDate = request.In.Date_end.ToString("o");

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["account_id"] = $"{apiKey}";
            parameters["product"] = $"{request.In.Product}";
            parameters["direction"] = $"{request.In.Direction}";
            parameters["date_start"] = $"{startDate}";
            parameters["date_end"] = $"{endDate}";
            uriBuilder.Query = parameters.ToString();

            using (var httpClient = new HttpClient())
            {

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_vonageSettingsOptions.APIKey}:{_vonageSettingsOptions.APISecret}");
                string val = Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(uriBuilder.ToString());

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    vonageRecordReports = JsonConvert.DeserializeObject<VonageRecordReports>(responseBody);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Data"); // Sheet name

                        // Add headers
                        int col = 1;
                        worksheet.Cell(1, col).Value = "No";
                        worksheet.Cell(1, col).Style.Font.Bold = true;
                        worksheet.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGray;
                        col++;
                        foreach (var prop in typeof(Record).GetProperties())
                        {
                            var headerCell = worksheet.Cell(1, col);
                            headerCell.Value = prop.Name;
                            // Apply CSS styling to the header cell
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                            col++;
                        }

                        // Add data rows
                        int row = 3;
                        int serialNumber = 1;
                        foreach (var item in vonageRecordReports.records)
                        {
                            col = 1;
                            worksheet.Cell(row, col).Value = serialNumber; // Serial number
                            worksheet.Cell(row, col).Style.Font.Bold = true;
                            worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            col++;
                            foreach (var prop in typeof(Record).GetProperties())
                            {
                                var propValue = prop.GetValue(item);
                                var dataCell = worksheet.Cell(row, col);
                                dataCell.Value = propValue != null ? propValue.ToString() : "";

                                // Apply horizontal alignment to the cell
                                dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                col++;
                            }
                            row++;
                            serialNumber++;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);

                            // Convert the MemoryStream to an IFormFile
                            IFormFile excelFile = new FormFile(stream, 0, stream.Length, "data.xlsx", "data.xlsx");

                            // Upload the IFormFile to Blob Storage
                            string documentName = ((UploadDocumentType)15).ToString();

                            var webFileOut = await _userFilesService.UploadWebFileOnGivenPathAsync(excelFile, documentName, cancellationToken, false);

                            string testURI = webFileOut.TempSasUri.ToString();
                            vonageRecordReports.FileURI = testURI;
                        }
                    }
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return _response.Error($"{responseBody}", AppStatusCodeError.InternalServerError500);
                }
            }
            return _response.Success(new GetVonageRecordReportOut("success", vonageRecordReports));
        }
    }
}
