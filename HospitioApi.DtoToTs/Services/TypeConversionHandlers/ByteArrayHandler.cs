using CSharpToTypeScript.Core.Constants;
using CSharpToTypeScript.Core.Models.TypeNodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpToTypeScript.Core.Services.TypeConversionHandlers;

internal class ByteArrayHandler : TypeConversionHandler
{
    public override TypeNode Handle(TypeSyntax type)
    {
        if (type is ArrayTypeSyntax array && array.RankSpecifiers.Last().Rank == 1
            && ((array.ElementType is PredefinedTypeSyntax predefinedOf && predefinedOf.Keyword.Text == PredefinedTypes.Byte)
                || (array.ElementType is IdentifierNameSyntax identifiedOf && identifiedOf.Identifier.Text == nameof(System.Byte))))
        {
            if (array.RankSpecifiers.Count > 1)
            {
                return new Models.TypeNodes.Array(
                    of: new Models.TypeNodes.String(),
                    rank: array.RankSpecifiers
                        .Take(array.RankSpecifiers.Count - 1)
                        .Aggregate(0, (total, specifier) => total + specifier.Rank));
            }

            return new Models.TypeNodes.String();
        }

        return base.Handle(type);
    }
}