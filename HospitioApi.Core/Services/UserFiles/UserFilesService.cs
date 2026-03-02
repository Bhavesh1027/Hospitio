using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Net;

namespace HospitioApi.Core.Services.UserFiles;
public class UserFilesService : IUserFilesService
{
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
    private readonly ILogger<UserFilesService> _logger;

    public UserFilesService(
        IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount,
        ILogger<UserFilesService> logger
        )
    {
        _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
        _logger = logger;
    }

    public async Task<MemoryStream?> GetFileAsync(string path, CancellationToken cancellationToken)
    {
        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);
        var blobClient = containerClient.GetBlobClient(path);
        var memoryStream = new MemoryStream();

        try
        {
            await blobClient.DownloadToAsync(memoryStream, cancellationToken);
        }
        catch (RequestFailedException e)
        {
            if (e.ErrorCode == "BlobNotFound")
            {
                return null;
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
    }
    public async Task<WebFileOutV1> GetFileTempSasURIAsync(string path, CancellationToken cancellationToken)
    {
        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);
        var blobClient = containerClient.GetBlobClient(path);

        BlobSasBuilder sas = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddDays(_hospitioApiStorageAccount.ExpirationTimeInDays)
        };
        // Allow read access
        sas.SetPermissions(BlobSasPermissions.Read);

        var memoryStream = new MemoryStream();

        try
        {
            await blobClient.DownloadToAsync(memoryStream, cancellationToken);
        }
        catch (RequestFailedException e)
        {
            if (e.ErrorCode == "BlobNotFound")
            {
                return null;
            }
        }
        memoryStream.Position = 0;

