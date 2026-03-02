using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;

namespace Hospitio.BackGroundService.Sender
{
    public class SendAlertMessageBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBackGroundServiceData _backGroundServiceData;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly BackGroundServicesSettingsOptions _time;
        private readonly VonageSettingsOptions _vonageSettingsOptions;
        public SendAlertMessageBackGroundService(IServiceProvider serviceProvider, IBackGroundServiceData backGroundServiceData, IChatService chatService, IHubContext<ChatHub> hubContext, IOptions<BackGroundServicesSettingsOptions> time, IOptions<VonageSettingsOptions> vonageSettingsOptions)
        {
            _serviceProvider = serviceProvider;
            _backGroundServiceData = backGroundServiceData;
            _chatService = chatService;
            _hubContext = hubContext;
            _time = time.Value;
            _vonageSettingsOptions = vonageSettingsOptions.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scopedb = _serviceProvider.CreateScope();
                    var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var _vonageService = scopedb.ServiceProvider.GetRequiredService<IVonageService>();
                    var alertMessages = await _backGroundServiceData.GetAlertMessages(DateTime.UtcNow, stoppingToken);
                    foreach (var alertMessage in alertMessages)
                    {
                        if ((alertMessage.Platform == (int)CommunicationPlatFromEnum.HospitioChat || alertMessage.Platform == null || alertMessage.Platform == 0) && alertMessage.MsgReqType != 2)
                        {
                            if (alertMessage.AlertType == "AdminStaffAlert" && alertMessage.ChatId == 0)
                            {
                                var channels = await _chatService.GetChannelDataFromUsersDetail(alertMessage.ReceiverId, "1", alertMessage.SenderId, "1");
                                if (channels != null)
                                {
                                    await SendMessage(channels.Id, alertMessage.SenderId.ToString(), alertMessage.SenderType.ToString(), alertMessage.ReceiverId.ToString(), alertMessage.ReceiverType.ToString(), alertMessage.AlertMessage, null, 1, "Text", null, null, null);
                                }
                                else
                                {
                                    var chatId = await _chatService.CreateChat(_db, alertMessage.SenderId.ToString(), alertMessage.ReceiverType.ToString(), alertMessage.ReceiverId, ((ChatUserTypeEnum)alertMessage.ReceiverType).ToString());
                                    await SendMessage(chatId, alertMessage.SenderId.ToString(), alertMessage.SenderType.ToString(), alertMessage.ReceiverId.ToString(), alertMessage.ReceiverType.ToString(), alertMessage.AlertMessage, null, 1, "Text", null, null, null);
                                }
                            }
                            else
                            {
                                var isSend = await SendMessage(alertMessage.ChatId, alertMessage.SenderId.ToString(), alertMessage.SenderType.ToString(), alertMessage.ReceiverId.ToString(), alertMessage.ReceiverType.ToString(), alertMessage.AlertMessage, null, 1, "Text", null, null, null);
                            }


                            //await SendNotification(alertMessage.ReceiverId.ToString(), (byte)alertMessage.ReceiverType, alertMessage.AlertMessage, _db, "Alert Message");

                        }
                        else if (alertMessage.Platform == (int)CommunicationPlatFromEnum.Whatsapp)
                        {
                            if (alertMessage.AlertType == "CustomerStaffAlert" && alertMessage.VonageAppId != null && alertMessage.VonagePrivateKey != null && alertMessage.FromPhoneNumber != null &&alertMessage.ToPhoneNumber != null && alertMessage.AlertMessage != null)
                            {
                                await _vonageService.SendWhatsappTextMessage(alertMessage.VonageAppId, alertMessage.VonagePrivateKey, alertMessage.FromPhoneNumber, alertMessage.ToPhoneNumber, alertMessage.AlertMessage);

                            }
                            else if (alertMessage.AlertType == "AdminStaffAlert" && alertMessage.FromPhoneNumber != null &&alertMessage.ToPhoneNumber != null && alertMessage.AlertMessage != null && alertMessage.WhatsappTemplateName != null && alertMessage.VonageTemplateStatus == "APPROVED")
                            {
                                await _vonageService.SendWhatsappTemplateMessage(_vonageSettingsOptions.AppId, _vonageSettingsOptions.AppPrivatKey, alertMessage.AlertMessage, alertMessage.ToPhoneNumber, alertMessage.FromPhoneNumber, alertMessage.WhatsappTemplateName, new List<string>(), false, new Dictionary<int, string>()); 
                            }
                        }
                        else if (alertMessage.Platform == (int)CommunicationPlatFromEnum.SMS)
                        {
                            if (alertMessage.AlertType == "CustomerStaffAlert" && alertMessage.VonageAppId != null && alertMessage.VonagePrivateKey != null && alertMessage.FromPhoneNumber != null &&alertMessage.ToPhoneNumber != null && alertMessage.AlertMessage != null)
                            {
                                await _vonageService.SendSMS(alertMessage.VonageAppId, alertMessage.VonagePrivateKey, alertMessage.FromPhoneNumber, alertMessage.ToPhoneNumber, alertMessage.AlertMessage);
                            }
                            else if (alertMessage.AlertType == "AdminStaffAlert" && alertMessage.FromPhoneNumber != null &&alertMessage.ToPhoneNumber != null && alertMessage.AlertMessage != null)
                            {
                                await _vonageService.SendSMS(_vonageSettingsOptions.AppId, _vonageSettingsOptions.AppPrivatKey, alertMessage.FromPhoneNumber, alertMessage.ToPhoneNumber, alertMessage.AlertMessage);
                            }
                        }
                        else if (alertMessage.Platform == (int)CommunicationPlatFromEnum.Email)
                        {
                            //send mail to staff with sender and receive mail with send 
                        }
                    }
                }
                catch (Exception e)
                {

                }
                await Task.Delay(TimeSpan.Parse(_time.SendAlertMessageBackGroundServiceTiming), stoppingToken);
            }
        }
        public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
        {
            using var scopedb = _serviceProvider.CreateScope();
            var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, (int)MsgReqTypeEnum.alertMessage, null);

            var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
            await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
            await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

            var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

            await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
            return 1;
        }
        public async Task<int> SendNotification(string receiverId, byte receiverUserType, string message, ApplicationDbContext _db, string Title)
        {
            CreateNotificationsIn createNotificationsIn = new CreateNotificationsIn();
            createNotificationsIn.Title = Title;
            createNotificationsIn.Message = message;

            var notification = new Notification()
            {
                Title = Title,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Notifications.AddAsync(notification, CancellationToken.None);
            await _db.SaveChangesAsync(CancellationToken.None);

            List<UserNotification> userNotifications = new List<UserNotification>();
            UserNotification userNotification = new UserNotification();
            userNotification.UserId = Convert.ToInt32(receiverId);
            userNotification.IsActive = 1;
            userNotifications.Add(userNotification);

            if (receiverUserType == 2)
            {
                NotificationHistory notificationHistory1 = new NotificationHistory();
                var customerUser = await _db.CustomerUsers.Where(e => e.Id == Convert.ToInt32(receiverId)).FirstOrDefaultAsync(CancellationToken.None);
                notificationHistory1.UserId = customerUser.CustomerId;
                notificationHistory1.NotificationId = notification.Id;
                notificationHistory1.IsActive = true;
                notificationHistory1.UserType = receiverUserType;

                await _db.NotificationHistorys.AddAsync(notificationHistory1, CancellationToken.None);
                await _db.SaveChangesAsync(CancellationToken.None);
                notification.CreatedAt = Convert.ToDateTime(Convert.ToDateTime(notification.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                var notifyobj = new
                {
                    Message = notification.Message,
                    Title = notification.Title,
                    CreatedAt = notification.CreatedAt,
                    Id = notification.Id
                };

                await _hubContext.Clients.Group($"user-2-{notificationHistory1.UserId}").SendAsync("GetNewNotification", notifyobj);

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(receiverId, Convert.ToInt32(receiverUserType));
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Count = totalUnreadNotificationCount };

                await _hubContext.Clients.Group($"user-2-{notificationHistory1.UserId}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);
            }
            else if (receiverUserType == 3)
            {
                NotificationHistory notificationHistory1 = new NotificationHistory();
                var customerGuest = await _db.CustomerGuests.Where(e => e.Id == Convert.ToInt32(receiverId)).FirstOrDefaultAsync(CancellationToken.None);
                notificationHistory1.UserId = customerGuest.Id;
                notificationHistory1.NotificationId = notification.Id;
                notificationHistory1.IsActive = true;
                notificationHistory1.UserType = receiverUserType;

                await _db.NotificationHistorys.AddAsync(notificationHistory1, CancellationToken.None);
                await _db.SaveChangesAsync(CancellationToken.None);
                notification.CreatedAt = Convert.ToDateTime(Convert.ToDateTime(notification.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                var notifyobj = new
                {
                    Message = notification.Message,
                    Title = notification.Title,
                    CreatedAt = notification.CreatedAt,
                    Id = notification.Id
                };


                await _hubContext.Clients.Group($"user-3-{customerGuest.Id}").SendAsync("GetNewNotification", notifyobj);

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(receiverId, (int)UserTypeEnum.Guest);
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Count = totalUnreadNotificationCount };

                await _hubContext.Clients.Group($"user-3-{customerGuest.Id}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);
            }

            return 1;
        }
    }
}
