using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.PiiSerializer;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text.Json;

namespace HospitioApi.Pipeline;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AppHandlerResponse
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IHandlerResponseFactory _response;
    private readonly IPiiSerializer _piiSerializer;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingPipelineBehavior(
        ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger,
        IHandlerResponseFactory response,
        IPiiSerializer piiSerializer,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _response = response;
        _piiSerializer = piiSerializer;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        var requestName = typeof(TRequest).Name;
        //var userId = _httpContextAccessor.HttpContext?.User.Id();

        var requestId = _httpContextAccessor.HttpContext!.Request.Headers["X-Request-Id"];
        //if (string.IsNullOrWhiteSpace(requestId))
        //{
        //    userId = "Not authenticated user";
        //}
        //var delegateAccessUserId = _httpContextAccessor.HttpContext?.User.DelegateAccessUserId();

        try
        {
            _logger.LogInformation("[Start] Handling {requestName}", requestName);
            _logger.LogInformation("[Start] Handling {requestId}", requestId!);
            //if (!string.IsNullOrWhiteSpace(delegateAccessUserId))
            //{
            //    _logger.LogInformation("Request by delegateAccessUserId: {userId}", delegateAccessUserId);
            //}

            try
            {
                //if (request is LoginRequest loginRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(loginRequest, true)
                //        .For(r => r.In.Password)
                //        .Serialize());
                //}
                //else if (request is CheckDelegateAccessRequest checkDelegateAccessRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(checkDelegateAccessRequest, true)
                //        .For(r => r.In.Password)
                //        .Serialize());
                //}
                //else if (request is ChangePasswordHandlerRequest changePasswordRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(changePasswordRequest, true)
                //        .For(r => r.In.CurrentPassword)
                //        .For(r => r.In.NewPassword)
                //        .Serialize());
                //}
                //else if (request is ChangePasswordByClientHandlerRequest changePasswordByClientHandlerRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(changePasswordByClientHandlerRequest, true)
                //        .For(r => r.In.NewPassword)
                //        .Serialize());
                //}
                //else if (request is RegisterUserHandlerRequest registerUserHandlerRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(registerUserHandlerRequest, true)
                //        .For(r => r.In.Password)
                //        .Serialize());
                //}
                //else if (request is ResetPasswordConfirmationHandlerRequest resetPasswordConfirmationRequest)
                //{
                //    LogInformationRequestPayload(_piiSerializer
                //        .Init(resetPasswordConfirmationRequest, true)
                //        .For(r => r.In.Password)
                //        .Serialize());
                //}
                //else
                //{
                LogInformationRequestPayload(JsonSerializer.Serialize(request));
                //}
            }
            catch (Exception e)
            {
                //SendErrorEmail(requestName, e.Message, new[] { "Could not serialize request payload." }, e);
                _logger.LogError(e, "[Error] Could not serialize request payload.");
            }

            var response = await next();

            if (response is AppHandlerResponse r)
            {
                if (r.HasFailure && r.Failure is not null)
                {
                    if (r.Failure is AppValidationException appValidationException)
                    {
                        var errors = appValidationException.ErrorResponse.Errors.Concat(appValidationException.ValidationFailures.Select(vf => $"{vf.PropertyName}: {vf.ErrorMessage}"));
                        //SendErrorEmail(requestName, appValidationException.Message, errors, appValidationException, skipEmail: appValidationException.SkipEmailNotification);
                        _logger.LogError(appValidationException, "[Error] Returned 'Failure' (Exception) value from Handler {requestName}. Errors: {errors}", requestName, errors);
                    }
                    else
                    {
                        // SendErrorEmail(requestName, r.Failure.Message, r.Failure.ErrorResponse.Errors, r.Failure, skipEmail: r.Failure.SkipEmailNotification);
                        _logger.LogError(r.Failure, "[Error] Returned 'Failure' (Exception) value from Handler {requestName}. Errors: {errors}", requestName, r.Failure.ErrorResponse.Errors);

                        if (r.Failure.HideErrors)
                        {
                            r.SetFailure(
                                new(r.Failure.Message, r.Failure.StatusCodeError, new List<string>(), r.Failure.SkipEmailNotification, r.Failure.HideErrors),
                                r.HasFailureAndRollbackOnFailure);
                            response = (TResponse)r;
                        }
                    }
                }
            }

            _logger.LogInformation("[End] Handled {requestName}", requestName);
            return response;
        }
        //catch (Azure.RequestFailedException e)
        //{
        //   // SendErrorEmail(requestName, e.Message, Array.Empty<string>(), e);
        //    _logger.LogError(e, "[Error] During Handling {requestName}", requestName);
        //    if (Enum.IsDefined(typeof(AppStatusCodeError), e.Status))
        //    {
        //        return (TResponse)_response.Error(e.ErrorCode ?? "Unknown Error", (AppStatusCodeError)e.Status, e);
        //    }
        //    return (TResponse)_response.Error(e.ErrorCode ?? "Unknown Error", AppStatusCodeError.InternalServerError500, e);
        //}
        catch (AppException e)
        {
            // SendErrorEmail(requestName, e.Message, e.ErrorResponse.Errors, e, skipEmail: e.SkipEmailNotification);
            _logger.LogError(e, "[Error] During Handling {requestName}. Errors: {errors}", requestName, e.ErrorResponse.Errors);
            if (e.HideErrors)
            {
                return (TResponse)_response.Error(e.Message, e.StatusCodeError);
            }
            return (TResponse)_response.Error(e.Message, e.StatusCodeError, e.ErrorResponse.Errors);
        }
        catch (Exception e)
        {
            // SendErrorEmail(requestName, e.Message, Array.Empty<string>(), e);
            _logger.LogError(e, "[Error] During Handling {requestName}", requestName);
            return (TResponse)_response.Error("Unexpected Error", AppStatusCodeError.InternalServerError500, e);
        }
    }

    private void LogInformationRequestPayload(string message) => _logger.LogInformation("Request payload => {@requestPayload}", message);

    //private void SendErrorEmail(string requestName, string message, IEnumerable<string> additionalErrors, Exception exception, bool skipEmail = false)
    //{
    //    var emailErrorBackgroundSettingsEnabled = _emailErrorBackgroundSettings.Enabled;
    //    var emailErrorBackgroundSettingsToEmails = _emailErrorBackgroundSettings.ToEmails;

    //    if (!skipEmail && emailErrorBackgroundSettingsEnabled && !string.IsNullOrWhiteSpace(emailErrorBackgroundSettingsToEmails))
    //    {
    //        var htmlMessage = $@"
    //        <div style=""font-family:monospace;"">
    //            <div>[Error] Returned 'Failure' value from Handler or Exception thrown</div>
    //            <div><strong>{requestName}</strong></div>
    //            <div>{message}</div>
    //        ";
    //        if (!additionalErrors.Any())
    //        {
    //            htmlMessage += $"<div>***** No Additional Errors *****<div>";
    //        }
    //        else
    //        {
    //            htmlMessage += "<div>***** Additional Errors *****<div>";
    //            foreach (var additionalError in additionalErrors)
    //            {
    //                htmlMessage += $"<div>* {additionalError}<div>";
    //            }
    //            htmlMessage += "<div>*****************************<div>";
    //        }
    //        htmlMessage += $@"
    //            <div>===================== Exception =====================</div>
    //            <div>{exception}</div>
    //            <div>=====================================================</div>
    //        </div>
    //        ";
    //        _emailBackgroundQueue.Enqueue(new(htmlMessage));
    //    }
    //}
}
