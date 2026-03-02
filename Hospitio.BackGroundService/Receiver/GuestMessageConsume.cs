using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared.Enums;
using System.Text;
using System.Text.RegularExpressions;

namespace HospitioApi.BackGroundService.Receiver;

public class GuestMessageConsume : BackgroundService
{
    private readonly ILogger<GuestMessageConsume> _logger;
    private readonly RabbitMQSettingsOptions _rabbitMQ;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly BackGroundServicesSettingsOptions _time;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly VonageTemplateEmailSettingsOptions _vonageTemplateEmailSettingsOptions;
    private readonly SMTPEmailSettingsOptions _smtpEmailSettings;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
    private readonly string WebRootPath = string.Empty;
    private readonly HospitioApiStorageAccountOptions _storageAccount;

    public GuestMessageConsume(ILogger<GuestMessageConsume> logger, IOptions<RabbitMQSettingsOptions> rabbiMQ, IChatService chatService, IHubContext<ChatHub> hubContext, IServiceScopeFactory scopeFactory, IOptions<BackGroundServicesSettingsOptions> time, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, IOptions<VonageTemplateEmailSettingsOptions> vonageTemplateEmailSettingsOptions, IOptions<SMTPEmailSettingsOptions> smtpEmailSettings, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, IOptions<HospitioApiStorageAccountOptions> storageAccount)
    {
        _rabbitMQ = rabbiMQ.Value;
        _logger = logger;
        _chatService = chatService;
        _hubContext = hubContext;
        _scopeFactory = scopeFactory;
        _time = time.Value;
        _frontEndLinksSettings=frontEndLinksSettings.Value;
        _vonageTemplateEmailSettingsOptions = vonageTemplateEmailSettingsOptions.Value;
        _smtpEmailSettings = smtpEmailSettings.Value;
        Environment=_environment;
        WebRootPath = Environment.WebRootPath;
        _storageAccount = storageAccount.Value;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Guest Message Service Started");

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
            channel.QueueDeclare(_rabbitMQ.GuestMessageQueue,
                true, false, false, null);

            channel.QueueBind(_rabbitMQ.GuestMessageQueue, _rabbitMQ.Exchange,
                "hospitio.guestmessage");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var task = ReceiveMessage(model, ea, stoppingToken);
                task.Wait();
            };
            channel.BasicConsume(queue: _rabbitMQ.GuestMessageQueue,
                    autoAck: true,
                    consumer: consumer);

            _logger.LogInformation($"Guest Message Comsume Service Running{DateTime.Now}");

