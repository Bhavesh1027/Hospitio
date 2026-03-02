using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.BackGroundService.Sender;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetAllTransferData;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetTransferDataByGuestId;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Hospitio.BackGroundService.TaxiTransfer;

public class TaxiTransferStatusCheckService : BackgroundService
{
	private readonly BackGroundServicesSettingsOptions _time;
	private readonly ILogger<CustomerQueue> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
	private readonly WelComePickUpsSettingsOptions _welComePickUpsSettings;

	public TaxiTransferStatusCheckService(
		IOptions<BackGroundServicesSettingsOptions> time,
		ILogger<CustomerQueue> logger,
		IServiceProvider serviceProvider,
		IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions,
		IOptions<WelComePickUpsSettingsOptions> welComePickUpsSettings
		)
	{
		_time = time.Value;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
		_welComePickUpsSettings = welComePickUpsSettings.Value;

	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;

				var _dapper = serviceProvider.GetRequiredService<IDapperRepository>();
				var _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
				var _jwtService = serviceProvider.GetRequiredService<IJwtService>();
				var _db = serviceProvider.GetRequiredService<ApplicationDbContext>();

				try
				{
					var spParams = new DynamicParameters();
					spParams.Add("PageNo", 1, DbType.Int32);
					spParams.Add("PageSize", int.MaxValue, DbType.Int32);
					spParams.Add("StatusCheck", true, DbType.Boolean);

					var taxiTransferData = await _dapper.GetAllJsonData<TaxiTransferResponse>("[dbo].[GetAllTaxiTransferData]", spParams, stoppingToken, commandType: CommandType.StoredProcedure);

					if (taxiTransferData != null || taxiTransferData.Count != 0)
					{
						foreach (var response in taxiTransferData)
						{
							#region UpdateTransferStatus -- Update TaxiData and Refund Process
							if (response.TransferStatus == "confirmed")
							{
								using var httpClient = _httpClientFactory.CreateClient();

								var showTransferApiUrl = _welComePickUpsSettings.WelComePickUps_URL + $"v1/external/transfers/{response.TransferId}";
								httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_welComePickUpsSettings.WelComePickUps_APIKey}");

								var showTransferDetailResponse = await httpClient.GetAsync(showTransferApiUrl, stoppingToken);

								if (showTransferDetailResponse.IsSuccessStatusCode)
								{
									var responseBody = await showTransferDetailResponse.Content.ReadAsStringAsync();
									var responseObject = JsonConvert.DeserializeObject<TransferModel>(responseBody);

									if (responseObject != null && responseObject.data.attributes.transfer_status != response.TransferStatus)
									{
										var taxiTransfer = await _db.TaxiTransferGuestRequests.FirstOrDefaultAsync(t => t.Id == response.Id, stoppingToken);
										if (taxiTransfer != null)
										{
											taxiTransfer.TransferStatus = responseObject.data.attributes.transfer_status;
											taxiTransfer.TransferJson = responseBody;

											if (taxiTransfer.RefundId == null && responseObject.data.attributes.transfer_status != "operated")
											{
												using var gr4VyHttpClient = _httpClientFactory.CreateClient();

												var captureApiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}transactions/{response.GRPaymentId}/refunds";
												var amount = (int.Parse(responseObject.data.attributes.fare.refund.policy.refund_percentage) / 100) * taxiTransfer.HospitioFareAmount;
												amount = Math.Round(amount ?? 0, 2) * 100;
												var requestBody = new { amount = amount };

												var json = JsonConvert.SerializeObject(requestBody);
												var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

												gr4VyHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtService.GenerateJWTokenForGr4vy());
												var detailResponse = await gr4VyHttpClient.PostAsync(captureApiUrl, httpContent, stoppingToken);

												if (detailResponse.IsSuccessStatusCode)
												{
													var refundResponseBody = await detailResponse.Content.ReadAsStringAsync();
													dynamic jsonResponse = JObject.Parse(refundResponseBody);

													taxiTransfer.GRPaymentDetails = refundResponseBody;
													taxiTransfer.RefundId = jsonResponse.id;
													taxiTransfer.RefundStatus = jsonResponse.status;

													taxiTransfer.RefundAmount = (int.Parse(responseObject.data.attributes.fare.refund.policy.refund_percentage) / 100) * taxiTransfer.HospitioFareAmount;
													taxiTransfer.RefundAmount = Math.Round(taxiTransfer.RefundAmount ?? 0, 2);
												}
											}

											await _db.SaveChangesAsync(stoppingToken);
										}
									}
								}
							}
							#endregion

							#region UpdateRefundStatus -- Update RefundStatus
							if (response.RefundStatus == "processing" && response.RefundId != null)
							{
								using var gr4VyHttpClient = _httpClientFactory.CreateClient();

								var apiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}transactions/{response.GRPaymentId}/refunds/{response.RefundId}";
								var token = _jwtService.GenerateJWTokenForGr4vy();

								gr4VyHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
								var detailResponse = await gr4VyHttpClient.GetAsync(apiUrl, stoppingToken);

								if (detailResponse.IsSuccessStatusCode)
								{
									var responseBody = await detailResponse.Content.ReadAsStringAsync();
									var jsonResponse = JObject.Parse(responseBody);


									var taxiTransfer = await _db.TaxiTransferGuestRequests.FirstOrDefaultAsync(t => t.Id == response.Id, stoppingToken);
									if (taxiTransfer != null)
									{
										dynamic tempJsonResponse = jsonResponse;
										taxiTransfer.RefundStatus = tempJsonResponse.status;
										await _db.SaveChangesAsync(stoppingToken);
									}

								}
							}
							#endregion
						}
					}
				}

				catch (Exception ex)
				{
					_logger.LogError(ex, "Error during process of checking TaxiTransferData status - TaxiTransferStatusCheckService services");
				}
			}

			_logger.LogInformation($"TaxiTransferStatusCheckService services Running{DateTime.Now}");

			await Task.Delay(TimeSpan.Parse(_time.TaxiTransferStatusCheckServiceTiming), stoppingToken);
		}
	}




}