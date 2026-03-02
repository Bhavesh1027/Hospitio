using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.IO.Compression;
using System.Net.Http.Headers;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordsStatus
{
    public record GetVonageRecordsStatusRequest(GetVonageRecordsStatusIn In) : IRequest<AppHandlerResponse>;
    public class GetVonageRecordsStatusHandler : IRequestHandler<GetVonageRecordsStatusRequest, AppHandlerResponse>
    {
        private readonly IHandlerResponseFactory _response;
        private readonly ApplicationDbContext _db;
        private readonly VonageSettingsOptions _vonageSettingsOptions;
        private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
        private readonly IUserFilesService _userFilesService;
        private readonly IHubContext<ChatHub> _hubContext;
        public GetVonageRecordsStatusHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<VonageSettingsOptions> vonageSettingsOptions, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount, IUserFilesService userFilesService, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _response = response;
            _vonageSettingsOptions  = vonageSettingsOptions.Value;
            _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
            _userFilesService = userFilesService;
            _hubContext = hubContext;
        }
        public async Task<AppHandlerResponse> Handle(GetVonageRecordsStatusRequest request, CancellationToken cancellationToken)
        {
            WebFileOut responseFromBlob = new();
            using (var httpClient = new HttpClient())
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_vonageSettingsOptions.APIKey}:{_vonageSettingsOptions.APISecret}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var response = await httpClient.GetAsync("https://api.nexmo.com/v3/media/3a7f4176-40cc-40ba-9e7d-53eba0577a51");
                //var response = await httpClient.GetAsync("https://api.nexmo.com/v3/media/7a23cd90-8d81-4e86-a681-a2c9d37711f8");
                var response = await httpClient.GetAsync(request.In._links.download_report.href);

                ResultForRetriveWhatsappAccount resultForRetriveWhatsappAccount = new ResultForRetriveWhatsappAccount();

                if (response.IsSuccessStatusCode)
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (ZipArchive archive = new ZipArchive(stream))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                // Check if the entry is a CSV file (you can modify this condition)
                                if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                                {
                                    using (var streamVsv = entry.Open())
                                    {
                                            string documentName = ((UploadDocumentType)15).ToString();

                                            responseFromBlob = await _userFilesService.UploadCSVFileOnGivenPathAsync(streamVsv, documentName, CancellationToken.None, false, "vonageDemoFileName.csv");
                                    }
                                }
                            }
                        }
                        #region Make Stream of Data
                        //StreamReader reader = new StreamReader(stream);
                        //string csvContent2 = await reader.ReadToEndAsync();
                        //var csv = new CsvReader(reader);
                        //byte[] decodedBytes = Convert.FromBase64String(responseBody);
                        //string filePath = "C:\\Users\\Milan Chudasma\\Downloads\\output.zip"; // Provide a suitable file path
                        //File.WriteAllBytes(filePath, csvContent);
                        #endregion
                    }
                    var newBoj = new
                    {
                        //ExpireAt = responseFromBlob.ExpireAt,
                        //ContentType = responseFromBlob.ContentType,
                        TempSasUri = responseFromBlob.TempSasUri,
                        //Name = responseFromBlob.Name,
                        //Location = responseFromBlob.Location,
                    };
                    var userId = await _db.Users.Where(x => x.UserLevelId == 1).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
                    await _hubContext.Clients.Group($"user-1-{userId}").SendAsync("GetNewVonageRecordsReport", newBoj);
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    resultForRetriveWhatsappAccount.Status = "failed";
                    resultForRetriveWhatsappAccount.Response = responseBody;
                }
            }
            return _response.Success(new GetVonageRecordsStatusOut("success"));
        }
    }
}
