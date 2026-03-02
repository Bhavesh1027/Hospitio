using HospitioApi.Shared;


namespace HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyFile;

public class GetPropertyFileOut : BaseResponseOut
{
    public GetPropertyFileOut(
        string message, MemoryStream memoryStream, string fileName,
        long fileId, string contentType) : base(message)
    {
        MemoryStream = memoryStream;
        ImageId = fileId;
        ContentType = contentType;
        FileName = fileName;
    }
    public MemoryStream MemoryStream { get; } = new();
    public string FileName { get; set; }
    public long ImageId { get; set; }
    public string ContentType { get; }
}