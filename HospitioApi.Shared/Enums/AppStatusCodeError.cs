using System.Net;

namespace HospitioApi.Shared.Enums
{
    public enum AppStatusCodeError
    {
        /**
         * Do not add Unauthorized401 to this enum.
         * 401 should only be returned automatically by the framework when the JWT token is not valid,
         * otherwise returning a custom 401 might cause issues with the RefreshTokenInterceptor in the front-end.
         * Instead use Forbidden403 if needed.
         */
        Forbidden403 = HttpStatusCode.Forbidden,
        /**
         * Do not add NotFound404 to this enum.
         * 404 should only be returned automatically by the framework when the client calls an endpoint that doesn't exist,
         * otherwise returning a custom 404 makes it more difficult to isolate expected errors vs unexpected errors
         * when looking at the logs in Azure Application Insights.
         * Instead use Gone410 if needed.
         */
        Gone410 = HttpStatusCode.Gone,
        Conflict409 = HttpStatusCode.Conflict,
        /**
         * Do not add BadRequest400 to this enum.
         * 400 should only be returned automatically by the framework when the client calls an endpoint
         * but cannot map the payload correctly or other similar issues.
         * This will make it easier when looking at the logs in Azure Application Insights and filter 400 vs custom 422 errors.
         * Instead use UnprocessableEntity422 if needed.
         */
        UnprocessableEntity422 = HttpStatusCode.UnprocessableEntity,
        InternalServerError500 = HttpStatusCode.InternalServerError,
        PaymentRequired402 = HttpStatusCode.PaymentRequired,
    }

    public static class AppStatusCodeErrorExtensions
    {
        public static AppStatusCodeError SafeCast(this AppStatusCodeError @this)
            => Enum.IsDefined(@this) ? @this : AppStatusCodeError.InternalServerError500;
    }
}
