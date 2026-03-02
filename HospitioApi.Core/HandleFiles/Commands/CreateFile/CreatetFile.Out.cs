
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleFiles.Commands.CreateFile;

public class CreateFileOut : BaseResponseOut
{
    public CreateFileOut(
        string message, MemoryStream memoryStream, string fileName,
        string location, string contentType, string tempSasUri) : base(message)
    {
        MemoryStream = memoryStream;
        Location = location;
        ContentType = contentType;
        FileName = fileName;
        TempSasUri = tempSasUri;
    }
    public MemoryStream MemoryStream { get; } = new();
    public string FileName { get; set; }
    public string Location { get; }
    public string ContentType { get; }
    public string TempSasUri { get; }

}
public class CreateFileOutV1 : BaseResponseOut
{
    public CreateFileOutV1(
       string message, string fileName,
       string location, string contentType, string tempSasUri, DateTime expireAt) : base(message)
    {
        FileName = fileName;
        Location = location;
        ContentType = contentType;
        TempSasUri = tempSasUri;
        ExpireAt = expireAt;
    }

    public string FileName { get; set; }
    public string Location { get; set; }
    public string ContentType { get; set; }
    public string TempSasUri { get; set; }
    public DateTime ExpireAt { get; set; }

}