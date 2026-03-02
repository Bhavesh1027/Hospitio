using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Net.Http.Headers;

namespace HospitioApi.Core.HandleConnectionDefinations.Commands.SyncConnectionDefinations;
public record SyncConnectionDefinationsRequest()
: IRequest<AppHandlerResponse>;
public class SyncConnectionDefinationsHandler : IRequestHandler<SyncConnectionDefinationsRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient httpClient;
    private readonly ApplicationDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    public SyncConnectionDefinationsHandler(IHandlerResponseFactory response, ApplicationDbContext db, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions)
    {
        _response = response;
        httpClient = new HttpClient();
        _db = db;
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(SyncConnectionDefinationsRequest request, CancellationToken cancellationToken)
    {
        string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + "connection-definitions";

        try
        {
            string token = _jwtService.GenerateJWTokenForGr4vy();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<SyncConnectionDefinationsIn>(responseBody);
                var dataitems = data.Items.Where(m => m.Category == "card" || m.Category == "wallet").ToList();

                var dbData = await _db.PaymentProcessors.ToListAsync(cancellationToken);

                SynchronizeData(dataitems, dbData);

                var newRecords = dbData.Where(item => item.Id == 0).ToList();
                var existingRecords = dbData.Where(item => item.Id != 0).ToList();

                // Add new records to the database
                _db.PaymentProcessors.AddRange(newRecords);
                //await _db.PaymentProcessors.AddRangeAsync(dbData);

                // Update existing records in the database
                _db.PaymentProcessors.UpdateRange(existingRecords);

                var existingIds = dbData.Select(item => item.Id).ToList();
                var itemsToUpdate = await _db.PaymentProcessors.Where(item => !existingIds.Contains(item.Id)).ToListAsync();
                //_db.PaymentProcessors.RemoveRange(itemsToDelete);
                foreach (var item in itemsToUpdate)
                {
                    item.IsActive = false;
                }

                await _db.SaveChangesAsync(cancellationToken);
                return _response.Success(new SyncConnectionDefinationsOut("Sync gr4vy connection-definitions successful."));
            }
            else
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }
        }
        catch (Exception ex)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
    }
    private static void SynchronizeData(List<ConnectionDefinition> apiData, List<PaymentProcessor> dbData)
    {
        // Create a dictionary to store the existing records in the database for quick lookup
        Dictionary<string, PaymentProcessor> dbDataDict = dbData.ToDictionary(x => x.GRID);

        foreach (var apiItem in apiData)
        {
            // Check if the API item exists in the database
            if (dbDataDict.TryGetValue(apiItem.Id, out var dbItem))
            {
                // Update the existing record in the database
                dbItem.GRID = apiItem.Id;
                dbItem.GRIcon = apiItem.Icon_Url;
                dbItem.GRName = apiItem.Name;
                dbItem.GRGroup = apiItem.Group;
                dbItem.GRCategory = apiItem.Category;
            }
            else
            {
                // Add a new record to the database
                var newDbItem = new PaymentProcessor
                {
                    GRID = apiItem.Id,
                    GRIcon = apiItem.Icon_Url,
                    GRName = apiItem.Name,
                    GRGroup = apiItem.Group,
                    GRCategory = apiItem.Category,
                    IsActive = true
                };
                dbData.Add(newDbItem);
            }
        }

        // Delete extra records from the database that are not present in the API data
        List<PaymentProcessor> itemsToDelete = dbData.Where(dbItem => !apiData.Any(apiItem => apiItem.Id == dbItem.GRID)).ToList();
        foreach (var itemToDelete in itemsToDelete)
        {
            dbData.Remove(itemToDelete);
        }
    }

}
