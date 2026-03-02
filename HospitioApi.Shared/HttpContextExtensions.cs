using Microsoft.AspNetCore.Http;

namespace HospitioApi.Shared;
public static class HttpContextExtensions
{
    public static string? IpAddress(this HttpContext? httpContext)
    {
        if (httpContext is null) { return null; }

        return httpContext.Request.Headers.ContainsKey("X-Forwarded-For")
            ? httpContext.Request.Headers["X-Forwarded-For"]
            : httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
