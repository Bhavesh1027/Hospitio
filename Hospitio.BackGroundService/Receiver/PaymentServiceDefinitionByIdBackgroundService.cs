using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System.Net.Http.Headers;

namespace Hospitio.BackGroundService.Receiver;

public class PaymentServiceDefinitionByIdBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly BackGroundServicesSettingsOptions _time;

    public PaymentServiceDefinitionByIdBackgroundService(IServiceProvider serviceProvider, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, IOptions<BackGroundServicesSettingsOptions> time)
    {
        _serviceProvider = serviceProvider;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _time = time.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Retrieve the necessary services
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
                var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
                var responseFactory = scope.ServiceProvider.GetRequiredService<IHandlerResponseFactory>();

                try
                {
                    // Get distinct grids from PaymentProcessors
                    var grids = dbContext.PaymentProcessors.Select(item => item.GRID).Distinct();
                    foreach (var grid in grids)
                    {
                        string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + $"payment-service-definitions/{grid}";
                        string token = jwtService.GenerateJWTokenForGr4vy();

                        // Create and configure HttpClient
                        var httpClient = httpClientFactory.CreateClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        // Make the API request
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl, stoppingToken);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Gr4vyConnectionDefinitionModel apiData = JsonConvert.DeserializeObject<Gr4vyConnectionDefinitionModel>(responseBody);

                            // Retrieve the related PaymentProcessors record based on the grid
                            var relatedPaymentProcessors = await dbContext.PaymentProcessors.Where(item => item.GRID == grid).ToListAsync(stoppingToken);

                            // Synchronize the data
                            foreach (var paymentProcessor in relatedPaymentProcessors)
                            {
                                var dbData = await dbContext.PaymentProcessorsDefinations.Where(p => p.PaymentProcessorId == paymentProcessor.Id).FirstOrDefaultAsync(stoppingToken);

                                // Update or add the synchronized data to the PaymentProcessorsDefinations table
                                if (dbData == null)
                                {
                                    // Add a new record to PaymentProcessorsDefinations
                                    PaymentProcessorsDefinations newRecord = new PaymentProcessorsDefinations
                                    {
                                        PaymentProcessorId = paymentProcessor.Id,
                                        GRFields = ObjectStringify.ConvertObjectToString(apiData.Fields),
                                        GRSupportedCountries = ObjectStringify.ConvertObjectToString(apiData.Supported_countries),
                                        GRSupportedCurrencies = ObjectStringify.ConvertObjectToString(apiData.Supported_currencies),
                                        GRSupportedFeatures = ObjectStringify.ConvertObjectToString(apiData.Supported_features),
                                        IsActive = true
                                    };

                                    await dbContext.PaymentProcessorsDefinations.AddAsync(newRecord, stoppingToken);
                                }
                                else
                                {
                                    dbData.GRFields = ObjectStringify.ConvertObjectToString(apiData.Fields);
                                    dbData.GRSupportedCountries = ObjectStringify.ConvertObjectToString(apiData.Supported_countries);
                                    dbData.GRSupportedCurrencies = ObjectStringify.ConvertObjectToString(apiData.Supported_currencies);
                                    dbData.GRSupportedFeatures = ObjectStringify.ConvertObjectToString(apiData.Supported_features);
                                    dbData.IsActive = paymentProcessor.IsActive;
                                    dbData.UpdateAt = DateTime.UtcNow;
                                }
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                }
            }

            // Wait for one month (approximately 30 days) before executing the task again
            await Task.Delay(TimeSpan.Parse(_time.PaymentServiceDefinitionByIdBackgroundServiceTiming), stoppingToken);
        }
    }
}
