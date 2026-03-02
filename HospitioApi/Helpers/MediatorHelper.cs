
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Helpers;

public interface IMediatorHelper
{
    Task<IResult> ToResultAsync(IRequest<AppHandlerResponse> request, CancellationToken cancellationToken);
}

public class MediatorHelper : IMediatorHelper
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MediatorHelper(
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult> ToResultAsync(IRequest<AppHandlerResponse> request, CancellationToken cancellationToken)
    {
        var handlerResponse = await _mediator.Send(request, cancellationToken);
        return handlerResponse.HasResponse
            ? Results.Ok(handlerResponse.Response)
            : ExceptionToResult(handlerResponse.Failure ?? new Exception("Unexpected Error"));
    }

    private IResult ExceptionToResult(Exception ex)
    {
        return ex switch
        {
            AppValidationException appValEx => HandleAppValException(appValEx),
            AppException appEx => HandleAppException(appEx),
            _ => HandleAnyException(ex)
        };

        IResult HandleAppValException(AppValidationException appValEx) => Results.ValidationProblem(appValEx.ValidationFailures
            .Select(e => (PropertyName: e.PropertyName.Replace("In.", null), e.ErrorMessage))
            .GroupBy(t => t.PropertyName)
            .ToDictionary(grp => grp.Key, grp => grp.Select(x => x.ErrorMessage).ToArray()));

        IResult HandleAppException(AppException ex)
        {
            Func<BaseResponseOut, IResult> result = ex.StatusCodeError switch
            {
                AppStatusCodeError.UnprocessableEntity422 => Results.UnprocessableEntity,
                AppStatusCodeError.PaymentRequired402 => PaymentRequired,
                AppStatusCodeError.Forbidden403 => Forbidden,
                AppStatusCodeError.Gone410 => Gone,
                AppStatusCodeError.Conflict409 => Results.Conflict,
                AppStatusCodeError.InternalServerError500 => InternalServerError,
                _ => InternalServerError
            };
            return result(ex.ErrorResponse);

            IResult PaymentRequired(object error) => Results.Json(error, statusCode: StatusCodes.Status402PaymentRequired);
            IResult Forbidden(object error) => Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            IResult Gone(object error) => Results.Json(error, statusCode: StatusCodes.Status410Gone);
            IResult InternalServerError(object error) => Results.Json(error, statusCode: StatusCodes.Status500InternalServerError);
        }

        IResult HandleAnyException(Exception ex)
        {
            var statusCode = (int)GetStatusCodeForException(ex);
            var message = string.IsNullOrWhiteSpace(ex.Message) ? ReasonPhrases.GetReasonPhrase(statusCode) : ex.Message;
            var detail = message.Replace("See the inner exception for details.", "").Trim();
            return Results.Problem(detail, _httpContextAccessor.HttpContext?.Request.Path, statusCode);

            AppStatusCodeError GetStatusCodeForException(Exception exception) => exception switch
            {
                FormatException _ => AppStatusCodeError.UnprocessableEntity422,
                ArgumentException _ => AppStatusCodeError.UnprocessableEntity422,
                InvalidOperationException _ => AppStatusCodeError.Conflict409,
                _ => AppStatusCodeError.InternalServerError500
            };
        }
    }
}
