using FluentValidation.Results;
using HospitioApi.Shared.Enums;


namespace HospitioApi.Shared;

public class AppException : Exception
{
    public AppException(string message, AppStatusCodeError statusCodeError, bool skipEmailNotification = false, bool hideErrors = false)
        : base(message)
    {
        ErrorResponse = new BaseResponseOut(message);
        StatusCodeError = statusCodeError;
        SkipEmailNotification = skipEmailNotification;
        HideErrors = hideErrors;
    }

    public AppException(string message, AppStatusCodeError statusCodeError, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false)
        : base(message)
    {
        ErrorResponse = new BaseResponseOut(message, errors);
        StatusCodeError = statusCodeError;
        SkipEmailNotification = skipEmailNotification;
        HideErrors = hideErrors;
    }

    public AppException(string message, AppStatusCodeError statusCodeError, List<string> errors, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false)
        : base(message, innerException)
    {
        ErrorResponse = new BaseResponseOut(message, errors);
        StatusCodeError = statusCodeError;
        SkipEmailNotification = skipEmailNotification;
        HideErrors = hideErrors;
    }

    public AppException(string message, AppStatusCodeError statusCodeError, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false)
        : base(message, innerException)
    {
        ErrorResponse = new BaseResponseOut(message);
        StatusCodeError = statusCodeError;
        SkipEmailNotification = skipEmailNotification;
        HideErrors = hideErrors;
    }
    public bool SkipEmailNotification { get; set; }
    public bool HideErrors { get; set; }
    public BaseResponseOut ErrorResponse { get; set; }
    public AppStatusCodeError StatusCodeError { get; set; }
}

public class AppValidationException : AppException
{
    const string message = "One or more validation errors occurred.";
    public AppValidationException(List<ValidationFailure> validationFailures)
        : base(message, AppStatusCodeError.UnprocessableEntity422)
    {
        ValidationFailures = validationFailures;
    }

    public AppValidationException(List<ValidationFailure> validationFailures, Exception innerException)
        : base(message, AppStatusCodeError.UnprocessableEntity422, innerException)
    {
        ValidationFailures = validationFailures;
    }

    public List<ValidationFailure> ValidationFailures { get; set; }
}
