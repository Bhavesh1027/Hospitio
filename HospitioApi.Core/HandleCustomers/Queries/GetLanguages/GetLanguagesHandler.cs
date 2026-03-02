using MediatR;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Language;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguages;

public record GetLanguagesRequest() : IRequest<AppHandlerResponse>;
public class GetLanguagesHandler : IRequestHandler<GetLanguagesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ILanguageService _languageService;

    public GetLanguagesHandler(IDapperRepository dapper, IHandlerResponseFactory response, ILanguageService languageService)
    {
        _dapper = dapper;
        _response = response;
        _languageService = languageService;
    }

    public async Task<AppHandlerResponse> Handle(GetLanguagesRequest request, CancellationToken cancellationToken)
    {
        var languages = await _languageService.GetSupportedLanguageAsync(cancellationToken);

        if (string.IsNullOrEmpty(languages))
        {
            return _response.Error($"Languages not found.", AppStatusCodeError.Gone410);
        }

        dynamic data = JObject.Parse(languages);
        dynamic translation = JObject.Parse(Convert.ToString(data["translation"]));

        List<LanguagesOut> languagesOut = new List<LanguagesOut>();

        foreach (var trans in translation)
        {

            var objValue = ((Newtonsoft.Json.Linq.JProperty)trans).Value;

            if (objValue.HasValues)
            {
                dynamic dynamicObjValue = JObject.Parse(Convert.ToString(objValue));
                var languageCode = trans != null ? ((Newtonsoft.Json.Linq.JProperty)trans).Name : string.Empty;

                if (dynamicObjValue != null)
                {
                    var name = Convert.ToString(dynamicObjValue.name);
                    var nativeName = Convert.ToString(dynamicObjValue.nativeName);
                    var dir = Convert.ToString(dynamicObjValue.dir);

                    LanguagesOut language = new LanguagesOut()
                    {
                        LanguageCode = languageCode,
                        Name = name,
                        NativeName = nativeName,
                        Dir = dir
                    };

                    languagesOut.Add(language);
                }
            }
        }

        return _response.Success(new GetLanguagesOut("Get languages successful.", languagesOut));
    }
}