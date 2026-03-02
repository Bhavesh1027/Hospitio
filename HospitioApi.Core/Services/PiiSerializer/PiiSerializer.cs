using Microsoft.Extensions.Configuration;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HospitioApi.Core.Services.PiiSerializer;


public class PiiSerializer : IPiiSerializer
{
    private readonly IConfiguration _configuration;

    public PiiSerializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public PiiSerializerHelper<T> Init<T>(T originalObject, bool useUnsafeRelaxedJsonEscaping)
    {
        var helper = new PiiSerializerHelper<T>(
            originalObject,
            useUnsafeRelaxedJsonEscaping,
            _configuration.GetValue<bool>("LoggingShowPII"));

        return helper;
    }
}

public class PiiSerializerHelper<T>
{
    public PiiSerializerHelper(
        T originalObject,
        bool useUnsafeRelaxedJsonEscaping,
        bool loggingShowPII)
    {
        UseUnsafeRelaxedJsonEscaping = useUnsafeRelaxedJsonEscaping;
        LoggingShowPII = loggingShowPII;
        DeepClonedObject = DeepClone(originalObject);
    }

    public bool UseUnsafeRelaxedJsonEscaping { get; }
    public bool LoggingShowPII { get; }
    public T DeepClonedObject { get; set; }

    private T DeepClone(T originalObject)
    {
        var deepCloneAsJson = UseUnsafeRelaxedJsonEscaping
            ? JsonSerializer.Serialize(originalObject, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })
            : JsonSerializer.Serialize(originalObject);

        var deepClone = UseUnsafeRelaxedJsonEscaping
            ? JsonSerializer.Deserialize<T>(deepCloneAsJson, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })
            : JsonSerializer.Deserialize<T>(deepCloneAsJson);

        if (deepClone is null)
        {
            throw new AppException($"Could not create a clone of {nameof(T)}.", AppStatusCodeError.InternalServerError500);
        }
        return deepClone;
    }
}

public static class PiiSerializerHelperExtensions
{
    public static PiiSerializerHelper<T> For<T>(this PiiSerializerHelper<T> helper, Expression<Func<T, object>> pathExpression)
    {
        var originalFullPath = pathExpression.ToString();
        var relativePropPath = originalFullPath[(originalFullPath.IndexOf(".", StringComparison.Ordinal) + 1)..];

        if (helper.DeepClonedObject is not null && !helper.LoggingShowPII)
        {
            SetProperty(helper.DeepClonedObject, relativePropPath, "********");
        }
        return helper;
    }

    public static string Serialize<T>(this PiiSerializerHelper<T> helper)
    {
        return helper.UseUnsafeRelaxedJsonEscaping
            ? JsonSerializer.Serialize(helper.DeepClonedObject, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })
            : JsonSerializer.Serialize(helper.DeepClonedObject);
    }

    private static void SetProperty(object source, string property, object target)
    {
        if (source is null)
        {
            return;
        }
        var bits = property.Split('.');
        for (var i = 0; i < bits.Length - 1; i++)
        {
            var prop = source.GetType().GetProperty(bits[i]);
            if (prop is not null)
            {
                var val = prop.GetValue(source, null);
                if (val is not null)
                {
                    source = val;
                }
            }
        }
        var propertyToSet = source.GetType()
                                           .GetProperty(bits[^1]);
        if (propertyToSet is not null)
        {
            propertyToSet.SetValue(source, target, null);
        }
    }
}
