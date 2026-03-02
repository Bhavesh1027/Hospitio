using CSharpToTypeScript.Core.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpToTypeScript.Core.Services;

internal class RootEnumConverter
{
    public RootEnumNode Convert(EnumDeclarationSyntax @enum)
        => new(
            name: @enum.Identifier.ValueText,
            members: ConvertEnumMembers(@enum.Members));

    private static IEnumerable<EnumMemberNode> ConvertEnumMembers(IEnumerable<EnumMemberDeclarationSyntax> members)
      => members.Select(m => new EnumMemberNode(
            name: m.Identifier.ValueText,
            value: m.EqualsValue?.Value.ToString()));
}