using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;

namespace Hospitio.BackGroundService.Vonage
{
    public class TemplateStatusCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly VonageSettingsOptions _vonageSettingsOptions;
        private readonly BackGroundServicesSettingsOptions _time;
        public TemplateStatusCheckService(IServiceProvider serviceProvider, IOptions<VonageSettingsOptions> vonageSettingsOptions, IOptions<BackGroundServicesSettingsOptions> time)
        {
            _serviceProvider = serviceProvider;
            _vonageSettingsOptions = vonageSettingsOptions.Value;
            _time = time.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var _vonageService = scope.ServiceProvider.GetRequiredService<IVonageService>();

                var VonageCreds = await _db.VonageCredentials.Where(x => x.WABAId == null || x.WABAId == "").ToListAsync(stoppingToken);
                foreach (var cred in VonageCreds)
                {
                    var customerVonageCred = await _db.VonageCredentials.Where(x => x.Id == cred.Id).FirstOrDefaultAsync(stoppingToken);
                    if (customerVonageCred != null)
                    {
                        var CustomerInfo = await _db.Customers.Where(x => x.Id == customerVonageCred.CustomerId).FirstOrDefaultAsync(stoppingToken);

                        if (CustomerInfo != null && CustomerInfo.WhatsappNumber != null && customerVonageCred.AppId != null && customerVonageCred.AppPrivatKey != null)
                        {
                            var response = await _vonageService.RetriveWhatsappAccount(customerVonageCred.AppId, customerVonageCred.AppPrivatKey, CustomerInfo.WhatsappNumber);
                            if (response != null && response.Status == "success")
                            {
                                var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Response);
                                string WABAId = responseBody.aggregate_id;
                                string Provider = responseBody.provider;
                                string Name = responseBody.name;

                                if (Provider == "whatsapp")
                                {
                                    customerVonageCred.WABAId = WABAId;
                                    await _db.SaveChangesAsync(stoppingToken);
                                }
                            }
                        }
                    }
                }

                #region RemoveChatWhichIsInactiveMoreThanDay

                var cutoffDate = DateTime.UtcNow.AddDays(-1);

                var channelsToRemove = await _db.Channels
                    .Where(c =>
                        c.CreateForm == UserTypeEnum.ChatWidgetUser.ToString() &&
                        _db.ChannelMessages
                            .Where(cm => cm.ChannelId == c.Id)
                            .Max(cm => cm.CreatedAt) < cutoffDate
                    )
                    .ToListAsync(stoppingToken);

                foreach (var channelInfo in channelsToRemove)
                {
                    var channelId = channelInfo.Id;

                    _db.Channels.Remove(channelInfo);

                    var messagesToDelete = await _db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .ToListAsync(stoppingToken);

                    if (messagesToDelete.Any())
                    {
                        _db.ChannelMessages.RemoveRange(messagesToDelete);
                    }

                    var usersToDelete = await _db.ChannelUsers
                        .Where(cu => cu.ChannelId == channelId)
                        .ToListAsync(stoppingToken);

                    _db.ChannelUsers.RemoveRange(usersToDelete);
                }

                await _db.SaveChangesAsync(stoppingToken);
                #endregion

                #region admin template status check

                var guestJourneyMessageTemplates = await _db.GuestJourneyMessagesTemplates.Where(e => e.VonageTemplateStatus == "PENDING").ToListAsync();
                var adminStaffAlertTemplates = await _db.AdminStaffAlerts.Where(e => e.VonageTemplateStatus == "PENDING").ToListAsync();


                if ((guestJourneyMessageTemplates != null && guestJourneyMessageTemplates.Count > 0) || (adminStaffAlertTemplates != null && adminStaffAlertTemplates.Count > 0))
                {

                    var token = await _vonageService.GenerateJWT(_vonageSettingsOptions.AppId, _vonageSettingsOptions.AppPrivatKey);

                    // Vonage Template Management API URL
                    string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{_vonageSettingsOptions.WABAId}/templates";

                    using (var httpClient = new HttpClient())
                    {
                        // Set JWT token in request headers for authorization
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        // Set content type
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Send GET request to create the template
                        var response = await httpClient.GetAsync(apiUrl);

                        //Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("List Template successfully. Response: " + responseBody);
                            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                            var templates = jsonResponse.templates;
                            foreach (var template in guestJourneyMessageTemplates)
                            {
                                foreach (var t in templates)
                                {
                                    if (t["id"].ToString() == template.VonageTemplateId)
                                    {
                                        string status = t["status"].ToString();
                                        template.VonageTemplateStatus = status;
                                        await _db.SaveChangesAsync(CancellationToken.None);
                                        // Save the updated database template back to your database
                                        break; // Exit the inner loop once a match is found
                                    }
                                }
                            }
                            foreach (var template in adminStaffAlertTemplates)
                            {
                                foreach (var t in templates)
                                {
                                    if (t["id"].ToString() == template.VonageTemplateId)
                                    {
                                        string status = t["status"].ToString();
                                        template.VonageTemplateStatus = status;
                                        await _db.SaveChangesAsync(CancellationToken.None);
                                        // Save the updated database template back to your database
                                        break; // Exit the inner loop once a match is found
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("List Template failed. Status Code: " + response.StatusCode);
                            string responseBody = await response.Content.ReadAsStringAsync();
                        }
                    }
                }

                #endregion

                #region customer template status check
                var customerGuestJourneyMessageTemplates = await _db.CustomerGuestJournies.Where(e => e.VonageTemplateStatus == "PENDING").ToListAsync();


                if (customerGuestJourneyMessageTemplates != null && customerGuestJourneyMessageTemplates.Count > 0)
                {
                    foreach (var template in customerGuestJourneyMessageTemplates)
                    {
                        var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == template.CutomerId).FirstOrDefaultAsync(CancellationToken.None);

                        if (vonageCred != null)
                        {
                            var token = await _vonageService.GenerateJWT(vonageCred.AppId, vonageCred.AppPrivatKey);

                            // Vonage Template Management API URL
                            string apiUrl = $"https://api.nexmo.com/v2/whatsapp-manager/wabas/{vonageCred.WABAId}/templates";

                            using (var httpClient = new HttpClient())
                            {
                                // Set JWT token in request headers for authorization
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                                // Set content type
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                // Send GET request to create the template
                                var response = await httpClient.GetAsync(apiUrl);

                                //Check if the request was successful
                                if (response.IsSuccessStatusCode)
                                {
                                    string responseBody = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine("List Template successfully. Response: " + responseBody);
                                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                                    var templates = jsonResponse.templates;

                                    foreach (var t in templates)
                                    {
                                        if (t["id"].ToString() == template.VonageTemplateId)
                                        {
                                            string status = t["status"].ToString();
                                            template.VonageTemplateStatus = status;
                                            await _db.SaveChangesAsync(CancellationToken.None);
                                            // Save the updated database template back to your database
                                            break; // Exit the inner loop once a match is found
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("List Template failed. Status Code: " + response.StatusCode);
                                    string responseBody = await response.Content.ReadAsStringAsync();
                                }
                            }
                        }

                    }

                }

                #endregion

            }

            // Wait for one day before executing the task again
            await Task.Delay(TimeSpan.Parse(_time.TemplateStatusCheckServiceTiming), stoppingToken);
        }
    }
}
