using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using HospitioApi.Core.HandleConnectionDefinations.Commands.SyncConnectionDefinations;
using HospitioApi.Core.Options;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Hospitio.BackGroundService.Receiver;

public class ConnectionDefinationsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly JwtSettingsForGr4vyOptions _jwtSettingsForGr4Vy;
    private readonly BackGroundServicesSettingsOptions _time;

    public ConnectionDefinationsBackgroundService(IServiceProvider serviceProvider, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, IOptions<JwtSettingsForGr4vyOptions> jwtSettingsForGr4Vy, IOptions<BackGroundServicesSettingsOptions> time)
    {
        _serviceProvider = serviceProvider;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _jwtSettingsForGr4Vy = jwtSettingsForGr4Vy.Value;
        _time = time.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Place the logic of SyncConnectionDefinationsHandler.Handle here
            string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + "connection-definitions";

            try
            {
                // Your logic to generate JWT token
                string token = GenerateJWTokenForGr4vy();

                // Create and configure HttpClient
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Make the API request
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        var data = JsonConvert.DeserializeObject<SyncConnectionDefinationsIn>(responseBody);
                        var dataitems = data.Items.Where(m => m.Category == "card" || m.Category == "wallet").ToList();

                        // Your logic for synchronization and database operations
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                            var dbData = await dbContext.PaymentProcessors.ToListAsync(stoppingToken);

                            // Your SynchronizeData method logic here
                            SynchronizeData(dataitems, dbData);

                            var newRecords = dbData.Where(item => item.Id == 0).ToList();
                            var existingRecords = dbData.Where(item => item.Id != 0).ToList();

                            // Add new records to the database
                            dbContext.PaymentProcessors.AddRange(newRecords);
                            //await dbContext.PaymentProcessors.AddRangeAsync(dbData);

                            // Update existing records in the database
                            dbContext.PaymentProcessors.UpdateRange(existingRecords);

                            var existingIds = dbData.Select(item => item.Id).ToList();
                            var itemsToUpdate = await dbContext.PaymentProcessors.Where(item => !existingIds.Contains(item.Id)).ToListAsync();
                            //_db.PaymentProcessors.RemoveRange(itemsToDelete);
                            foreach (var item in itemsToUpdate)
                            {
                                item.IsActive = false;
                            }

                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                    else
                    {
                        // Handle non-success status code
                    }
                }

                // Wait for one month (approximately 30 days) before executing the task again
                await Task.Delay(TimeSpan.Parse(_time.ConnectionDefinationsBackgroundServiceTiming), stoppingToken);
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
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

    public string GenerateJWTokenForGr4vy()
    {
        using ECDsa ecdsaFromPrivateKey = ECDsa.Create();
        ecdsaFromPrivateKey.ImportFromPem(_jwtSettingsForGr4Vy.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsaFromPrivateKey), SecurityAlgorithms.EcdsaSha512)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = new List<Claim>()
        {
            new Claim(CustomClaimTypes.Scopes, "merchant-accounts.write"),
            new Claim(CustomClaimTypes.Scopes, "merchant-accounts.read"),
            new Claim(CustomClaimTypes.Scopes, "buyers.read"),
            new Claim(CustomClaimTypes.Scopes, "buyers.write"),
            new Claim(CustomClaimTypes.Scopes, "connections.read"),
            new Claim(CustomClaimTypes.Scopes, "connections.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-services.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-services.write"),
            new Claim(CustomClaimTypes.Scopes, "transactions.read"),
            new Claim(CustomClaimTypes.Scopes, "transactions.write"),
            new Claim(CustomClaimTypes.Scopes, "digital-wallets.read"),
            new Claim(CustomClaimTypes.Scopes, "digital-wallets.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-methods.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-methods.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-method-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-method-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-service-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-service-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-options.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-options.write"),
            new Claim(CustomClaimTypes.Scopes, "card-scheme-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "card-scheme-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "flows.write"),
        };

        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          subject: new ClaimsIdentity(claims),
          expires: utcNow.Add(TimeSpan.FromMinutes(_jwtSettingsForGr4Vy.JwtValidForMinutes)),
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", _jwtSettingsForGr4Vy.JwtKidKey);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
