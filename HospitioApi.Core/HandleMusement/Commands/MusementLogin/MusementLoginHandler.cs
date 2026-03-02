using MediatR;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;
using System.Text.Json;

namespace HospitioApi.Core.HandleMusement.Commands.MusementLogin;
public record MusementLoginRequest()
    : IRequest<AppHandlerResponse>;
public class MusementLoginHandler : IRequestHandler<MusementLoginRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MusementSettingsOptions _musementSettings;
    public MusementLoginHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response,
        IHttpClientFactory httpClientFactory,
         IOptions<MusementSettingsOptions> musementSettings
        )
    {
        _dapper = dapper;
        _response = response;
        _httpClientFactory = httpClientFactory;
        _musementSettings = musementSettings.Value;
    }

    public async Task<AppHandlerResponse> Handle(MusementLoginRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();

        var loginEndpoint = _musementSettings.musement_url + "login";
        var applicationValue = "string"; // Replace with your application value

        var requestContent = new
        {
            client_id = _musementSettings.client_id,
            client_secret = _musementSettings.client_secret,
            grant_type = _musementSettings.grant_type
        };

        var requestContentJson = JsonSerializer.Serialize(requestContent);

        var musementRequest = new HttpRequestMessage(HttpMethod.Post, loginEndpoint);
        musementRequest.Headers.Add("X-Musement-Application", applicationValue);
        musementRequest.Headers.Add("X-Musement-Version", "3.4.0");
        musementRequest.Content = new StringContent(requestContentJson, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(musementRequest);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<MusementLoginResponseOut>(responseContent);


            return _response.Success(new MusementLoginOut("Login successful.", tokenResponse));
        }
        else
        {
            return _response.Error("Login failed", AppStatusCodeError.Gone410);
        }

       
    }
}
