using CSharpToTypeScript.Core.Constants;

namespace CSharpToTypeScript.Core.Services.TypeConversionHandlers;

internal class StringConverter : BasicTypeConverterBase<Models.TypeNodes.String>
{
    protected override IEnumerable<string> ConvertibleFromPredefined => new[]
    {
        PredefinedTypes.String, PredefinedTypes.Char
    };

    protected override IEnumerable<string> ConvertibleFromIdentified => new[]
    {
        nameof(System.String), nameof(System.Char), nameof(System.TimeSpan), nameof(System.Guid), nameof(System.Uri)
    };
}