using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Options;
using HospitioApi.Core.RabbitMQ;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Text;

namespace Hospitio.BackGroundService.Receiver;

public class ReceiveWPMessageConsume : BackgroundService
{
    private readonly ILogger<ReceiveWPMessageConsume> _logger;
    private IRabbitMQClient _rabbit;
    private readonly RabbitMQSettingsOptions _rabbitMQ;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IBackGroundServiceData _backGroundServiceData;
    private readonly BackGroundServicesSettingsOptions _time;


    public ReceiveWPMessageConsume(ILogger<ReceiveWPMessageConsume> logger, IRabbitMQClient rabbit, IOptions<RabbitMQSettingsOptions> rabbiMQ, IChatService chatService, IHubContext<ChatHub> hubContext, IServiceScopeFactory scopeFactory, IBackGroundServiceData backGroundServiceData, IOptions<BackGroundServicesSettingsOptions> time)
    {
        _rabbitMQ = rabbiMQ.Value;
        _rabbit = rabbit;
        _logger = logger;
        _chatService = chatService;
        _hubContext = hubContext;
        _scopeFactory = scopeFactory;
        _backGroundServiceData = backGroundServiceData;
        _time = time.Value;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        _logger.LogInformation("Customer Queue Comsume Service Started");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {

        _logger.LogInformation("Service Stepped");
        return Task.CompletedTask;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scopedb = _scopeFactory.CreateScope())
            {
                //var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var _db = scopedb.ServiceProvider.GetService<ApplicationDbContext>();


                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMQ.HostName,
                    Port = _rabbitMQ.Port,
                    UserName = _rabbitMQ.UserName,
                    Password = _rabbitMQ.Password
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(_rabbitMQ.Exchange, ExchangeType.Direct);
                channel.QueueDeclare(_rabbitMQ.WPMessageQueue,
                    true, false, false, null);

                channel.QueueBind(_rabbitMQ.WPMessageQueue, _rabbitMQ.Exchange,
                    "hospitio.receivewpmessage");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {

                    await ReceiveMessage(model, ea);
                    //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: _rabbitMQ.WPMessageQueue,
                        autoAck: true,
                        consumer: consumer);

                _logger.LogInformation($"Customer Queue Comsume Service Running{DateTime.Now}");
                await Task.Delay(TimeSpan.Parse(_time.ReceiveWPMessageConsumeTiming), stoppingToken);
            }
        }
    }
    public async Task ReceiveMessage(Object model, BasicDeliverEventArgs ea)
    {
        var scopedb = _scopeFactory.CreateScope();
        var _db = scopedb.ServiceProvider.GetService<ApplicationDbContext>();
        var body = ea.Body.ToArray();
        var json = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<GetInboundWebhookIn>(json);
        var responseFactory = scopedb.ServiceProvider.GetRequiredService<IUserFilesService>();

        if (message != null)
        {
            if (message.message_type == "text" || message.message_type == "file" || message.message_type == "video" || message.message_type == "audio" || message.message_type == "image")
            {

                if (message.message_type == "file")
                {
                    message.message_type = "Pdf";
                }

                if (message.message_type != "text")
                {
                    if (message.file != null && !string.IsNullOrEmpty(message.file.url))
                    {
                        // Check the file type and download accordingly
                        string fileExtension = GetFileExtensionFromUrlOrName(message.file.url, "pdf");

                        // Download and create an IFormFile
                        IFormFile file = await DownloadAndCreateFormFileAsync(message.file.url, "original" + fileExtension, message.message_type);
                        message.File = file;
                        message.Attachment = message.file.caption;
                        // Now, you can use 'file' as an IFormFile to save it to blob storage or perform other operations.
                    }
                    else if (message.image != null && !string.IsNullOrEmpty(message.image.url))
                    {
                        // Get the file extension from the URL or file name
                        string fileExtension = GetFileExtensionFromUrlOrName(message.image.url, "jpg");

                        // Download and create an IFormFile for the image
                        IFormFile imageFile = await DownloadAndCreateFormFileAsync(message.image.url, "image" + fileExtension, message.message_type);

                        message.File = imageFile;
                        message.Attachment = message.image.caption;
                        // Now, you can use 'imageFile' as an IFormFile for the image.
                    }
                    else if (message.video != null && !string.IsNullOrEmpty(message.video.url))
                    {
                        string fileExtension = GetFileExtensionFromUrlOrName(message.video.url, "mp4");

                        // Download and create an IFormFile for the video
                        IFormFile videoFile = await DownloadAndCreateFormFileAsync(message.video.url, "video" + fileExtension, message.message_type);
                        message.File = videoFile;
                        message.Attachment = message.video.caption;
                        // Now, you can use 'videoFile' as an IFormFile for the video.
                    }
                    else if (message.audio != null && !string.IsNullOrEmpty(message.audio.url))
                    {
                        // Get the file extension from the URL or file name
                        string fileExtension = GetFileExtensionFromUrlOrName(message.audio.url, "mpeg"); // Update the expected extension to "mpeg"

                        // Download and create an IFormFile for the audio
                        IFormFile audioFile = await DownloadAndCreateFormFileAsync(message.audio.url, "audio" + fileExtension, message.message_type);
                        message.File = audioFile;
                        message.Attachment = message.audio.caption;
                        // Now, you can use 'audioFile' as an IFormFile for the audio.
                    }


                    string documentName = ((UploadDocumentType)10).ToString();

                    var webFile = await responseFactory.UploadWebFileOnGivenPathAsync(message.File, documentName, CancellationToken.None , false);

                    message.FileUrl = webFile.Location;
                }
                await _backGroundServiceData.AddAnonymousUser(_db, message, CancellationToken.None);
            }
        }

    }
    private async Task<IFormFile> DownloadAndCreateFormFileAsync(string url, string fileName, string type)
    {
        using (var httpClient = new HttpClient())
        {
            // Download the file as bytes
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                // Create a Stream from the byte array
                var stream = new MemoryStream(fileBytes);

                // Get the file extension from the Content-Type header
                string contentType = response.Content.Headers.ContentType?.MediaType;

                string fileExtension = string.Empty;

                if (!string.IsNullOrEmpty(contentType))
                {
                    fileExtension = $".{contentType.Split('/')[1]}";
                }
                if (type == "audio")
                {
                    fileExtension = ".mp3";
                    contentType = "audio/mpeg";
                }

                // Use the file extension from the Content-Type header or the provided fileName
                if (string.IsNullOrEmpty(fileExtension))
                {
                    fileExtension = GetFileExtensionFromUrlOrName(url, "jpg"); // Provide a default extension if needed
                }

                // Create an IFormFile instance
                var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName + fileExtension)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType // Set the content type appropriately
                };

                return formFile;
            }
            else
            {
                // Handle the case where the download failed
                throw new Exception("Failed to download the file from the URL.");
            }
        }
    }
    private string GetFileExtensionFromUrlOrName(string urlOrName, string defaultExtension)
    {
        // Try to extract the file extension from the URL or name
        string fileExtension = Path.GetExtension(urlOrName);

        // If the extension is empty or not recognized, use the default extension
        if (string.IsNullOrEmpty(fileExtension) || !fileExtension.StartsWith("."))
        {
            // Use the default extension only if no extension is found
            fileExtension = string.IsNullOrEmpty(fileExtension) ? "." + defaultExtension : fileExtension;
        }

        return fileExtension;
    }
}
