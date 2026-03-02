using Microsoft.AspNetCore.Http;


namespace HospitioApi.Core.HandleFiles.Commands.CreateFile;
public class CreateFileIn
{
    public CreateFileIn(IFormFile file, string? containerName, string documentType)
    {
        File = file;
        ContainerName = containerName;
        DocumentType = documentType;
    }
    public IFormFile File { get; set; }
    public string DocumentType { get; set; }
    public string? ContainerName { get; set; }

}