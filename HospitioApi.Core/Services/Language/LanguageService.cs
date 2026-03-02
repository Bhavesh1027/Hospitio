using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HospitioApi.Core.Services.Language;

public class LanguageService : ILanguageService
{
    private readonly ThirdPartyAPIUrlOptions _thirdPartyAPIUrl;

    public LanguageService(IOptions<ThirdPartyAPIUrlOptions> thirdPartyAPIUrl)
    {
        _thirdPartyAPIUrl = thirdPartyAPIUrl.Value;
    }

    public async Task<string> GetSupportedLanguageAsync(CancellationToken cancellationToken)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_thirdPartyAPIUrl.Languages);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await MakeRequestAsync(request, client, cancellationToken);
            var prettyJson = PrettyJson(response);

            return prettyJson;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception();
        }
    }
    public async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client, CancellationToken cancellationToken)
    {
        var response = await client.SendAsync(getRequest).ConfigureAwait(false);
        var responseString = string.Empty;
        try
        {
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            throw new Exception();
        }
        return responseString;
    }
    public string PrettyJson(string unPrettyJson)
    {
        if (string.IsNullOrEmpty(unPrettyJson)) return string.Empty;

        var options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

        var json = System.Text.Json.JsonSerializer.Serialize(jsonElement, options);

        return json;
    }
}
