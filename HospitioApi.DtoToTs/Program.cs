using CSharpToTypeScript.Core.Options;
using CSharpToTypeScript.Core.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

await new InitializeDtoToTs().Convert();

public class InitializeDtoToTs
{
    private readonly CodeConverter _codeConverter;
    private readonly FileNameConverter _fileNameConverter;
    public InitializeDtoToTs()
    {
        var typeConverterFactory = TypeConverterFactory.Create();
        var rootTypeConverter = new RootTypeConverter(typeConverterFactory);
        var rootEnumConverter = new RootEnumConverter();
        var syntaxTreeConverter = new SyntaxTreeConverter(rootTypeConverter, rootEnumConverter);
        _codeConverter = new CodeConverter(syntaxTreeConverter);
        _fileNameConverter = new FileNameConverter();
    }

    public async Task Convert()
    {
        var settings = new SettingsModel();

        var directoryCore = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new Exception();
        directoryCore = Path.GetFullPath(Path.Combine(directoryCore, @"..\..\..\..\HospitioApi.Core"));

        var directoryShared = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new Exception();
        directoryShared = Path.GetFullPath(Path.Combine(directoryShared, @"..\..\..\..\HospitioApi.Shared"));

        var output = @"..\..\..\..\HospitioApi";

        var filesIn = MapToInterfaces(directoryCore, "In.cs", settings, TsNamespaces.Add);
        var filesOut = MapToInterfaces(directoryCore, "Out.cs", settings, TsNamespaces.Add);
        var filesOthers = MapToInterfaces(directoryCore, "Ts.cs", settings, TsNamespaces.Skip);
        var filesEnums = MapToInterfaces(directoryShared, "Ts.cs", settings, TsNamespaces.Skip);

        var directoryHospitioApi = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new Exception();
        directoryHospitioApi = Path.GetFullPath(Path.Combine(directoryHospitioApi, output, "Contracts.ts"));

        if (File.Exists(directoryHospitioApi))
        {
            await WriteAllText.ClearAllTextinFile(directoryHospitioApi);
        }

        var orderedFiles = filesIn.Concat(filesOut).Concat(filesOthers).Concat(filesEnums)
            .OrderBy(t => t.tsNamespaces == TsNamespaces.Add)
            .ThenBy(t => t.domain)
            .ThenBy(t => t.handler)
            .Select(t => t.content);

        await WriteAllText.WriteAllTextinFile(directoryHospitioApi, orderedFiles);
        await File.WriteAllTextAsync(directoryHospitioApi, File.ReadAllText(directoryHospitioApi).Replace("MemoryStream", "any").Replace("FormFile", "any"));
    }

    private IEnumerable<(string content, string handler, string cqrs, string domain, TsNamespaces tsNamespaces)> MapToInterfaces(string directoryPath, string extension, SettingsModel settings, TsNamespaces tsNamespaces)
    {
        return FileSystem.GetFilesWithExtension(directoryPath, extension)
            .Select(filePath =>
            {
                var handlerDir = Path.GetDirectoryName(filePath) ?? "";
                var cqrsDir = Path.GetDirectoryName(handlerDir) ?? "";
                var domainDir = Path.GetDirectoryName(cqrsDir) ?? "";
                var handler = new DirectoryInfo(handlerDir).Name;
                var cqrs = new DirectoryInfo(cqrsDir).Name;
                var domain = new DirectoryInfo(domainDir).Name.Replace("Handle", "");
                var content = _codeConverter.ConvertToTypeScript(File.ReadAllText(filePath), filePath, settings.MapToCodeConversionOptions());
                content = content switch
                {
                    null => "",
                    not null when string.IsNullOrWhiteSpace(content) => "",
                    not null when tsNamespaces == TsNamespaces.Add => $"export namespace {domain}.{handler} {{\r\n{content}\r\n}}",
                    _ => content
                };
                return (content, handler, cqrs, domain, tsNamespaces);
            })
            .Where(t => !string.IsNullOrWhiteSpace(t.content));
    }
}

public enum TsNamespaces { Skip = 0, Add = 1 }

public static class FileSystem
{
    public static bool IsSameOrParrentDirectory(this string parrent, string child)
        => Path.GetFullPath(child).StartsWith(Path.GetFullPath(parrent));

    public static string ContainingDirectory(this string filePath)
        => new FileInfo(filePath)?.DirectoryName ?? "";

    public static IEnumerable<string> GetFilesWithExtension(string directoryPath, string extension)
        => Directory.GetFiles(directoryPath, $"*.{extension}", SearchOption.AllDirectories);

    public static bool EndsWithFileExtension(this string text)
        => Regex.IsMatch(text, @"\.\w+$");
}
class WriteAllText
{
    public static async Task WriteAllTextinFile(string path, IEnumerable<string> text)
    {
        await File.AppendAllTextAsync(path, string.Join("\r\n", text));
    }

    public static async Task ClearAllTextinFile(string path)
    {
        await File.WriteAllTextAsync(path, string.Empty);
    }
}
public class SettingsModel : IValidatableObject
{
    [Display(Name = "Use Tabs")]
    public bool UseTabs { get; set; }

    public bool Export { get; set; } = true;

    [Range(1, 8), Display(Name = "Tab Size")]
    public int? TabSize { get; set; } = 4;

    [Display(Name = "Convert Dates To")]
    public DateOutputType ConvertDatesTo { get; set; }

    [Display(Name = "Convert Nullables To")]
    public NullableOutputType ConvertNullablesTo { get; set; }

    [Display(Name = "To Camel Case")]
    public bool ToCamelCase { get; set; } = true;

    [Display(Name = "Remove Interface Prefix")]
    public bool RemoveInterfacePrefix { get; set; } = true;

    [Display(Name = "Generate Imports")]
    public bool GenerateImports { get; set; } = false;

    [Display(Name = "Use Kebab Case")]
    public bool UseKebabCase { get; set; }

    [Display(Name = "Append Model Suffix")]
    public bool AppendModelSuffix { get; set; } = true;

    [Display(Name = "Quotation Mark")]
    public QuotationMark QuotationMark { get; set; }

    [Display(Name = "Append New Line")]
    public bool AppendNewLine { get; set; } = true;

    [Display(Name = "String Enums")]
    public bool StringEnums { get; set; }

    [Display(Name = "Enum String To Camel Case")]
    public bool EnumStringToCamelCase { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!UseTabs && TabSize is null)
        {
            yield return new ValidationResult(
                "Provide Tab Size value or toggle Use Tabs.",
                new[] { nameof(TabSize), nameof(UseTabs) });
        }
    }

    public CodeConversionOptions MapToCodeConversionOptions()
        => new(Export, UseTabs, TabSize, ConvertDatesTo, ConvertNullablesTo, ToCamelCase, RemoveInterfacePrefix,
            GenerateImports ? ImportGenerationMode.Simple : ImportGenerationMode.None,
            UseKebabCase, AppendModelSuffix, QuotationMark, AppendNewLine, StringEnums, EnumStringToCamelCase);
}
