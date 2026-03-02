

using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.Services.HandlerResponse;

public class HandlerResponseFactory : IHandlerResponseFactory
{
    public AppHandlerResponse Success(BaseResponseOut response) =>
        new(response);

    public AppHandlerResponse Error(AppException ex) =>
        new(ex, rollbackOnFailure: true);
    public AppHandlerResponse Error(AppValidationException ex) =>
        new(ex, rollbackOnFailure: true);
    public AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, skipEmailNotification, hideErrors), rollbackOnFailure: true);
    public AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, errors, skipEmailNotification, hideErrors), rollbackOnFailure: true);
    public AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, innerException, skipEmailNotification, hideErrors), rollbackOnFailure: true);
    public AppHandlerResponse Error(string message, AppStatusCodeError statusCodeError, Exception innerException, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, errors, innerException, skipEmailNotification, hideErrors), rollbackOnFailure: true);

    public AppHandlerResponse ErrorAndCommitPartialChanges(AppException ex) =>
        new(ex, rollbackOnFailure: false);
    public AppHandlerResponse ErrorAndCommitPartialChanges(AppValidationException ex) =>
        new(ex, rollbackOnFailure: false);
    public AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, skipEmailNotification, hideErrors), rollbackOnFailure: false);
    public AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, errors, skipEmailNotification, hideErrors), rollbackOnFailure: false);
    public AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, Exception innerException, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, innerException, skipEmailNotification, hideErrors), rollbackOnFailure: false);
    public AppHandlerResponse ErrorAndCommitPartialChanges(string message, AppStatusCodeError statusCodeError, Exception innerException, List<string> errors, bool skipEmailNotification = false, bool hideErrors = false) =>
        new(new AppException(message, statusCodeError, errors, innerException, skipEmailNotification, hideErrors), rollbackOnFailure: false);
}