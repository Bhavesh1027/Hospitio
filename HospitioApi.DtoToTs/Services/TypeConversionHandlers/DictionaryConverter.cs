using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.ObjectModel;

namespace CSharpToTypeScript.Core.Services.TypeConversionHandlers;

internal class DictionaryConverter : TypeConversionHandler
{
    private IEnumerable<string> ConvertibleFrom => new[]
    {
        nameof(Dictionary<object, object>), nameof(IDictionary<object, object>),
        nameof(ReadOnlyDictionary<object, object>), nameof(IReadOnlyDictionary<object, object>)
    };

    private readonly TypeConversionHandler _converter;

    public DictionaryConverter(TypeConversionHandler converter)
    {
        _converter = converter;
    }

    public override Models.TypeNodes.TypeNode Handle(TypeSyntax type)
    {
        if (type is GenericNameSyntax generic && ConvertibleFrom.Contains(generic.Identifier.Text)
            && generic.TypeArgumentList.Arguments.Count == 2)
        {
            var key = _converter.Handle(generic.TypeArgumentList.Arguments[0]);

            if (key is Models.TypeNodes.String || key is Models.TypeNodes.Number)
            {
                return new Models.TypeNodes.Dictionary(
                    key: key,
                    value: _converter.Handle(generic.TypeArgumentList.Arguments[1]));
            }
        }

        return base.Handle(type);
    }
}