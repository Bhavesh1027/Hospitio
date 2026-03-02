using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.LanguageTranslator;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;

public record GetLanguageTranslationRequest(bool isAdmin, int channelMessageId, string message, string? customerId) : IRequest<AppHandlerResponse>;

public class GetLanguageTranslationHandler : IRequestHandler<GetLanguageTranslationRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ILanguageTranslatorService _languageTranslatorService;

    public GetLanguageTranslationHandler(IHandlerResponseFactory response, ILanguageTranslatorService languageTranslatorService)
    {
        _response = response;
        _languageTranslatorService = languageTranslatorService;
    }

    public async Task<AppHandlerResponse> Handle(GetLanguageTranslationRequest request, CancellationToken cancellationToken)
    {
        LanguageTranslationOut languageOut = new LanguageTranslationOut();

        //var language = await _languageTranslatorService.GetLanguageTranslatedAsync(request.isAdmin, request.customerId != null ? Int32.Parse(request.customerId) : 0, request.channelMessageId, request.message);

        //if (language != null)
        //{
        //    //languageOut.detectedLanguageCode = language.detectedLanguageCode;
        //    languageOut.convertedMessage = language.message;
        //    //languageOut.convertedLanguageCode = language.convertedLanguageCode;
        //}

        return _response.Success(new GetLanguageTranslationOut("Get converted language successful.", languageOut));
    }
}
