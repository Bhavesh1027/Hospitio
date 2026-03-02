

namespace HospitioApi.Shared;
public class WebFile
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; } = 0;
    public string? PreviewLocation { get; set; }
    public MemoryStream MemoryStream { get; set; } = new();
    public string ContentType { get; set; } = string.Empty;

}