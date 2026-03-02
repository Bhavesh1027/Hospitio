using Microsoft.AspNetCore.Http;

namespace HospitioApi.Core.HandleVonage.Commands.InboundWebhook;

public class GetInboundWebhookIn
{
    public string? to { get; set; }
    public string? from { get; set; }
    public string? channel { get; set; }
    public string? message_uuid { get; set; }
    public string? text { get; set; }
    public DateTime? timestamp { get; set; }
    public string? message_type { get; set; }
    public ImageData? image { get; set; }
    public string? context_status { get; set; }
    public ProfileData? profile { get; set; }
    public VideoData? video { get; set; }
    public AudioData? audio { get; set; }
    public FileData? file { get; set; }
    public IFormFile? File { get; set; }
    public string? FileUrl { get; set; }
    public string? Attachment { get; set; }
}

public class ImageData
{
    public string? url { get; set; }
    public string? caption { get; set; }
}

public class ProfileData
{
    public string? name { get; set; }
}

public class VideoData
{
    public string? url { get; set; }
    public string? caption { get; set; }
}
public class AudioData
{
    public string? url { get; set; }
    public string? caption { get; set; }
}
public class FileData
{
    public string? url { get; set; }
    public string? caption { get; set; }
}

