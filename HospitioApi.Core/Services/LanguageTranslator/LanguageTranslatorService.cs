using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.LanguageTranslator.Models;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.Services.LanguageTranslator;

public class LanguageTranslatorService : ILanguageTranslatorService
{
    private readonly AzurLanguageTranslatorSettingsOptions _languageTranslator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LanguageTranslatorService( IOptions<AzurLanguageTranslatorSettingsOptions> languageTranslator, IHttpContextAccessor httpContextAccessor)
    {
        _languageTranslator = languageTranslator.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LanguageTranslate> GetLanguageTranslatedAsync(ApplicationDbContext _db,int userType, int customerId, int channelMessageId, string message)
    {
        string translationLanguage = "en";

        if (userType == (int)UserTypeEnum.Hospitio)
        {
            var onboarding = await _db.HospitioOnboardings.SingleOrDefaultAsync(x => x.IncomingTranslationLangage != null);
            if (onboarding != null)
            {
                translationLanguage = onboarding.IncomingTranslationLangage!.ToLower();
            }
        }
        else if (userType == (int)UserTypeEnum.Customer)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == customerId && x.IncomingTranslationLangage != null && x.IsActive == true);

            if (customer != null)
            {
                translationLanguage = customer.IncomingTranslationLangage!.ToLower();
            }
        }

        var channelMessage = await _db.ChannelMessages.FirstOrDefaultAsync(x => x.Id == channelMessageId);

        string key = _languageTranslator.AzurKey;
        string endpoint = _languageTranslator.EndPoint;
        string location = _languageTranslator.Location;

        string route = $"translate?api-version=3.0&to={translationLanguage}";
        string textToTranslate = message;
        object[] body = new object[] { new { Text = textToTranslate } };
        var requestBody = JsonConvert.SerializeObject(body);

        string jsonContent = string.Empty;

        using (var client = new HttpClient())
        using (var requestt = new HttpRequestMessage())
        {
            // Build the request.
            requestt.Method = HttpMethod.Post;
            requestt.RequestUri = new Uri(endpoint + route);
            requestt.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // location required if you're using a multi-service or regional (not global) resource. 
            requestt.Headers.Add("Ocp-Apim-Subscription-Key", key);
            requestt.Headers.Add("Ocp-Apim-Subscription-Region", location);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(requestt).ConfigureAwait(false);

            // Read response as a string.
            jsonContent = await response.Content.ReadAsStringAsync();
        }

        var languageDetail = JsonConvert.DeserializeObject<List<LanguageDetail>>(jsonContent);

        LanguageTranslate language = new LanguageTranslate();

        if (languageDetail != null)
        {
            //language.detectedLanguageCode = languageDetail[0].detectedLanguage.language;
            language.message = languageDetail[0].translations[0].text;
            //language.convertedLanguageCode = languageDetail[0].translations[0].to;
            language.channelMessageId = channelMessageId;
        }

        return language;
    }

    public async Task<LanguageTranslate> GetGuestTranslatedLanguage(ApplicationDbContext _db, int userType, int guestId, int channelMessageId, string message, string translatelanguage)
    {

        string key = _languageTranslator.AzurKey;
        string endpoint = _languageTranslator.EndPoint;
        string location = _languageTranslator.Location;
        string translationLanguage = translatelanguage;

        if (userType == (int)UserTypeEnum.Guest)
        {
            var channelId = await _db.ChannelUserTypeCustomerGuest.Where(i => i.UserId == guestId).Select(i => i.ChannelId).FirstOrDefaultAsync();

            var Lastmessage = await _db.TextMessage
                            .Where(i => i.ChannelId == channelId && i.MessageSender == userType && i.MessageSenderId == guestId)
                            .OrderBy(i => i.Id)
                            .Select(i => i.Message).ToListAsync();

            if (Lastmessage.Count != 0)
            {
                object[] translatebody = new object[] { new { Text = Lastmessage[Lastmessage.Count-1] } };
                var translaterequestbody = JsonConvert.SerializeObject(translatebody);
                string jsonContents = string.Empty;
                string RouteDetect = "detect?api-version=3.0";
                using (var client = new HttpClient())
                using (var requestt = new HttpRequestMessage())
                {
                    // Build the request.
                    requestt.Method = HttpMethod.Post;
                    requestt.RequestUri = new Uri(endpoint + RouteDetect);
                    requestt.Content = new StringContent(translaterequestbody, Encoding.UTF8, "application/json");

                    // location required if you're using a multi-service or regional (not global) resource. 
                    requestt.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    requestt.Headers.Add("Ocp-Apim-Subscription-Region", location);

                    // Send the request and get response.
                    HttpResponseMessage response = await client.SendAsync(requestt).ConfigureAwait(false);

                    // Read response as a string.
                    jsonContents = await response.Content.ReadAsStringAsync();

                    List<detectedLanguage> TransleteLangaugeresults = JsonConvert.DeserializeObject<List<detectedLanguage>>(jsonContents)!;
                    translationLanguage = TransleteLangaugeresults[0].language.ToString();
                }
            }
        }
        var channelMessage = await _db.ChannelMessages.FirstOrDefaultAsync(x => x.Id == channelMessageId);

        string route = $"translate?api-version=3.0&to={translationLanguage}";
        string textToTranslate = message;
        object[] body = new object[] { new { Text = textToTranslate } };
        var requestBody = JsonConvert.SerializeObject(body);

        string jsonContent = string.Empty;

        using (var client = new HttpClient())
        using (var requestt = new HttpRequestMessage())
        {
            // Build the request.
            requestt.Method = HttpMethod.Post;
            requestt.RequestUri = new Uri(endpoint + route);
            requestt.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // location required if you're using a multi-service or regional (not global) resource. 
            requestt.Headers.Add("Ocp-Apim-Subscription-Key", key);
            requestt.Headers.Add("Ocp-Apim-Subscription-Region", location);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(requestt).ConfigureAwait(false);

            // Read response as a string.
            if(response.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                jsonContent = await response.Content.ReadAsStringAsync();
            }
        }

        var languageDetail = JsonConvert.DeserializeObject<List<LanguageDetail>>(jsonContent);

        LanguageTranslate language = new LanguageTranslate();

        if (languageDetail != null)
        {
            //language.detectedLanguageCode = languageDetail[0].detectedLanguage.language;
            language.message = languageDetail[0].translations[0].text;
            //language.convertedLanguageCode = languageDetail[0].translations[0].to;
            language.channelMessageId = channelMessageId;
        }

        return language;
    }
}
