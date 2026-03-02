using Microsoft.AspNetCore.Http;
using HospitioApi.Shared;

namespace HospitioApi.Core.Services.UserFiles;
public interface IUserFilesService
{
    Task<MemoryStream?> GetFileAsync(string path, CancellationToken cancellationToken);
    string GetContentTypeFromExtension(string? extension);
    Task<WebFile> CreateWebFileAsync(IFormFile file, CancellationToken cancellationToken);
    bool IsImage(string? filename);
    bool IsPdf(string? filename);
    Task<WebFile> CreateWebFileAsync(Stream fileStreamIn, string fileName, CancellationToken cancellationToken);
    Task<WebFileOut> UploadWebFileAsync(IFormFile file, CancellationToken cancellationToken);
    Task<WebFileOut> UploadWebFileWithRotationAsync(IFormFile file, RotateMode rotationmode, CancellationToken cancellationToken);
    Task<bool> DeleteBLOBFileAsync(string blobNamewithFileExtension, CancellationToken cancellationToken);
    Task<WebFileOut> UploadWebFileOnGivenPathAsync(IFormFile file, string directoryPath, CancellationToken cancellationToken, bool ExpireTime);
    Task<WebFileOutV1> GetFileTempSasURIAsync(string path, CancellationToken cancellationToken);
    Task<WebFileOut> UploadCSVFileOnGivenPathAsync(Stream csvContent, string directoryPath, CancellationToken cancellationToken, bool ExpireTime, string filename);
}