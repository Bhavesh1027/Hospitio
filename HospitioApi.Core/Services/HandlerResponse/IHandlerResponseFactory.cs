using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.Services.HandlerResponse;
public interface IHandlerResponseFactory
{
    AppHandlerResponse Success(BaseResponseOut response);

    AppHandlerResponse Error(AppException ex);
    AppHandlerResponse Error(AppValidationException ex);
    AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, Exception innerException, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false);

    AppHandlerResponse ErrorAndCommitPartialChanges(AppException ex);
    AppHandlerResponse ErrorAndCommitPartialChanges(AppValidationException ex);
    AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false);
    AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, Exception innerException, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false);
}