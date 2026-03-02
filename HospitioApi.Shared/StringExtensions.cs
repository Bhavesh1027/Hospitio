

using Microsoft.AspNetCore.Http;

namespace HospitioApi.Shared;
public static class StringExtensions
{
    /// <summary>
    /// Resolves the string as a relative url and applies an http: or https: protocol to the url if it needs it
    /// </summary>
    /// <param name="makeSsl">Whether to force the https: protocol to be used</param>
    /// <returns>A new string with the http(s): protocol, server, and port prefixed if needed to the resolved path</returns>
    public static string ToAbsoluteUrl(this string url, HttpContext httpContext, bool makeSsl = false)
    {
        if (url == null)
        {
            return "";
        }
        if ((!makeSsl && url.StartsWith("http:")) || url.StartsWith("https:"))
        {
            return url;
        }
        string scheme = makeSsl ? "https://" : httpContext.Request.Scheme + "://";
        string server = httpContext.Request.Host.Host;
        int port = httpContext.Request.Host.Port.HasValue ? httpContext.Request.Host.Port.Value : 1;
        if (port > 0 && port != 80 && port != 443)
        {
            server = server + ":" + port;
        }
        return scheme + server + url;
    }

    public static string GetNumbers(this string @this)
    {
        return new string(@this.Where(c => char.IsDigit(c)).ToArray());
    }

    public static string To10DigitPhoneNumber(this string @this)
    {
        @this = @this.GetNumbers();

        if (@this.Length == 10)
        {
            return @this; /** 10 digit format */
        }
        else if (@this.Length == 11 && @this[0] == 1)
        {
            return @this[1..]; /** Remove USA country code if it was added by user */
        }
        else
        {
            return @this; /** Unknown format, return number as is */
        }
    }
}