        if (memoryStream.Length > 0)
        {
            return new WebFileOutV1()
            {
                TempSasUri = blobClient.GenerateSasUri(sas).ToString(),
                ExpireAt = sas.ExpiresOn.UtcDateTime
            };
        }
        return null;
    }

    public async Task<WebFile> CreateWebFileAsync(Stream fileStreamIn, string fileName, CancellationToken cancellationToken)
    {
        fileStreamIn.Position = 0;
        var file = new FormFile(fileStreamIn, 0, fileStreamIn.Length, "file", fileName);
        return await CreateWebFileAsync(file, cancellationToken);
    }

    public async Task<WebFile> CreateWebFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        var contentType = GetContentTypeFromExtension(extension);

        string blobNameWithoutExtension = Guid.NewGuid().ToString();
        string blobFilePath = Path.ChangeExtension(Path.Combine(
            $"{DateTime.UtcNow.Year}",
            $"{DateTime.UtcNow.Month}",
            $"{DateTime.UtcNow.Day}",
            blobNameWithoutExtension), extension);

        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);

        using (var stream = file.OpenReadStream())
        {
            var blobClient = containerClient.GetBlobClient(blobFilePath);
            await UplodadToBlobStorageAsync(stream, blobClient, contentType, cancellationToken);
        }

        string? thumbnailBlobFilePath = null;

        if (IsImage(file.FileName))
        {
            try
            {
                using var stream = new MemoryStream();
                thumbnailBlobFilePath = Path.ChangeExtension(blobFilePath, $".w{_hospitioApiStorageAccount.ThumbnailFileWidth}.jpg");

                var blobClient = containerClient.GetBlobClient(thumbnailBlobFilePath);
                await CreateJpgThumbnailAsync(file.OpenReadStream(), stream, cancellationToken);
                await UplodadToBlobStorageAsync(stream, blobClient, contentType, cancellationToken);
            }
            catch (Exception e)
            {
                /** Only log and continue execution since thumbnail image files
                 * are not critical since original was already uploaded */
                _logger.LogError(e, "Could not create thumbnail image file for {blobFilePath}", blobFilePath);
            }
        }

        return new WebFile
        {
            Name = fileNameWithoutExtension ?? blobNameWithoutExtension,
            Location = blobFilePath,
            Size = file.Length,
            PreviewLocation = thumbnailBlobFilePath
        };

    }

    public async Task<WebFileOut> UploadWebFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        bool IsFileImage;
        if (IsImage(file.FileName))
        {
            IsFileImage = true;
        }
        else
        {
            IsFileImage = false;
        }

        var extension = IsFileImage ? "jpg" : Path.GetExtension(file.FileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        var contentType = IsFileImage ? "image/jpeg" : GetContentTypeFromExtension(extension);

        string blobNameWithoutExtension = Guid.NewGuid().ToString();
        string blobFilePath = Path.ChangeExtension(Path.Combine(
            $"{DateTime.UtcNow.Year}",
            $"{DateTime.UtcNow.Month}",
            $"{DateTime.UtcNow.Day}",
            blobNameWithoutExtension), extension);

        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);

        var inputOpStream = file.OpenReadStream();
        var outStream = new MemoryStream();

        var blobClient = containerClient.GetBlobClient(blobFilePath);

        if (IsImage(file.FileName))
        {
            await CreateJpegFileAsync(inputOpStream, outStream, RotateMode.None, cancellationToken , Path.GetExtension(file.FileName));
        }
        else
        {
            inputOpStream.Position = 0;
            inputOpStream.CopyTo(outStream);
        }
        //TODO put delete logic
        await UplodadToBlobStorageAsync(outStream, blobClient, contentType, cancellationToken);

        string? thumbnailBlobFilePath = null;

        if (IsImage(file.FileName))
        {
            try
            {
                using var streamT = new MemoryStream();
                thumbnailBlobFilePath = Path.ChangeExtension(blobFilePath, $".w{_hospitioApiStorageAccount.ThumbnailFileWidth}.jpg");
                var blobClientT = containerClient.GetBlobClient(thumbnailBlobFilePath);
                await CreateJpgThumbnailAsync(file.OpenReadStream(), streamT, cancellationToken);
                await UplodadToBlobStorageAsync(streamT, blobClientT, contentType, cancellationToken);
            }
            catch (Exception e)
            {
                /** Only log and continue execution since thumbnail image files
                 * are not critical since original was already uploaded */
                _logger.LogError(e, "Could not create thumbnail image file for {blobFilePath}", blobFilePath);
            }
        }

        outStream.Position = 0;

        return new WebFileOut
        {
            Name = (fileNameWithoutExtension ?? blobNameWithoutExtension) + "." + extension,
            Location = blobFilePath,
            Size = outStream.Length,
            PreviewLocation = thumbnailBlobFilePath,
            MemoryStream = outStream,
            ContentType = contentType
        };
    }

    //Upload file with directory structure
    public async Task<WebFileOut> UploadWebFileOnGivenPathAsync(IFormFile file, string directoryPath, CancellationToken cancellationToken, bool ExpireTime)
    {
        bool IsFileImage;
        if (IsImage(file.FileName))
        {
            IsFileImage = true;
        }
        else
        {
            IsFileImage = false;
        }

        var extension = IsFileImage ? "jpg" : Path.GetExtension(file.FileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        var contentType = IsFileImage ? "image/jpeg" : GetContentTypeFromExtension(extension);

        string blobNameWithoutExtension = Guid.NewGuid().ToString();
        string blobFilePath = Path.ChangeExtension(Path.Combine(directoryPath,
            blobNameWithoutExtension), extension);

        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);

        var inputOpStream = file.OpenReadStream();
        var outStream = new MemoryStream();

        var blobClient = containerClient.GetBlobClient(blobFilePath);

        if (IsImage(file.FileName))
        {
            await CreateJpegFileAsync(inputOpStream, outStream, RotateMode.None, cancellationToken, Path.GetExtension(file.FileName));
        }
        else
        {
            inputOpStream.Position = 0;
            inputOpStream.CopyTo(outStream);
        }
        //TODO put delete logic
        var Sas = await UplodadToBlobStorageWithTempURIAsync(outStream, blobClient, contentType, _hospitioApiStorageAccount.ExpirationTimeInDays, cancellationToken , ExpireTime);
        if (Sas == null)
        {
            _logger.LogError("Could not get sas of iamge file for {blobFilePath}", blobFilePath);
        }
        outStream.Position = 0;
        var tempSasURI = blobClient.GenerateSasUri(Sas).ToString();

        return new WebFileOut
        {
            Name = (fileNameWithoutExtension ?? blobNameWithoutExtension) + "." + extension,
            Location = blobFilePath,
            Size = outStream.Length,
            //PreviewLocation = thumbnailBlobFilePath,
            MemoryStream = outStream,
            ContentType = contentType,
            TempSasUri = tempSasURI is not null ? tempSasURI : string.Empty,
            ExpireAt = Sas is not null ? Sas.ExpiresOn.UtcDateTime : DateTime.UtcNow.AddDays(_hospitioApiStorageAccount.ExpirationTimeInDays)
        };
    }

    public async Task<WebFileOut> UploadCSVFileOnGivenPathAsync(Stream csvContent, string directoryPath, CancellationToken cancellationToken, bool ExpireTime , string filename)
    {
        bool IsFileImage;
        if (IsImage(filename))
        {
            IsFileImage = true;
        }
        else
        {
            IsFileImage = false;
        }

        var extension = IsFileImage ? "jpg" : Path.GetExtension(filename);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
        var contentType = IsFileImage ? "image/jpeg" : GetContentTypeFromExtension(extension);

        string blobNameWithoutExtension = Guid.NewGuid().ToString();
        string blobFilePath = Path.ChangeExtension(Path.Combine(directoryPath,
            blobNameWithoutExtension), extension);

        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);

        var blobClient = containerClient.GetBlobClient(blobFilePath);

        var Sas = await UplodadToBlobStorageWithTempURIAsync(csvContent, blobClient, contentType, _hospitioApiStorageAccount.ExpirationTimeInDays, cancellationToken, ExpireTime);
        if (Sas == null)
        {
            _logger.LogError("Could not get sas of Image file for {blobFilePath}", blobFilePath);
        }
        var tempSasURI = blobClient.GenerateSasUri(Sas).ToString();

        return new WebFileOut
        {
            Name = (fileNameWithoutExtension ?? blobNameWithoutExtension) + "." + extension,
            Location = blobFilePath,
            //Size = outStream.Length,
            //PreviewLocation = thumbnailBlobFilePath,
            //MemoryStream = outStream,
            ContentType = contentType,
            TempSasUri = tempSasURI is not null ? tempSasURI : string.Empty,
            ExpireAt = Sas is not null ? Sas.ExpiresOn.UtcDateTime : DateTime.UtcNow.AddDays(_hospitioApiStorageAccount.ExpirationTimeInDays)
        };
    }
    private static async Task<BlobSasBuilder> UplodadToBlobStorageWithTempURIAsync(Stream stream, BlobClient blobClient, string contentType, int expirationTimeInDays, CancellationToken cancellationToken , bool ExpireTime)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
        var blobUploadOptions = new BlobUploadOptions { HttpHeaders = blobHttpHeader };

        var uploadedBlob = await blobClient.UploadAsync(stream, blobUploadOptions, cancellationToken).WaitAsync(cancellationToken);
        var rawResponse = uploadedBlob.GetRawResponse();
        if (rawResponse.Status != (int)HttpStatusCode.Created)
        {
            throw new AppException(rawResponse.ReasonPhrase,
                Enum.IsDefined(typeof(AppStatusCodeError), rawResponse.Status)
                    ? (AppStatusCodeError)rawResponse.Status
                    : AppStatusCodeError.InternalServerError500
                );
        }

        // Create a user SAS that only allows reading for a minute
        BlobSasBuilder sas = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = ExpireTime ? DateTimeOffset.UtcNow.AddDays(365) : DateTimeOffset.UtcNow.AddDays(expirationTimeInDays)
        };
        sas.SetPermissions(BlobSasPermissions.Read);
        return sas;

    }

    private static async Task UplodadToBlobStorageAsync(Stream stream, BlobClient blobClient, string contentType, CancellationToken cancellationToken)
    {
        stream.Position = 0;
        var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
        var blobUploadOptions = new BlobUploadOptions { HttpHeaders = blobHttpHeader };

        var uploadedBlob = await blobClient.UploadAsync(stream, blobUploadOptions, cancellationToken);
        var rawResponse = uploadedBlob.GetRawResponse();
        if (rawResponse.Status != (int)HttpStatusCode.Created)
        {
            throw new AppException(rawResponse.ReasonPhrase,
                Enum.IsDefined(typeof(AppStatusCodeError), rawResponse.Status)
                    ? (AppStatusCodeError)rawResponse.Status
                    : AppStatusCodeError.InternalServerError500
                );
        }
    }

    private async Task CreateJpegFileAsync(Stream inStream, Stream outStream, RotateMode angle, CancellationToken cancellationToken, string extension)
    {
        using Image image = Image.Load(inStream);


        if (image.Width < _hospitioApiStorageAccount.MinimumFileWidth)
        {
            image.Mutate(x => x.Resize(_hospitioApiStorageAccount.MinimumFileWidth, 0));
        }
        else if (image.Width > _hospitioApiStorageAccount.MaximumFileWidth)
        {
            image.Mutate(x => x.Resize(_hospitioApiStorageAccount.MaximumFileWidth, 0));
        }
        else
        {
            //** image.Mutate not required**//
        }
        image.Mutate(x => x.Rotate(angle));
        if (extension == ".png")
        {
            await image.SaveAsPngAsync(outStream, cancellationToken);
        }
        else
        {
            await image.SaveAsJpegAsync(
                outStream,
                new JpegEncoder { ColorType = JpegEncodingColor.Rgb, Quality = _hospitioApiStorageAccount.FileQuality },
                cancellationToken);
        }

    }

    private async Task CreateJpgThumbnailAsync(Stream inStream, Stream outStream, CancellationToken cancellationToken)
    {
        using Image image = Image.Load(inStream);
        image.Mutate(x => x.Resize(_hospitioApiStorageAccount.ThumbnailFileWidth, 0));
        await image.SaveAsJpegAsync(
            outStream,
            new JpegEncoder { Quality = _hospitioApiStorageAccount.ThumbnailFileQuality },
            cancellationToken);
    }

    public string GetContentTypeFromExtension(string? extension)
    {
        extension ??= "";
        extension = extension.Replace(@".", "").ToLower();
        return extension switch
        {
            "bmp" => "image/bmp",
            "jpg" => "image/jpeg",
            "jpeg" => "image/jpeg",
            "jpe" => "image/jpeg",
            "jfif" => "image/jpeg",
            "jif" => "image/jpeg",
            "pdf" => "application/pdf",
            "png" => "image/png",
            "dib" => "image/dib",
            "tiff" => "image/tiff",
            "tif" => "image/tif",
            "gif" => "image/gif",
            "mp4" => "video/mp4",
            "mp3" => "audio/mpeg",
            "json" => "application/json",
            "csv" => "text/csv",
            "zip" => "application/zip",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => throw new AppException($"Extension {extension} not allowed.", AppStatusCodeError.Forbidden403)
        };
    }

    public bool IsImage(string? filename)
    {
        if (filename == null) { return false; }
        var extension = Path.GetExtension(filename).Replace(@".", "").ToLower();
        var allowedImageExtensions = new List<string> { "bmp", "jpg", "jpeg", "jpe", "jfif", "jif", "png", "dib", "tiff", "tif", "gif" };
        return allowedImageExtensions.Contains(extension);
    }

    public bool IsPdf(string? filename)
    {
        if (filename == null) { return false; }
        var extension = Path.GetExtension(filename).Replace(@".", "").ToLower();
        return extension is "pdf";
    }

    public async Task<WebFileOut> UploadWebFileWithRotationAsync(IFormFile file, RotateMode rotationmode, CancellationToken cancellationToken)
    {
        bool IsFileImage;
        if (IsImage(file.FileName))
        {
            IsFileImage = true;
        }
        else
        {
            return new WebFileOut();
        }

        var extension = IsFileImage ? "jpg" : Path.GetExtension(file.FileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        var contentType = IsFileImage ? "image/jpeg" : GetContentTypeFromExtension(extension);

        string blobNameWithoutExtension = Guid.NewGuid().ToString();
        string blobFilePath = Path.ChangeExtension(Path.Combine(
            $"{DateTime.UtcNow.Year}",
            $"{DateTime.UtcNow.Month}",
            $"{DateTime.UtcNow.Day}",
            blobNameWithoutExtension), extension);

        var blobServiceClient = new BlobServiceClient(_hospitioApiStorageAccount.ConnectionStringKey);
        var containerClient = blobServiceClient.GetBlobContainerClient(_hospitioApiStorageAccount.UserFilesContainerName);

        var inputOpStream = file.OpenReadStream();
        var outStream = new MemoryStream();

        var blobClient = containerClient.GetBlobClient(blobFilePath);

        if (IsImage(file.FileName))
        {
            await CreateJpegFileAsync(inputOpStream, outStream, rotationmode, cancellationToken, Path.GetExtension(file.FileName));
        }
        else
        {
            inputOpStream.Position = 0;
            inputOpStream.CopyTo(outStream);
        }
        await UplodadToBlobStorageAsync(outStream, blobClient, contentType, cancellationToken);

        string? thumbnailBlobFilePath = null;

        if (IsImage(file.FileName))
        {
            try
            {
                using var streamT = new MemoryStream();
                thumbnailBlobFilePath = Path.ChangeExtension(blobFilePath, $".w{_hospitioApiStorageAccount.ThumbnailFileWidth}.jpg");
                var blobClientT = containerClient.GetBlobClient(thumbnailBlobFilePath);
                await CreateJpgThumbnailAsync(file.OpenReadStream(), streamT, cancellationToken);
                await UplodadToBlobStorageAsync(streamT, blobClientT, contentType, cancellationToken);
            }
            catch (Exception e)
            {
                /** Only log and continue execution since thumbnail image files
                 * are not critical since original was already uploaded */
                _logger.LogError(e, "Could not create thumbnail image file for {blobFilePath}", blobFilePath);
            }
        }
        outStream.Position = 0;

        return new WebFileOut
        {
            Name = (fileNameWithoutExtension ?? blobNameWithoutExtension) + "." + extension,
            Location = blobFilePath,
            Size = outStream.Length,
            PreviewLocation = thumbnailBlobFilePath,
            MemoryStream = outStream,
            ContentType = contentType
        };
    }

    public async Task<bool> DeleteBLOBFileAsync(string blobNamewithFileExtension, CancellationToken cancellationToken)
    {
        BlobClient blobClient = new BlobClient(_hospitioApiStorageAccount.ConnectionStringKey, _hospitioApiStorageAccount.UserFilesContainerName, blobNamewithFileExtension);
        bool isExistorDeleted = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, cancellationToken);
        return isExistorDeleted;
    }
}

public class WebFileOut
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; } = 0;
    public string? PreviewLocation { get; set; }
    public MemoryStream MemoryStream { get; set; } = new();
    public string ContentType { get; set; } = string.Empty;
    public string TempSasUri { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
}
public class WebFileOutV1
{
    public string TempSasUri { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }

}
