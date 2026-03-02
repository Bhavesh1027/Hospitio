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

namespace HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;
public record SyncPaymentServiceDefinitionsRequest()
: IRequest<AppHandlerResponse>;
public class SyncPaymentServiceDefinitionsHandler : IRequestHandler<SyncPaymentServiceDefinitionsRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly HttpClient httpClient;
    private readonly ApplicationDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    public SyncPaymentServiceDefinitionsHandler(IHandlerResponseFactory response, ApplicationDbContext db, IJwtService jwtService, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions)
    {
        _response = response;
        httpClient = new HttpClient();
        _db = db;
        _jwtService = jwtService;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(SyncPaymentServiceDefinitionsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var grids = _db.PaymentProcessors.Select(item => item.GRID).Distinct();
            foreach (var grid in grids)
            {
                string apiUrl = _gr4VyApiSettingsOptions.BaseUrl + $"payment-service-definitions/{grid}";
                string token = _jwtService.GenerateJWTokenForGr4vy();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Gr4vyConnectionDefinitionModel apiData = JsonConvert.DeserializeObject<Gr4vyConnectionDefinitionModel>(responseBody);

                    //Gr4vyConnectionDefinitionModel apiData = api.Item;

                    // Retrieve the related PaymentProcessors record based on the grid
                    List<PaymentProcessor> relatedPaymentProcessors = await _db.PaymentProcessors.Where(item => item.GRID == grid).ToListAsync(cancellationToken);

                    // Synchronize the data
                    foreach (var paymentProcessor in relatedPaymentProcessors)
                    {
                        PaymentProcessorsDefinations dbData = await _db.PaymentProcessorsDefinations.Where(p => p.PaymentProcessorId == paymentProcessor.Id).FirstOrDefaultAsync(cancellationToken);

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

                            await _db.PaymentProcessorsDefinations.AddAsync(newRecord, cancellationToken);

                        }
                        else
                        {
                            dbData.GRFields = ObjectStringify.ConvertObjectToString(apiData.Fields);
                            dbData.GRSupportedCountries = ObjectStringify.ConvertObjectToString(apiData.Supported_countries);
                            dbData.GRSupportedCurrencies = ObjectStringify.ConvertObjectToString(apiData.Supported_currencies);
                            dbData.GRSupportedFeatures = ObjectStringify.ConvertObjectToString(apiData.Supported_features);
                            dbData.IsActive = (paymentProcessor.IsActive == false) ? false : true;
                            dbData.UpdateAt = DateTime.UtcNow;
                        }
                        await _db.SaveChangesAsync(cancellationToken);

                    }
                }
            }
            return _response.Success(new SyncPaymentServiceDefinitionsOut("Sync gr4vy payment-service-definition successful."));

        }
        catch (Exception ex)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

    }

}
