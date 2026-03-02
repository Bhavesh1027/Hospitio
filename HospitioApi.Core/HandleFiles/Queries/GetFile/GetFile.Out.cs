using HospitioApi.Shared;


namespace HospitioApi.Core.HandleFiles.Queries.GetFile;
public class GetFileOut : BaseResponseOut
{
    public GetFileOut(
        string message, MemoryStream memoryStream,
        string fileDownloadName, string contentType) : base(message)
    {
        MemoryStream = memoryStream;
        FileDownloadName = fileDownloadName;
        ContentType = contentType;
    }
    public MemoryStream MemoryStream { get; }
    public string FileDownloadName { get; }
    public string ContentType { get; }
}
public class GetFileOutV1 : BaseResponseOut
{
    public GetFileOutV1(
        string message,
        string fileDownloadName, string contentType, string tempSasUri, DateTime expireAt) : base(message)
    {
        FileDownloadName = fileDownloadName;
        ContentType = contentType;
        TempSasUri = tempSasUri;
        ExpireAt = expireAt;
    }

    public string FileDownloadName { get; }
    public string ContentType { get; }
    public string TempSasUri { get; set; }
    public DateTime ExpireAt { get; set; }
}