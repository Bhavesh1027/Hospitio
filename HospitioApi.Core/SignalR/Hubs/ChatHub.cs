using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Chat.Models;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Chat.Models.QuestionAnswers;
using HospitioApi.Core.Services.Chat.Models.Tickets;
using HospitioApi.Core.Services.LanguageTranslator;
using HospitioApi.Core.Services.LanguageTranslator.Models;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;
using Vonage;
using Vonage.Messages;
using Vonage.Messages.Sms;
using Vonage.Messages.WhatsApp;
using Vonage.Request;

namespace HospitioApi.Core.SignalR.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public static Dictionary<string, List<string>> ConnectedUsers = new();
        private readonly IChatService _chatService;
        private readonly ApplicationDbContext _db;
        private readonly VonageSettingsOptions _vonage;
        private readonly ILanguageTranslatorService _languageTranslatorService;
        private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;

        public ChatHub(IChatService chatService, ApplicationDbContext db, IOptions<VonageSettingsOptions> vonage, ILanguageTranslatorService languageTranslatorService, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount)
        {
            _vonage = vonage.Value;
            _db = db;
            _chatService = chatService;
            _languageTranslatorService = languageTranslatorService;
            _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
        }

        public Task BroadcastMessage(string name, string message) =>
            Clients.All.SendAsync("broadcastMessage", name, message);

        public Task Echo(string name, string message) =>
            Clients.Client(Context.ConnectionId)
                   .SendAsync("echo", name, $"{message} (echo from server)");

        public override async Task OnConnectedAsync()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            string permission = Context.User.FindFirstValue("permission");

            if (userId != null)
            {
                // Add the user name to the SignalR Context
                Context.Items["userId"] = userId;

                Console.WriteLine($"{Context.ConnectionId} connected");
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            if (!string.IsNullOrEmpty(userId))
            {
                // Remove user ID from the Context
                Context.Items.Remove(userId);

                // update user chat status to active
                await _chatService.UpdateUserChatStatus(_db, int.Parse(userId), int.Parse(userType), false);

                var connectedUsers = await _chatService.GetConnectedUsers(_db, int.Parse(userId), int.Parse(userType));
                var statusChangeResponse = new { UserId = userId, Status = false, UserType = ((ChatUserTypeEnum)int.Parse(userType)).ToString(), ChatId = 0 };
                foreach (var connectedUser in connectedUsers)
                {
                    var dynamicResponse = new
                    {
                        UserId = statusChangeResponse.UserId,
                        Status = statusChangeResponse.Status,
                        UserType = statusChangeResponse.UserType,
                        ChatId = connectedUser.ChatId
                    };
                    await Clients.Group($"user-{connectedUser.UserType}-{connectedUser.UserId}").SendAsync("ActiveStatusChange", dynamicResponse);
                }
                Console.WriteLine($"{Context.ConnectionId} disconnected");
            }
            await base.OnDisconnectedAsync(e);
        }
        public async Task<string> JoinGroup()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            string groupName = $"user-{userType}-{userId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // update user chat status to active
            var isUpdated = await _chatService.UpdateUserChatStatus(_db, int.Parse(userId), int.Parse(userType), true);

            var connectedUsers = await _chatService.GetConnectedUsers(_db, int.Parse(userId), int.Parse(userType));
            var statusChangeResponse = new { UserId = userId, Status = true, UserType = ((ChatUserTypeEnum)int.Parse(userType)).ToString(), ChatId = 0 };
            foreach (var connectedUser in connectedUsers)
            {
                var dynamicResponse = new
                {
                    UserId = statusChangeResponse.UserId,
                    Status = statusChangeResponse.Status,
                    UserType = statusChangeResponse.UserType,
                    ChatId = connectedUser.ChatId
                };
                await Clients.Group($"user-{connectedUser.UserType}-{connectedUser.UserId}").SendAsync("ActiveStatusChange", dynamicResponse);
            }

            /*await Clients.Caller.SendAsync("JoinGroupSuccessful", userId);*/
            return userId;
        }

        public async Task<string> LeaveGroup()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            string groupName = $"user-{userType}-{userId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);


            // update user chat status to inactive
            var isUpdated = await _chatService.UpdateUserChatStatus(_db, int.Parse(userId), int.Parse(userType), false);

            // update user deactivated
            await _chatService.UpdateUserDeActivate(_db, int.Parse(userId), int.Parse(userType));

            var connectedUsers = await _chatService.GetConnectedUsers(_db, int.Parse(userId), int.Parse(userType));
            var statusChangeResponse = new { UserId = userId, Status = false, UserType = ((ChatUserTypeEnum)int.Parse(userType)).ToString(), ChatId = 0 };
            foreach (var connectedUser in connectedUsers)
            {
                var dynamicResponse = new
                {
                    UserId = statusChangeResponse.UserId,
                    Status = statusChangeResponse.Status,
                    UserType = statusChangeResponse.UserType,
                    ChatId = connectedUser.ChatId
                };
                await Clients.Group($"user-{connectedUser.UserType}-{connectedUser.UserId}").SendAsync("ActiveStatusChange", dynamicResponse);
            }
            /*await Clients.Caller.SendAsync("LeaveGroupSuccessful", userId);*/
            return userId;
        }

        public async Task<int> CreateChat(object obj)
        {
            var chat = 0;
            if (obj != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(obj.ToString());
                string receiverId = jsonObject.receiverId;
                string chatType = jsonObject.chatType;

                var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
                var userId = _context.HttpContext.User.UserId();
                var userType = _context.HttpContext.User.UserType();
                chat = await _chatService.CreateChat(_db, userId, userType, int.Parse(receiverId), chatType);
            }
            return chat;
        }

        public async Task<List<Channels>> ChatList(int page, int limit, bool isDeleted)
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string userId = _context.HttpContext.User.UserId();
            string userType = _context.HttpContext.User.UserType();

            var chatList = await _chatService.GetChatList(_db, userId, page, limit, isDeleted);

            return chatList;
        }
        public async Task<ChatListResponse> ChatListDapper(object objChatList)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objChatList.ToString());
            int page = jsonObject.page;
            int limit = jsonObject.limit;
            bool isDeleted = jsonObject.isDeleted;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string userId = _context.HttpContext.User.UserId();
            string userType = _context.HttpContext.User.UserType();

            var chatListDapper = await _chatService.GetChatListDapper(_db, int.Parse(userId), page, limit, isDeleted, int.Parse(userType));

            return chatListDapper;
        }

        public async Task<ChatListCustomerResponse> ChatListCustomer(object objChatListCustomer)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objChatListCustomer.ToString());
            int page = jsonObject.page;
            int limit = jsonObject.limit;
            string chatType = jsonObject.chatType;
            string filter = string.Join(",", jsonObject.filter);

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string userId = _context.HttpContext.User.UserId();
            string userType = _context.HttpContext.User.UserType();

            var chatListCustomer = await _chatService.GetChatListCustomer(_db, userId, userType, page, limit, chatType, filter);

            return chatListCustomer;
        }

        public async Task<List<ChatMessageList>> ChatMessageList(object objChatMessageList)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objChatMessageList.ToString());
            int chatId = jsonObject.chatId;
            int page = jsonObject.page;
            int limit = jsonObject.limit;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string userId = _context.HttpContext.User.UserId();
            string userType = _context.HttpContext.User.UserType();

            var chatMessageList = await _chatService.GetChatMessageList(_db, chatId, page, limit, userId, userType);

            return chatMessageList;
        }

        public async Task SendMessage(object objSendMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objSendMessage.ToString());
            int chatId = jsonObject.chatId;
            string message = jsonObject.message;
            byte source = jsonObject.source;
            string messagetype = jsonObject.messagetype;
            int? requestId = jsonObject.requestId;
            string? attachment = jsonObject.attachment;
            string? url = jsonObject.url;
            Uri? temporaryUrl = null;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;

            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();
            string? customerId = null;
            if (userType != "1")
                customerId = _context.HttpContext.User.CustomerId();

            //var lastMessage = await _db.ChannelMessages.Where(e => e.ChannelId == chatId).LastAsync(CancellationToken.None);
            //var sourceName = lastMessage.Source;
            if (source == (int)MessageSourceEnum.whatsapp)
            {
                var lastMessage = await _db.ChannelMessages
                               .Where(e => e.ChannelId == chatId && e.Source == (byte)MessageSourceEnum.whatsapp &&
                                     ((e.MessageSender == byte.Parse(userType) && e.MessageSenderId != int.Parse(userId)) ||
                                     (e.MessageSender != byte.Parse(userType))))
                               .OrderBy(e => e.Id).LastAsync();
                DateTime lastMessageTime = lastMessage.CreatedAt ?? DateTime.MinValue;

                if (url != null)
                {
                    string storageConnectionString = _hospitioApiStorageAccount.ConnectionStringKey;
                    // Blob container name
                    string containerName = _hospitioApiStorageAccount.UserFilesContainerName;
                    // Expiration time for the temporary URL
                    TimeSpan expirationTime = TimeSpan.FromDays(365.25 * 100); // Adjust as needed

                    // Create a BlobServiceClient using the connection string
                    var blobServiceClient = new BlobServiceClient(storageConnectionString);
                    // Get a reference to the container
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    // Get a reference to the blob (file)
                    var blobClient = containerClient.GetBlobClient(url);

                    // Generate a temporary URL with read access that expires
                    temporaryUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationTime));
                }

                // Sender and Receiver Details Get
                string senderPhoneNumber = string.Empty;
                string AppId = string.Empty;
                string AppPrivatKey = string.Empty;

                if (userType == ((int)ChatUserTypeEnum.HospitioUser).ToString())
                {
                    var senderDetails = await _chatService.SendWhatsAppMessage(_db, message, chatId, userId, userType, "[dbo].[SP_GetSenderUserForWP]");
                    senderPhoneNumber = senderDetails!.Phonenumber!;
                    AppId = _vonage.AppId;
                    AppPrivatKey = _vonage.AppPrivatKey;
                }
                else if (userType == ((int)ChatUserTypeEnum.CustomerUser).ToString())
                {
                    var senderDetails = await _chatService.SendWhatsAppMessage(_db, message, chatId, userId, userType, "[dbo].[SP_GetSenderUserForWP]");
                    senderPhoneNumber = senderDetails!.Phonenumber!;
                    AppId = senderDetails.AppId!;
                    AppPrivatKey = senderDetails.AppPrivatKey!;
                }

                var receiver = await _chatService.SendWhatsAppMessage(_db, message, chatId, userId, userType, "[dbo].[SP_GetReceiverUserForWP]");

                receiver.Phonenumber = receiver.Phonenumber!.Substring(1);
                senderPhoneNumber = senderPhoneNumber.Substring(1);
                DateTime currentTime = DateTime.Now;

                TimeSpan timeSinceLastMessage = currentTime - lastMessageTime;

                // Define the 24-hour window duration
                TimeSpan twentyFourHours = TimeSpan.FromHours(24);

                // Check if the time since the last message is within the 24-hour window
                if (timeSinceLastMessage <= twentyFourHours)
                {
                    Console.WriteLine("The 24-hour window is still open.");
                    source = (int)MessageSourceEnum.whatsapp;
                    var credentials = Credentials.FromAppIdAndPrivateKey(AppId, AppPrivatKey);
                    var client = new VonageClient(credentials);

                    dynamic request = null;

                    switch (messagetype)
                    {
                        case "Text":
                            request = new WhatsAppTextRequest
                            {
                                To = receiver!.Phonenumber,
                                From = senderPhoneNumber,
                                Text = message
                            };
                            break;
                        case "Image":
                            request = new WhatsAppImageRequest
                            {
                                To = receiver!.Phonenumber,
                                From = senderPhoneNumber,
                                Image = new CaptionedAttachment
                                {
                                    Caption = attachment,
                                    Url = temporaryUrl.ToString()
                                }
                            };
                            break;
                        case "File":
                        case "Pdf":
                            request = new WhatsAppFileRequest
                            {
                                To = receiver!.Phonenumber,
                                From = senderPhoneNumber,
                                File = new CaptionedAttachment
                                {
                                    Caption = attachment,
                                    Url = temporaryUrl.ToString()
                                }
                            };
                            break;
                        case "Video":
                            request = new WhatsAppVideoRequest
                            {
                                To = receiver!.Phonenumber,
                                From = senderPhoneNumber,
                                Video = new CaptionedAttachment
                                {
                                    Caption = attachment,
                                    Url = temporaryUrl.ToString()
                                }
                            };
                            break;
                        case "Audio":
                            request = new WhatsAppAudioRequest
                            {
                                To = receiver!.Phonenumber,
                                From = senderPhoneNumber,
                                Audio = new Attachment
                                {
                                    Url = temporaryUrl.ToString()
                                }
                            };
                            break;
                        default:
                            Console.WriteLine("This message type are not allowed");
                            break;
                    }
                    //if (request)
                    //{
                        var response = await client.MessagesClient.SendAsync(request);
                    //}
                }

                else
                {
                    Console.WriteLine("The 24-hour window has closed.");
                    source = (int)MessageSourceEnum.sms;
                    var credentials = Credentials.FromAppIdAndPrivateKey(AppId, AppPrivatKey);
                    var request = new SmsRequest
                    {
                        To = receiver!.Phonenumber,
                        From = senderPhoneNumber,
                        Text = message
                    };
                    //source = (byte)CommunicationPlatFromEnum.Whatsapp;
                    var client = new VonageClient(credentials);
                    var response = await client.MessagesClient.SendAsync(request);
                }

            }
            var chat = await _chatService.SendMessage(_db, chatId, userId, userType, message, source, messagetype, attachment, requestId, url, null, 1, null);
            //var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(_db, userId,int.Parse(userType));
            var receiverData = await _chatService.GetReceiverData(_db, chatId, int.Parse(userId), ((ChatUserTypeEnum)int.Parse(userType)).ToString());

            int messageType = (int)(ChatUserTypeEnum)Enum.Parse(typeof(ChatUserTypeEnum), receiverData.UserType);

            var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverData.UserId.ToString(), messageType);
            await Clients.Group($"user-{userType}-{userId}").SendAsync("GetNewMessage", chat);

            if(source != (int)MessageSourceEnum.whatsapp)
            {
                await Clients.Group($"user-{messageType}-{receiverData.UserId}").SendAsync("GetNewMessage", chat);
            }
            //await Clients.Group($"user-{messageType}-{receiverData.UserId}").SendAsync("GetNewMessage", chat);

            var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

            #region automessagepart

            if (chat.UserType == "CustomerGuest" && receiverData.UserType == "CustomerUser")
            {
                List<CustomerDigitalAssistant> assistanceData = LoadAssistanceDataForCustomer(customerId);
                string autoReply = ProcessIncomingMessage(message, assistanceData);

                if (!string.IsNullOrWhiteSpace(autoReply))
                {
                    var autochat = await _chatService.SendMessage(_db, chatId, receiverData.UserId.ToString(), "2", autoReply, source, messagetype, attachment, requestId, url, null, 1, null);
                    var autoreceiverData = await _chatService.GetReceiverData(_db, chatId, int.Parse(receiverData.UserId.ToString()), ((ChatUserTypeEnum)int.Parse("2")).ToString());

                    int automessageType = (int)(ChatUserTypeEnum)Enum.Parse(typeof(ChatUserTypeEnum), autochat.UserType);

                    var autototalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(autoreceiverData.UserId.ToString(), messageType);
                    await Clients.Group($"user-{userType}-{userId}").SendAsync("GetNewMessage", autochat);
                    await Clients.Group($"user-{messageType}-{receiverData.UserId}").SendAsync("GetNewMessage", autochat);
                }
            }
            #endregion

            await Clients.Group($"user-{messageType}-{receiverData.UserId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        }

        private string ProcessIncomingMessage(string message, List<CustomerDigitalAssistant> assistanceData)
        {
            string normalizedQuery = message.ToLower().Replace("-", "").Replace("/", " ").Replace("?", "");

            // Iterate through digital assistant responses
            foreach (var assistanceItem in assistanceData)
            {
                string[] wordsInResponseName = assistanceItem.Name.ToLower().Replace("-", "").Replace("/", " ").Split(' ');

                // Remove empty strings from the array
                wordsInResponseName = wordsInResponseName.Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();

                // Check if the response name is present as a whole phrase in the query
                if (wordsInResponseName.Any(word => normalizedQuery.Contains(word)))
                {
                    return assistanceItem.Details;
                }
            }

            return "";
        }

        private List<CustomerDigitalAssistant> LoadAssistanceDataForCustomer(string customerId)
        {
            return _db.CustomerDigitalAssistants.Where(x => x.CustomerId == Int32.Parse(customerId) && x.IsActive == true).ToList();
        }

        public async Task<ChatList> ReadChatMessage(object objReadChatMessage)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objReadChatMessage.ToString());
            int chatId = jsonObject.chatId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            var markChatRead = await _chatService.MarkChatMessageAsRead(_db, chatId, int.Parse(userId), int.Parse(userType));

            var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(userId, int.Parse(userType));
            var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };
            await Clients.Group($"user-{userType}-{userId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
            return markChatRead;
        }

        public async Task<int> DeleteChat(object objDeleteChat)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objDeleteChat.ToString());
            int chatId = jsonObject.chatId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string userId = _context.HttpContext.User.UserId();

            await _chatService.DeleteChat(_db, chatId, int.Parse(userId));

            return chatId;
        }
        public async Task<int> GetTotalUnreadMessageCount()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(userId, int.Parse(userType));
            return totalUnreadMessageCount;
        }
        public async Task<UserDetail> GetUserDetailByChatId(object objUserDetail)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objUserDetail.ToString());
            int chatId = jsonObject.chatId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            var userDetail = await _chatService.GetUserDetailByChatId(_db, int.Parse(userId), ((ChatUserTypeEnum)int.Parse(userType)).ToString(), chatId);
            return userDetail;
        }
        public async Task<UserChat> GetUserChatListByChatId(object objUserChat)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objUserChat.ToString());
            int chatId = jsonObject.chatId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            var userDetail = await _chatService.GetUserChatListByChatId(_db, int.Parse(userId), ((ChatUserTypeEnum)int.Parse(userType)).ToString(), chatId);
            return userDetail;
        }
        public async Task<CustomerGuestDetail> GetCustomerGuestDetailByChatId(object objCustGuestDetail)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objCustGuestDetail.ToString());
            int chatId = jsonObject.chatId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();

            var custGuestDetail = await _chatService.GetCustomerGuestDetailByChatId(_db, int.Parse(userId), chatId);
            return custGuestDetail;
        }
        public async Task<GuestDetails> GetGuestChatDetailById(object objChatomerGuestId)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objChatomerGuestId.ToString());
            int guestId = jsonObject.GuestId;
            var guestDetail = await _chatService.GetGuestChatDetailById(_db, guestId);
            return guestDetail;
        }

        public async Task<ChannelId> GetChatId(object objChannelId)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objChannelId.ToString());
            int userId = jsonObject.userId;
            string userType = jsonObject.userType;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            string claimUserId = _context.HttpContext.User.UserId();
            string claimUserType = _context.HttpContext.User.UserType();

            var channelId = await _chatService.GetChatId(_db, userId, userType, int.Parse(claimUserId), claimUserType);

            return channelId;
        }
        public async Task<int> GetTicketTotalUnReadCountByTicketId(object objTicketId)
        {
            int totalTicketUnReadCount = 0;
            if (objTicketId != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(objTicketId.ToString());
                int ticketId = jsonObject.ticketId;

                var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
                var userId = _context.HttpContext.User.CustomerId();
                var userType = _context.HttpContext.User.UserType();

                totalTicketUnReadCount = await _chatService.GetTicketTotalUnReadCountByTicketId(int.Parse(userId), int.Parse(userType), ticketId);
            }
            return totalTicketUnReadCount;
        }
        public async Task<TicketMessageCount> UpdateAllTicketMessageRead(object objTicketId)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(objTicketId.ToString());
            int ticketId = jsonObject.ticketId;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();
            var customerId = "";
            if (userType == ((int)UserTypeEnum.Customer).ToString())
            {
                customerId = _context.HttpContext.User.CustomerId();
            }

            var updateMessageCount = await _chatService.UpdateAllTicketMessageRead(int.Parse(userId), int.Parse(userType), ticketId);

            var totalUnreadTicketCount = await _chatService.GetTotalUnreadTicketCount(userId, userType, customerId);
            var totalUnreadTicketCountResponse = new { Type = "Ticket", Id = ticketId, Count = totalUnreadTicketCount };

            await Clients.Group($"user-{userType}-{userId}").SendAsync("GetTotalUnreadCount", totalUnreadTicketCountResponse);
            return updateMessageCount;
        }
        public async Task<TicketDetail> GetTicketDetailByTicketId(object objTicketId)
        {
            TicketDetail ticketDetail = null;
            if (objTicketId != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(objTicketId.ToString());
                int ticketId = jsonObject.ticketId;

                var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
                var userId = _context.HttpContext.User.UserId();
                var userType = _context.HttpContext.User.UserType();

                ticketDetail = await _chatService.GetTicketDetailByTicketId(userId, userType, ticketId);
            }
            return ticketDetail;
        }

        public async Task<int> GetTotalUnreadTicketCount()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();
            var customerId = "";
            if (userType == ((int)UserTypeEnum.Customer).ToString())
            {
                customerId = _context.HttpContext.User.CustomerId();
            }

            var totalUnreadMessageCount = await _chatService.GetTotalUnreadTicketCount(userId, userType, customerId);
            return totalUnreadMessageCount;
        }

        public async Task<LanguageTranslate> GetTranslatedLanguage(object languageRequest)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(languageRequest.ToString());

            int channelMessageId = jsonObject.channelMessageId;
            string message = jsonObject.message;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;

            string customerId = string.Empty;
            int userType = Int32.Parse(_context.HttpContext.User.UserType());

            if (userType == (int)UserTypeEnum.Customer)
            {
                customerId = _context.HttpContext.User.CustomerId();
            }

            return await _languageTranslatorService.GetLanguageTranslatedAsync(_db,userType, customerId != string.Empty && customerId != null ? Int32.Parse(customerId) : 0, channelMessageId, message);
        }

        public async Task<LanguageTranslate> GetGuestTranslatedLanguage(object languageRequest)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(languageRequest.ToString());

            int channelMessageId = jsonObject.channelMessageId;
            string message = jsonObject.message;
            string translatelanguage = jsonObject.
        translatemessage;

            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;

            string UserId = string.Empty;
            int userType = Int32.Parse(_context.HttpContext.User.UserType());

            if (userType == (int)UserTypeEnum.Guest)
            {
                UserId = _context.HttpContext.User.UserId();
            }

            return await _languageTranslatorService.GetGuestTranslatedLanguage(_db,userType, UserId != string.Empty && UserId != null ? Int32.Parse(UserId) : 0, channelMessageId, message, translatelanguage);
        }

        public async Task<NotificationCount> UpdateNotificationRead()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();
            var customerId = _context.HttpContext.User.CustomerId();

            if (int.Parse(userType) == (int)UserTypeEnum.Guest)
            {
                var updateNotificationCount = await _chatService.UpdateAllNotificationRead(int.Parse(userId), int.Parse(userType));

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(userId, int.Parse(userType));
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Id = int.Parse(userId), Count = totalUnreadNotificationCount };
                await Clients.Group($"user-{userType}-{userId}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);

                return updateNotificationCount;
            }
            else
            {
                var updateNotificationCount = await _chatService.UpdateAllNotificationRead(int.Parse(customerId), int.Parse(userType));

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(userId, int.Parse(userType));
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Id = int.Parse(userId), Count = totalUnreadNotificationCount };
                await Clients.Group($"user-{userType}-{userId}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);

                return updateNotificationCount;
            };
        }

        public async Task<List<TotalUnreadCount>> GetTotalUnreadCount()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();
            var customerId = "";
            if (userType == ((int)UserTypeEnum.Customer).ToString())
            {
                customerId = _context.HttpContext.User.CustomerId();
            }

            if (int.Parse(userType) == (int)UserTypeEnum.Guest)
            {
               var totalUnreadCount = await _chatService.GetTotalUnreadCount(userId, userType, userId);
                return totalUnreadCount;
            }
            else if (int.Parse(userType) == (int)UserTypeEnum.ChatWidgetUser)
            {
                var totalUnreadCount = await _chatService.GetTotalUnreadCount(userId, userType);
                return totalUnreadCount;
            }
            else
            {
               var totalUnreadCount = await _chatService.GetTotalUnreadCount(userId, userType, customerId);
                return totalUnreadCount;
            }
        }

        public async Task<QaUnreadCount> UpdateAllQuestionAnswersRead()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var userId = _context.HttpContext.User.UserId();
            var userType = _context.HttpContext.User.UserType();

            var updateQuestionAnswerCount = await _chatService.UpdateAllQuestionAnswersRead(userId);

            return updateQuestionAnswerCount;
        }

        public async Task<UpdateGuestRequestResponse> UpdateGuestRequestStatus(object request)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(request.ToString());
            int requestId = jsonObject.requestId;
            int requestType = jsonObject.requestType;
            byte status = jsonObject.status;
            int id = jsonObject.id;

            if (requestType == 1)
            {
                var guestRequest = await _db.GuestRequests.Where(e => e.Id == requestId).FirstOrDefaultAsync();
                if (guestRequest != null)
                {
                    guestRequest.Status = status;
                    await _db.SaveChangesAsync(CancellationToken.None);

                }
            }
            else if (requestType == 2)
            {
                var guestRequest = await _db.EnhanceStayItemExtraGuestRequests.Where(e => e.Id == requestId).FirstOrDefaultAsync();
                if (guestRequest != null)
                {
                    guestRequest.Status = status;
                    await _db.SaveChangesAsync(CancellationToken.None);

                }
            }

            UpdateGuestRequestResponse responseData = new UpdateGuestRequestResponse()
            {
                Id = id,
                Status = status
            };
            return responseData;
        }
        public async Task<List<CustomerOnboardingStatus>> GetCustomerOnboardingStatus()
        {
            var _context = Context.Features.SingleOrDefault(f => f.Key == typeof(IHttpContextFeature)).Value as IHttpContextFeature;
            var  customerId = _context.HttpContext.User.CustomerId();

            var getCustomerOnboardingStatus = await _chatService.GetCustomerOnboardingStatus(_db, customerId);
            return getCustomerOnboardingStatus;
        }

    }
}


