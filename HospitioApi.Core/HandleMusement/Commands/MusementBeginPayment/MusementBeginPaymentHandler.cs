using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text;

namespace HospitioApi.Core.HandleMusement.Commands.MusementBeginPaymentStripe;
public record MusementBeginPaymentRequest(MusementBeginPaymentIn In) : IRequest<AppHandlerResponse>;
public class MusementBeginPaymentHandler : IRequestHandler<MusementBeginPaymentRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MusementSettingsOptions _musementSettings;
    public MusementBeginPaymentHandler(
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

    public async Task<AppHandlerResponse> Handle(MusementBeginPaymentRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Musement-Application", "string");
        client.DefaultRequestHeaders.Add("X-Musement-Version", "3.4.0");

        JObject json = JObject.Parse(@"{
          adyen_token : '' ,
          card_brand : '',
          card_country : '',
          client_ip : '',
          order_uuid : '' ,
          redirect_url_success_3d_secure : ''
        }");

        json["adyen_token"] = request.In.adyen_token;
        json["card_brand"] = request.In.card_brand;
        json["card_country"] = request.In.card_country;
        json["client_ip"] = request.In.client_ip;
        json["order_uuid"] = request.In.order_uuid;
        json["redirect_url_success_3d_secure"] = request.In.redirect_url_success_3d_secure;

        var postData = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        var mesumentRequest = await client.PostAsync(_musementSettings.musement_url + request.In.Url, postData);

        if (mesumentRequest.IsSuccessStatusCode)
        {
            var response = await mesumentRequest.Content.ReadAsStringAsync();
            //var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<musementBeginPaymentClass>(response);
            return _response.Success(new MusementBeginPaymentOut("Get Data successful.", response));
        }
        else
        {
            return _response.Error("Data not found", AppStatusCodeError.Gone410);
        }

    }
}
