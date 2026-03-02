

namespace HospitioApi.Core.Options;
public class HospitioApiStorageAccountOptions
{
    public const string HospitioApiStorageAccount = "HospitioApiStorageAccount"; /** String must match property in appsettings.json file */

    public string UserFilesContainerName { get; set; } = string.Empty;
    public int ThumbnailFileQuality { get; set; } = default;
    public int ThumbnailFileWidth { get; set; } = default;
    public string ConnectionStringKey { get; set; } = string.Empty;
    public int FileQuality { get; set; }
    public int MinimumFileWidth { get; set; } 
    public int MaximumFileWidth { get; set; } 
    public int ExpirationTimeInDays { get; set; }
    public string BlobStorageBaseURL { get; set; } = string.Empty;

    //public long MaxFileSize { get; set; } = 100000;
}