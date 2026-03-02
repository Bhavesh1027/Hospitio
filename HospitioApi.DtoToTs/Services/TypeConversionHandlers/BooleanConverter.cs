using CSharpToTypeScript.Core.Constants;

namespace CSharpToTypeScript.Core.Services.TypeConversionHandlers;

internal class BooleanConverter : BasicTypeConverterBase<Models.TypeNodes.Boolean>
{
    protected override IEnumerable<string> ConvertibleFromPredefined => new[]
    {
        PredefinedTypes.Bool
    };

    protected override IEnumerable<string> ConvertibleFromIdentified => new[]
    {
        nameof(System.Boolean)
    };
}