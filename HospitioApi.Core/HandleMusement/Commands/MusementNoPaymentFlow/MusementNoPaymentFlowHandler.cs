using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.HandleMusement.Commands.MusementNoPaymentFlow;
public record MusementNoPaymentFlowRequest(MusementNoPaymentFlowIn In) : IRequest<AppHandlerResponse>;
public class MusementNoPaymentFlowHandler : IRequestHandler<MusementNoPaymentFlowRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MusementSettingsOptions _musementSettings;
    public MusementNoPaymentFlowHandler(
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
    public async Task<AppHandlerResponse> Handle(MusementNoPaymentFlowRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Musement-Application", "string");
        client.DefaultRequestHeaders.Add("X-Musement-Currency", "USD");
        client.DefaultRequestHeaders.Add("X-Musement-Version", "3.4.0");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.In.token}");

        JObject json = JObject.Parse(@"{
          uuid : '' 
        }");

        json["uuid"] = request.In.uuid;

        var postData = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        var mesumentRequest = await client.PostAsync(_musementSettings.musement_url + request.In.Url, postData);

        if (mesumentRequest.IsSuccessStatusCode)
        {
            var response = await mesumentRequest.Content.ReadAsStringAsync();
            return _response.Success(new MusementNoPaymentFlowOut("Get Data successful.", response));
        }
        else
        {
            return _response.Error("Data not found", AppStatusCodeError.Gone410);
        }

    }
}