            await Task.Delay(TimeSpan.Parse(_time.GuestMessageConsumeTiming), stoppingToken);
        }
    }
    public async Task ReceiveMessage(Object model, BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        using var scopedb = _scopeFactory.CreateScope();
        var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var _vonageService = scopedb.ServiceProvider.GetRequiredService<IVonageService>();
        var _mail = scopedb.ServiceProvider.GetRequiredService<ISendEmail>();
        var body = ea.Body.ToArray();
        var json = Encoding.UTF8.GetString(body);

        var message = JsonConvert.DeserializeObject<CustomerGuestJorneyDetails>(json);
        var task = MainMethod(message, _vonageService, _mail, cancellationToken, _db);
        task.Wait();
    }
    public async Task<int> SendMessageInChatForBoth(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        using var scopedb = _scopeFactory.CreateScope();
        var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId.ToString(), Convert.ToInt32(receiverUserType));

        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, (int)MsgReqTypeEnum.journeyMessage, null);

        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);
        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
    public async Task<int> SendMessageInChatForCustomer(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        using var scopedb = _scopeFactory.CreateScope();
        var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, (int)MsgReqTypeEnum.journeyMessage, null);

        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        return 1;
    }
    public async Task<dynamic> SendWhatsappMessage(CustomerGuestJorneyDetails message, IVonageService vonageService)
    {
        bool hasBodyParameters = false;

        var realTemplateMessage = message.TempletMessage;
        var templateMessage = realTemplateMessage;
        var buttons = message.Buttons;
        var buttonObject = (buttons != null) ? JsonConvert.DeserializeObject<List<button>>(buttons) : null; // buttonobject null >> no button
        bool hasButton = buttonObject != null;
        var ButtonParameters = new List<string>(); // count - 0 >> no parameter
        Dictionary<int, string> ButtonParametesrWithIndex = new Dictionary<int, string>();
        if (buttonObject != null && buttonObject.Count > 0)
        {
            int counter = 0;
            foreach (var item in buttonObject)
            {
                if (item.type == "URL")
                {
                    ButtonParametesrWithIndex[counter] =  $"{message.GuestURL}";
                    ButtonParameters.Add($"{message.GuestURL}");
                    item.value = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                }
                counter++;
            }
        }
        var attchement = (JsonConvert.SerializeObject(buttonObject) == "null") ? null : JsonConvert.SerializeObject(buttonObject);
        var bodyplaceholderMatches = realTemplateMessage != null ? System.Text.RegularExpressions.Regex.Matches(realTemplateMessage, @"\{([^}]+)\}").ToList() : null;

        hasBodyParameters = (bodyplaceholderMatches.Count > 0);
        int templateMessagePlaceholders = bodyplaceholderMatches.Count;

        Guid MessgaeUuid = Guid.Empty;
        List<string> BodyParameters = new List<string>();
        if (bodyplaceholderMatches?.Count > 0)
        {
            string[] temparr = new string[bodyplaceholderMatches.Count];
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        temparr[count] = message.BussinessName;
                        templateMessage = templateMessage.Replace("{hotel_name}", message.BussinessName);
                        break;
                    case "{booking_days}":
                        temparr[count] = message.BookingDays.ToString();
                        templateMessage = templateMessage.Replace("{booking_days}", message.BookingDays.ToString());
                        break;
                    case "{hotel_address}":
                        temparr[count] = message.BussinessAddress;
                        templateMessage = templateMessage.Replace("{hotel_address}", message.BussinessAddress);
                        break;
                    case "{hotel_phonenumber}":
                        temparr[count] = message.BussinessPhoneNumber;
                        templateMessage = templateMessage.Replace("{hotel_phonenumber}", message.BussinessPhoneNumber);
                        break;
                    case "{guest_name}":
                        temparr[count] = message.GuestName;
                        templateMessage = templateMessage.Replace("{guest_name}", message.GuestName);
                        break;
                    case "{guest_reservationnumber}":
                        temparr[count] = message.ReservationNumber.ToString();
                        templateMessage = templateMessage.Replace("{guest_reservationnumber}", message.ReservationNumber);
                        break;
                    case "{checkin_date}":
                        temparr[count] = message.CheckinDate.ToString();
                        templateMessage = templateMessage.Replace("{checkin_date}", message.CheckinDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{checkout_date}":
                        temparr[count] = message.CheckoutDate.ToString();
                        templateMessage = templateMessage.Replace("{checkout_date}", message.CheckoutDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{guest_url}":
                        temparr[count] = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                        templateMessage = templateMessage.Replace("{guest_url}", $"[Click Here]({_frontEndLinksSettings.GuestPortal}?id={message.GuestURL})");
                        break;
                }
                count++;
            }

            BodyParameters = temparr.ToList();
        }
        string receiver = message.Phone.ToString().IndexOf('+') == 0 ? message.Phone.ToString().Trim().Substring(1) : message.Phone.ToString();

        string sender = message.CustomerWhatsAppNumber.ToString().IndexOf('+') == 0 ? message.CustomerWhatsAppNumber.ToString().Trim().Substring(1) : message.CustomerWhatsAppNumber.ToString();
        string templateName = message.TemplateName;

        if (!string.IsNullOrWhiteSpace(message.APPId) &&
            !string.IsNullOrWhiteSpace(message.PrivateKey) &&
            !string.IsNullOrWhiteSpace(realTemplateMessage) &&
            !string.IsNullOrWhiteSpace(receiver) &&
            !string.IsNullOrWhiteSpace(sender) &&
            !string.IsNullOrWhiteSpace(templateName)
            )
        {
            if (IsValidPhoneNumber(receiver) && IsValidPhoneNumber(sender))
            {
                var response = await vonageService.SendWhatsappTemplateMessage(message.APPId, message.PrivateKey, realTemplateMessage??"", receiver, sender, templateName, BodyParameters, hasButton, ButtonParametesrWithIndex);
                MessgaeUuid = response?.MessageUuid;
            }
        }
        return new { MessageId = MessgaeUuid, TemplateMessage = templateMessage, Attachment = attchement };
    }
    public dynamic MakeWhatsappMessageForSendInChat(CustomerGuestJorneyDetails message, IVonageService vonageService)
    {
        var realTemplateMessage = message.TempletMessage;
        var templateMessage = realTemplateMessage;
        var buttons = message.Buttons;
        var buttonObject = (buttons != null) ? JsonConvert.DeserializeObject<List<button>>(buttons) : null;
        var ButtonParameters = new List<string>();
        if (buttonObject != null && buttonObject.Count > 0)
        {
            foreach (var item in buttonObject)
            {
                if (item.type == "URL")
                {
                    ButtonParameters.Add($"{message.GuestURL}");
                    item.value = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                }
                if (item.type == "PHONE_NUMBER")
                {
                    item.value = $"{message.BussinessPhoneNumber}";
                }
            }
        }
        var attchement = (JsonConvert.SerializeObject(buttonObject) == "null") ? null : JsonConvert.SerializeObject(buttonObject);
        var bodyplaceholderMatches = realTemplateMessage != null ? System.Text.RegularExpressions.Regex.Matches(realTemplateMessage, @"\{([^}]+)\}").ToList() : null;

        if (bodyplaceholderMatches?.Count > 0)
        {
            string[] temparr = new string[bodyplaceholderMatches.Count];
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        temparr[count] = message.BussinessName;
                        templateMessage = templateMessage.Replace("{hotel_name}", message.BussinessName);
                        break;
                    case "{booking_days}":
                        temparr[count] = message.BookingDays.ToString();
                        templateMessage = templateMessage.Replace("{booking_days}", message.BookingDays.ToString());
                        break;
                    case "{hotel_address}":
                        temparr[count] = message.BussinessAddress;
                        templateMessage = templateMessage.Replace("{hotel_address}", message.BussinessAddress);
                        break;
                    case "{hotel_phonenumber}":
                        temparr[count] = message.BussinessPhoneNumber;
                        templateMessage = templateMessage.Replace("{hotel_phonenumber}", message.BussinessPhoneNumber);
                        break;
                    case "{guest_name}":
                        temparr[count] = message.GuestName;
                        templateMessage = templateMessage.Replace("{guest_name}", message.GuestName);
                        break;
                    case "{guest_reservationnumber}":
                        temparr[count] = message.ReservationNumber.ToString();
                        templateMessage = templateMessage.Replace("{guest_reservationnumber}", message.ReservationNumber);
                        break;
                    case "{checkin_date}":
                        temparr[count] = message.CheckinDate.ToString();
                        templateMessage = templateMessage.Replace("{checkin_date}", message.CheckinDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{checkout_date}":
                        temparr[count] = message.CheckoutDate.ToString();
                        templateMessage = templateMessage.Replace("{checkout_date}", message.CheckoutDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{guest_url}":
                        temparr[count] = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                        templateMessage = templateMessage.Replace("{guest_url}", $"[Click Here]({_frontEndLinksSettings.GuestPortal}?id={message.GuestURL})");
                        break;
                }
                count++;
            }
        }
        return new { TemplateMessage = templateMessage, Attachment = attchement };
    }
    private string PopulateEmailTemplate(CustomerGuestJorneyDetails message)
    {
        string emailTemplate = string.Empty;
        string path = Path.Combine(WebRootPath, "html/") + _vonageTemplateEmailSettingsOptions.EmailTemplate;

        #region for a button
        var buttons = message.Buttons;
        var buttonObject = (buttons != null) ? JsonConvert.DeserializeObject<List<button>>(buttons) : null;
        if (buttonObject != null && buttonObject.Count > 0)
        {
            foreach (var item in buttonObject)
            {
                if (item.type == "URL")
                {
                    item.value = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                }
                if (item.type == "PHONE_NUMBER")
                {
                    item.value = $"{message.BussinessPhoneNumber}";
                }

            }
        }
        var attchement = (JsonConvert.SerializeObject(buttonObject) == "null") ? null : JsonConvert.SerializeObject(buttonObject);
        #endregion

        #region for a message
        var realTemplateMessage = message.TempletMessage;
        var templateMessage = realTemplateMessage;

        var bodyplaceholderMatches = realTemplateMessage != null ? System.Text.RegularExpressions.Regex.Matches(realTemplateMessage, @"\{([^}]+)\}").ToList() : null;

        if (bodyplaceholderMatches?.Count > 0)
        {
            string[] temparr = new string[bodyplaceholderMatches.Count];
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        temparr[count] = message.BussinessName;
                        templateMessage = templateMessage.Replace("{hotel_name}", message.BussinessName);
                        break;
                    case "{booking_days}":
                        temparr[count] = message.BookingDays.ToString();
                        templateMessage = templateMessage.Replace("{booking_days}", message.BookingDays.ToString());
                        break;
                    case "{hotel_address}":
                        temparr[count] = message.BussinessAddress;
                        templateMessage = templateMessage.Replace("{hotel_address}", message.BussinessAddress);
                        break;
                    case "{hotel_phonenumber}":
                        temparr[count] = message.BussinessPhoneNumber;
                        templateMessage = templateMessage.Replace("{hotel_phonenumber}", message.BussinessPhoneNumber);
                        break;
                    case "{guest_name}":
                        temparr[count] = message.GuestName;
                        templateMessage = templateMessage.Replace("{guest_name}", message.GuestName);
                        break;
                    case "{guest_reservationnumber}":
                        temparr[count] = message.ReservationNumber.ToString();
                        templateMessage = templateMessage.Replace("{guest_reservationnumber}", message.ReservationNumber);
                        break;
                    case "{checkin_date}":
                        temparr[count] = message.CheckinDate.ToString();
                        templateMessage = templateMessage.Replace("{checkin_date}", message.CheckinDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{checkout_date}":
                        temparr[count] = message.CheckoutDate.ToString();
                        templateMessage = templateMessage.Replace("{checkout_date}", message.CheckoutDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{guest_url}":
                        temparr[count] = $"{_frontEndLinksSettings.GuestPortal}?id={message.GuestURL}";
                        templateMessage = templateMessage.Replace("{guest_url}", $"[Click Here]({_frontEndLinksSettings.GuestPortal}?id={message.GuestURL})");
                        break;
                }
                count++;
            }
        }
        #endregion
        string strHTMLforButton = string.Empty;
        try
        {
            using (var reader = new StreamReader(path))
            {
                emailTemplate = reader.ReadToEnd();
            }
            if (buttonObject != null)
            {
                foreach (var button in buttonObject)
                {
                    string temp = string.Empty;
                    if (button.type == "URL")
                    {
                        temp = $"<tr><td style=\"padding: 0 30px 20px 30px; text-align: center; width: 100%;\">\r\n                                    <p style=\"margin-top: 0px;font-size: 16px;color: #333;background-color: #d3d3d3;padding: 10px;border-radius: 5px;margin-bottom: 0px;\">\r\n                                        <a href=\"{button.value}\" style=\"color: #007BFF; text-decoration: none;\">{button.text}</a>\r\n                                    </p>\r\n                                </td></tr>";
                    }
                    else if (button.type == "PHONE_NUMBER")
                    {
                        temp = $"<tr>\r\n                                <td style=\"padding: 0 30px 20px 30px; text-align: center; width: 100%;\">\r\n                                    <p style=\"margin-top: 0px;font-size: 16px;color: #333;background-color: #d3d3d3;padding: 10px;border-radius: 5px;margin-bottom: 0px;\">\r\n                                        {button.text}: <a href=\"tel:{button.value}\" style=\"color: #007BFF; text-decoration: none;\">{button.value}</a>\r\n                                    </p>\r\n                                </td>\r\n                            </tr>";
                    }
                    strHTMLforButton += temp;
                }
            }
            if (!string.IsNullOrWhiteSpace(templateMessage))
            {
                templateMessage = templateMessage.Replace("\n", "<br>");
                emailTemplate = emailTemplate.Replace("<!--{TemplateBodyMsgTag}-->", templateMessage);
            }
            if (!string.IsNullOrWhiteSpace(strHTMLforButton))
            {
                emailTemplate = emailTemplate.Replace("<!--{TemplateButtonTag}-->", strHTMLforButton);
            }
            if (!string.IsNullOrWhiteSpace(message.BussinessName))
            {
                emailTemplate = emailTemplate.Replace("<!--{CustomerBussinessName}-->", message.BussinessName);
            }
            if (!string.IsNullOrWhiteSpace(message.CustomerColour))
            {
                emailTemplate = emailTemplate.Replace("<!--{CustomerColor}-->", message.CustomerColour);
            }
            if (!string.IsNullOrWhiteSpace(message.CustomerLogoURL))
            {
                emailTemplate = emailTemplate.Replace("<!--{ImageBlobURL}-->", $"{_storageAccount.BlobStorageBaseURL}{message.CustomerLogoURL}");
            }
            if (!string.IsNullOrWhiteSpace(message.BussinessName))
            {
                emailTemplate = emailTemplate.Replace("<!--{AltTag_For_Image}-->", $"{message.BussinessName} Logo");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reading email template in reset password.");
        }
        return emailTemplate;
    }
    public async Task<bool> SendMailToGuest(CustomerGuestJorneyDetails message, ISendEmail _mail, CancellationToken cancellationToken)
    {
        var fullEmailBody = PopulateEmailTemplate(message);

        SendEmailOptions sendEmail = new SendEmailOptions();
        sendEmail.Subject = _vonageTemplateEmailSettingsOptions.Subject;
        sendEmail.Addresslist = message.Email;
        sendEmail.IsHTML = true;
        sendEmail.Body = fullEmailBody;
        sendEmail.IsNoReply =true;

        var isSend = await _mail.ExecuteAsync(sendEmail, cancellationToken);
        return isSend;
    }
    public async Task SendTemplateInWebChat(CustomerGuestJorneyDetails message, IVonageService _vonageService, ApplicationDbContext _db, int source)
    {
        var SenderUserType = ((int)UserTypeEnum.Customer).ToString();
        var receiverUserType = ((int)UserTypeEnum.Guest).ToString();

        var channels = await _chatService.GetChannelDataFromUsersDetail(message.GuestId, receiverUserType, message.CustomerUserId, SenderUserType);
        var result = MakeWhatsappMessageForSendInChat(message, _vonageService);
        if (channels != null)
        {
            if (source == (int)MessageSourceEnum.WebChat)
            {

                await SendMessageInChatForBoth(channels.Id, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, result.TemplateMessage, null, (int)MessageSourceEnum.WebChat, "Template", result.Attachment, null, null);
            }
            else if (source == (int)MessageSourceEnum.whatsapp)
            {
                await SendMessageInChatForCustomer(channels.Id, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, result.TemplateMessage, null, (int)MessageSourceEnum.whatsapp, "Template", result.Attachment, null, null);
            }
        }
        else
        {
            var chatId = await _chatService.CreateChat(_db, message.CustomerUserId.ToString(), SenderUserType, message.GuestId, ((ChatUserTypeEnum)int.Parse(receiverUserType)).ToString());
            if (source == (int)MessageSourceEnum.WebChat)
            {

                await SendMessageInChatForBoth(chatId, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, result.TemplateMessage, null, (int)MessageSourceEnum.WebChat, "Template", result.Attachment, null, null);
            }
            else if (source == (int)MessageSourceEnum.whatsapp)
            {
                await SendMessageInChatForCustomer(chatId, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, result.TemplateMessage, null, (int)MessageSourceEnum.whatsapp, "Template", result.Attachment, null, null);
            }
        }
    }
    public async Task MainMethod(CustomerGuestJorneyDetails message, IVonageService _vonageService, ISendEmail _mail, CancellationToken cancellationToken, ApplicationDbContext _db)
    {
        if (message?.ActionName != null)
        {
            switch (message.ActionName)
            {
                case "GUEST_MESSAGE":
                    if (message.EligibleForWhatsappCommunication == "1")
                    {
                        if (message.CustomerUserId != 0 && message.GuestId != 0 && !string.IsNullOrEmpty(message.VonageTemplateId) && message.IsActive == true)
                        {
                            if (message.VonageTemplateStatus == "APPROVED")
                            {
                                var result = await SendWhatsappMessage(message, _vonageService);
                                await SendTemplateInWebChat(message, _vonageService, _db, (int)MessageSourceEnum.whatsapp).ConfigureAwait(true);
                            }
                        }
                    }
                    if (message.EligibleForSMSCommunication == "1" && message.SMSTwoWayCommunication == "True")
                    {
                        var result = MakeWhatsappMessageForSendInChat(message, _vonageService);
                        if (!string.IsNullOrWhiteSpace(result.TemplateMessage) &&
                            !string.IsNullOrWhiteSpace(message.Phone) &&
                            !string.IsNullOrWhiteSpace(message.CustomerWhatsAppNumber) &&
                            !string.IsNullOrWhiteSpace(message.APPId) &&
                            !string.IsNullOrWhiteSpace(message.PrivateKey))
                        {
                            if (IsValidPhoneNumber(message.Phone) && IsValidPhoneNumber(message.CustomerWhatsAppNumber))
                            {
                                await _vonageService.SendSMS(message.APPId, message.PrivateKey, message.CustomerWhatsAppNumber, message.Phone, result.TemplateMessage);

                            }
                        }
                    }
                    if (message.EligibleForEmailCommunication == "1")
                    {
                        if (!string.IsNullOrWhiteSpace(message.Email))
                        {
                            var isSend = SendMailToGuest(message, _mail, cancellationToken);
                        }
                    }
                    if (message.EligibleForWebChatCommunication == "1")
                    {
                        await SendTemplateInWebChat(message, _vonageService, _db, (int)MessageSourceEnum.WebChat).ConfigureAwait(true);
                    }

                    Console.WriteLine($" [x] Received {message.ActionName}");
                    break;
            }
        }
    }
    static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return false;
        }
        string pattern = @"^\+?\d{1,}$";

        Regex regex = new Regex(pattern);
        Match match = regex.Match(phoneNumber);
        return match.Success;
    }
}
