using Dapper;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;
using HospitioApi.Core.Services.Chat.Models;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Chat.Models.QuestionAnswers;
using HospitioApi.Core.Services.Chat.Models.Tickets;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.LanguageTranslator;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IDapperRepository _dapper;
        private readonly ILanguageTranslatorService _languageTranslatorService;

        public ChatService(IDapperRepository dapper
            , ILanguageTranslatorService languageTranslatorService
            )
        {
            _dapper = dapper;
            _languageTranslatorService = languageTranslatorService;

        }

    public async Task<int> CreateChat(ApplicationDbContext _db, string senderId, string userType, int receiverId, string chatType)
        {
            //add channel based on create from
            Channels chat = new Channels();
            if (userType == ((int)UserTypeEnum.Hospitio).ToString())
            {
                chat = new ChannelUserHospitioUser
                {
                    Uuid = Guid.NewGuid().ToString(),
                    channelUserID = int.Parse(senderId),
                    IsActive = true,
                };
            } else if (userType == ((int)UserTypeEnum.Customer).ToString())
            {
                chat = new ChannelUserCustomerUser
                {
                    Uuid = Guid.NewGuid().ToString(),
                    channelUserID = int.Parse(senderId),
                    IsActive = true,
                };
            } else if(userType == ((int)UserTypeEnum.Guest).ToString())
            {
                chat = new ChannelUserCustomerGuest
                {
                    Uuid = Guid.NewGuid().ToString(),
                    channelUserID = int.Parse(senderId),
                    IsActive = true,
                };
            }
            else if (userType == ((int)UserTypeEnum.AnonymousUser).ToString())
            {
                chat = new ChannelUserAnonymousUser
                {
                    Uuid = Guid.NewGuid().ToString(),
                    channelUserID = int.Parse(senderId),
                    IsActive = true,
                };
            }
            else if (userType == ((int)UserTypeEnum.ChatWidgetUser).ToString())
            {
                chat = new ChannelUserChatWidgetUser
                {
                    Uuid = Guid.NewGuid().ToString(),
                    channelUserID = int.Parse(senderId),
                    IsActive = true,
                };
            }

            _db.Channels.Add(chat);
            await _db.SaveChangesAsync(CancellationToken.None);

            //add sender to ChannelUsers table
            ChannelUsers senderUser = new ChannelUsers();
            if (userType == ((int)UserTypeEnum.Hospitio).ToString())
            {
                senderUser = new ChannelUserTypeUser()
                {
                    ChannelId = chat.Id,
                    UserId = int.Parse(senderId),
                    IsActive = true,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()
                };
            }
            else if (userType == ((int)UserTypeEnum.Customer).ToString())
            {
                senderUser = new ChannelUserTypeCustomerUser()
                {
                    ChannelId = chat.Id,
                    UserId = int.Parse(senderId),
                    IsActive = true,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()
                };
            }
            else if (userType == ((int)UserTypeEnum.Guest).ToString())
            {
                senderUser = new ChannelUserTypeCustomerGuest()
                {
                    ChannelId = chat.Id,
                    UserId = int.Parse(senderId),
                    IsActive = true,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()
                };
            }
            else if (userType == ((int)UserTypeEnum.AnonymousUser).ToString())
            {
                senderUser = new ChannelUserTypeAnonymousUser()
                {
                    ChannelId = chat.Id,
                    UserId = int.Parse(senderId),
                    IsActive = true,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()
                };
            }
            else if (userType == ((int)UserTypeEnum.ChatWidgetUser).ToString())
            {
                senderUser = new ChannelUserTypeChatWidgetUser()
                {
                    ChannelId = chat.Id,
                    UserId = int.Parse(senderId),
                    IsActive = true,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()
                };
            }

            _db.ChannelUsers.Add(senderUser);

            //add receiver to ChannelUsers table
            ChannelUsers receiverUser = new ChannelUsers ();
            if (chatType == "HospitioUser")
            {
                receiverUser = new ChannelUserTypeUser()
                {
                    ChannelId = chat.Id,
                    UserId = receiverId,
                    IsActive = false,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()

                };
            }
            else if (chatType == "CustomerUser")
            {
                receiverUser = new ChannelUserTypeCustomerUser()
                {
                    ChannelId = chat.Id,
                    UserId = receiverId,
                    IsActive = false,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()

                };
            }
            else if (chatType == "CustomerGuest")
            {
                receiverUser = new ChannelUserTypeCustomerGuest()
                {
                    ChannelId = chat.Id,
                    UserId = receiverId,
                    IsActive = false,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()

                };
            }
            else if (chatType == "AnonymousUser")
            {
                receiverUser = new ChannelUserTypeAnonymousUser()
                {
                    ChannelId = chat.Id,
                    UserId = receiverId,
                    IsActive = false,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()

                };
            }
            else if (chatType == "ChatWidgetUser")
            {
                receiverUser = new ChannelUserTypeChatWidgetUser()
                {
                    ChannelId = chat.Id,
                    UserId = receiverId,
                    IsActive = false,
                    LastMessageReadTime = DateTime.Now.ToUniversalTime()

                };
            }
            _db.ChannelUsers.Add(receiverUser);
            await _db.SaveChangesAsync(CancellationToken.None);
            return chat.Id;
        }

        public async Task<List<Channels>> GetChatList(ApplicationDbContext _db, string userId, int page, int limit, bool isDeleted, string chatType = null, List<string> filters = null)
        {
            // Return the list of chats based on the provided parameters
            var chats = await _db.Channels
            .Where(c => c.channelUserID == int.Parse(userId))
            .Where(c => isDeleted == true ? c.DeletedAt != null : c.DeletedAt == null)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

            return chats;
        }

        public async Task<ChatListResponse> GetChatListDapper(ApplicationDbContext _db, int userId, int page, int limit, bool isDeleted, int userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("PageNo", page, DbType.Int32);
            spParams.Add("PageSize", limit, DbType.Int32);
            spParams.Add("IsDeleted", isDeleted, DbType.Boolean);
            spParams.Add("UserType", ((ChatUserTypeEnum)userType).ToString());

            var chats = await _dapper
            .GetAllJsonData<ChatListResponse>("[dbo].[SP_GetChatList]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return chats[0];
        }

        public async Task<ChatListCustomerResponse> GetChatListCustomer(ApplicationDbContext _db, string userId, string userType, int page, int limit, string chatType = null, string filters = null)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", ((ChatUserTypeEnum)int.Parse(userType)).ToString());
            spParams.Add("PageNo", page, DbType.Int32);
            spParams.Add("PageSize", limit, DbType.Int32);
            spParams.Add("ChatType", chatType);
            spParams.Add("Filter", filters);

            //Console.WriteLine("UserId: ", userId);
            //Console.WriteLine("UserType: ", ((ChatUserTypeEnum)int.Parse(userType)).ToString());
            //Console.WriteLine("PageNo: ", page);
            //Console.WriteLine("PageSize: ", limit);
            //Console.WriteLine("PageNo: ", chatType);
            //Console.WriteLine("Filter: ", filters);

            var chats = await _dapper
            .GetAllJsonData<ChatListCustomerResponse>("[dbo].[SP_GetChatListCustomer]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return chats[0];
        }

        public async Task<List<ChatMessageList>> GetChatMessageList(ApplicationDbContext _db, int chatId, int page, int limit, string userId, string userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("PageNo", page, DbType.Int32);
            spParams.Add("PageSize", limit, DbType.Int32);
            spParams.Add("UserType", ((ChatUserTypeEnum)int.Parse(userType)).ToString());
            spParams.Add("UserId" , int.Parse(userId), DbType.Int32);

            var chatsMessageList = await _dapper
           .GetAllJsonData<ChatMessageList>("[dbo].[SP_GetChatMessageList]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return chatsMessageList;
        }

        public async Task<ChatMessageList> SendMessage(ApplicationDbContext _db, int chatId, string senderId, string userType, string message, byte source, string type, string attachment, int? requestId, string url,string message_uuid,byte messageRequestType,byte? requestType,int? EnhanceRequestId)
        {
            //using var scopedb = _scopeFactory.CreateScope();
            //var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            string translatedMessahge = "";

            if (type == "Text")
            {
                var customerGuest = await _db.ChannelUserTypeCustomerGuest.Where(x => x.ChannelId == chatId).FirstOrDefaultAsync(CancellationToken.None);
                if (customerGuest != null)
                {
                    var translatedLanaguage = await _languageTranslatorService.GetGuestTranslatedLanguage(_db, (int)UserTypeEnum.Guest, customerGuest.UserId??0, 0, message, "en");
                    translatedMessahge = translatedLanaguage.message;
                }
            }
            type = char.ToUpper(type[0]) + type.Substring(1).ToLower();
            ChannelMessages objMsg = new ChannelMessages();
            if (type == "Text")
            {
                objMsg = new TextMessage()
                {
                    ChannelId = chatId,
                    MessageType = type,
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Message = message,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId,
                    TranslateMessage = translatedMessahge
                };
            }
            else if (type == "Audio")
            {
                objMsg = new AudioMessage()
                {
                    ChannelId = chatId,
                    MessageType = type,
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Url = url,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId
                };
            }
            else if (type == "Video")
            {
                objMsg = new VideoMessage()
                {
                    ChannelId = chatId,
                    MessageType = type,
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Url = url,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId
                };
            }
            else if (type == "Image")
            {
                objMsg = new ImageMessage()
                {
                    ChannelId = chatId,
                    MessageType = type,
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Url = url,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId
                };
            }
            else if (type == "Pdf")
            {
                objMsg = new PdfMessage()
                {
                    ChannelId = chatId,
                    MessageType = type,
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Url = url,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId
                };
            }
            else if (type == "Template")
            {
                objMsg = new TemplateMessage()
                {
                    ChannelId = chatId,
                    MessageType = "Text",
                    MessageSender = byte.Parse(userType),
                    Source = source,
                    MsgReqType = messageRequestType,
                    Attachment = attachment,
                    RequestId = requestId,
                    MessageSenderId = int.Parse(senderId),
                    Message = message,
                    Url = url,
                    MessageUuid = message_uuid,
                    RequestType = requestType,
                    EnhanceStayItemsGuestRequestId = EnhanceRequestId
                };
            }
            _db.ChannelMessages.Add(objMsg);
            await _db.SaveChangesAsync(CancellationToken.None);

            //var spParams = new DynamicParameters();
            //spParams.Add("ChatId", chatId, DbType.Int32);
            //spParams.Add("UserId", senderId, DbType.Int32);
            //spParams.Add("UserType", userType, DbType.Int32);

            // var receiverData = await _dapper
            //.GetSingle<ReceiverData>("[dbo].[SP_GetReceiverUser]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            //if(receiverData != null)
            //{
            //    UpdateLastMessage(_db, receiverData.UserId, receiverData.UserType, chatId, objMsg.Id,DateTime.Now);
            //}
            byte? requeststatus = null;
            if (objMsg.RequestId != null)
            {
                if (requestType == 1)
                {
                    var guestRequest = await _db.GuestRequests.Where(e => e.Id == requestId).FirstOrDefaultAsync();
                    if (guestRequest != null)
                    {
                        requeststatus = (byte)(GuestRequestStatusEnum)guestRequest.Status!;
                    }
                }
                else if (requestType == 2)
                {
                    var guestRequest = await _db.EnhanceStayItemExtraGuestRequests.Where(e => e.Id == requestId).FirstOrDefaultAsync();
                    if (guestRequest != null)
                    {
                        requeststatus = (byte)(GuestRequestStatusEnum)guestRequest.Status!;
                    }
                }
            }

            ChatMessageList objOut = new ChatMessageList
            {
                Id = objMsg.Id,
                ChannelId = objMsg.ChannelId,
                MessageType = objMsg.MessageType,
                MessageSender = objMsg.MessageSender,
                Source = objMsg.Source,
                MsgReqType = objMsg.MsgReqType,
                Attachment = objMsg.Attachment,
                RequestId = objMsg.RequestId,   
                Url = url,
                Message = message,
                TranslateMessage = translatedMessahge??null,
                IsActive = objMsg.IsActive,
                CreatedAt = Convert.ToDateTime(Convert.ToDateTime(objMsg.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                IsRead = objMsg.IsRead,
                MessageSenderId = objMsg.MessageSenderId,
                UserType = ((ChatUserTypeEnum)int.Parse(userType)).ToString(),
                RequestType = objMsg.RequestType,
                RequestStatus = requeststatus
            };

            return objOut;
        }

        public async Task<ChatList> MarkChatMessageAsRead(ApplicationDbContext _db, int chatId, int userId, int userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType, DbType.Int32);

            var chatsMessage = await _dapper
           .GetSingle<ChatList>("[dbo].[SP_ReadChatMessage]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return chatsMessage;
        }

        public async Task<int> DeleteChat(ApplicationDbContext _db, int chatId, int userId)
        {
            // Implement deleting a chat logic (e.g., marking the chat as deleted in the database)
            var chat = await _db.Channels.FirstOrDefaultAsync(c => c.Id == chatId && (c.channelUserID == userId));
            var Id = 0;
            if (chat != null)
            {
                chat.DeletedAt = DateTime.Now;
                _db.SaveChangesAsync(CancellationToken.None);
                Id = chat.Id;
            }
            return Id;
        }

        public async Task<int> GetTotalUnreadMessageCount(string userId,int userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType, DbType.Int32);
            spParams.Add("ChatUserType", ((ChatUserTypeEnum)userType).ToString());

            var totalCount = await _dapper.GetSingle<int>("[dbo].[SP_GetTotalUnReadMessageCount]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return totalCount;
        }

        public async Task<int> GetTotalUnreadMessageCountPerChat(ApplicationDbContext _db, string userId,string userType, int chatId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType, DbType.Int32);
            spParams.Add("ChatId", chatId);

            var totalCount = await _dapper.GetSingle<int>("[dbo].[SP_GetTotalUnReadMessageCountPerChat]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return totalCount;
        }

        public async Task<bool> UpdateUserChatStatus(ApplicationDbContext _db, int userId, int userType, bool status)
        {
            List<ChannelUsers> users = null;
            bool isUpdated = false;
            if (userType == ((int)UserTypeEnum.Hospitio))
            {
                users = await _db.ChannelUserTypeUser.Where(i => i.UserId == userId).ToListAsync<ChannelUsers>();
            }
            else if (userType == ((int)UserTypeEnum.Customer))
            {
                users = await _db.ChannelUserTypeCustomerUser.Where(i => i.UserId == userId).ToListAsync<ChannelUsers>();
            }
            else if (userType == ((int)UserTypeEnum.Guest))
            {
                users = await _db.ChannelUserTypeCustomerGuest.Where(i => i.UserId == userId).ToListAsync<ChannelUsers>();
            }
            else if (userType == ((int)UserTypeEnum.ChatWidgetUser))
            {
                users = await _db.ChannelUserTypeChatWidgetUser.Where(i => i.UserId == userId).ToListAsync<ChannelUsers>();
            }
            if (users != null)
            {
                foreach (var user in users)
                {
                    user.IsActive = status;
                }
                await _db.SaveChangesAsync(CancellationToken.None);
                isUpdated = true;
            }
            return isUpdated;
        }

        public async Task UpdateUserDeActivate(ApplicationDbContext _db, int userId, int userType)
        {
            if (userType == ((int)UserTypeEnum.Hospitio))
            {
                var userDetail = await _db.Users.FirstOrDefaultAsync(c => c.Id == userId);
                if (userDetail != null)
                {
                    userDetail.DeActivated = DateTime.Now.ToUniversalTime();
                    _db.SaveChangesAsync(CancellationToken.None);
                }
            }
            else if (userType == ((int)UserTypeEnum.Customer))
            {
                var customerDetail = await _db.Customers.FirstOrDefaultAsync(c => c.Id == userId);
                if (customerDetail != null)
                {
                    customerDetail.DeActivated = DateTime.Now.ToUniversalTime();
                    _db.SaveChangesAsync(CancellationToken.None);
                }
            }
        }

        public async Task<UserDetail> GetUserDetailByChatId(ApplicationDbContext _db, int userId, string userType, int chatId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("Id", userId, DbType.Int32);
            spParams.Add("Type", userType);


            var userDetail = await _dapper
           .GetSingle<UserDetail>("[dbo].[SP_GETUserDetailByChatId]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return userDetail;
        }
        public async Task<UserChat> GetUserChatListByChatId(ApplicationDbContext _db, int userId, string userType, int chatId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("Id", userId, DbType.Int32);
            spParams.Add("Type", userType);

            var userChat = await _dapper
           .GetSingle<UserChat>("[dbo].[SP_GetChatListByChatId]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return userChat;
        }

        public async Task<bool> UpdateLastMessage(ApplicationDbContext _db, int userId, string userType, int chatId, int lastMessageReadId, DateTime lastMessageReadTime)
        {
            ChannelUsers? user = null;
            bool isUpdated = false;
            if (userType == "HospitioUser" || userType == ((int)UserTypeEnum.Hospitio).ToString())
            {
                user = await _db.ChannelUserTypeUser.Where(i => i.UserId == userId && i.ChannelId == chatId).FirstOrDefaultAsync();
            }
            else if (userType == "CustomerUser" || userType == ((int)UserTypeEnum.Customer).ToString())
            {
                user = await _db.ChannelUserTypeCustomerUser.Where(i => i.UserId == userId && i.ChannelId == chatId).FirstOrDefaultAsync();
            }
            else if (userType == "CustomerGuest" || userType == ((int)UserTypeEnum.Guest).ToString())
            {
                user = await _db.ChannelUserTypeCustomerGuest.Where(i => i.UserId == userId && i.ChannelId == chatId).FirstOrDefaultAsync();
            }
            if (user != null)
            {
                user.LastMessageReadId = lastMessageReadId;
                user.LastMessageReadTime = lastMessageReadTime;
                await _db.SaveChangesAsync(CancellationToken.None);
                isUpdated = true;
            }
            return isUpdated;
        }

        public async Task<ReceiverData> GetReceiverData(ApplicationDbContext _db, int chatId, int userId,string userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType);

            var receiverData = await _dapper
           .GetSingle<ReceiverData>("[dbo].[SP_GetReceiverUser]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            return receiverData;
        }

        public async Task<List<ConnectedUsers>> GetConnectedUsers(ApplicationDbContext _db,int userId,int userType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", ((ChatUserTypeEnum)userType).ToString());

            var connectedUsers = await _dapper
           .GetAll<ConnectedUsers>("[dbo].[SP_GetConnectedUsers]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            
            return connectedUsers;
        }

        public async Task<CustomerGuestDetail> GetCustomerGuestDetailByChatId(ApplicationDbContext _db, int userId, int chatId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("ChatId", chatId, DbType.Int32);
            spParams.Add("Id", userId, DbType.Int32);

            var custGuestDetail = await _dapper
           .GetSingle<CustomerGuestDetail>("[dbo].[SP_GetCustomerGuestDetailByChatId]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return custGuestDetail;
        }

        public async Task<ChannelId> GetChatId(ApplicationDbContext _db, int userId, string userType, int claimUserId, string claimUserType)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType);
            spParams.Add("ClaimUserId", claimUserId, DbType.Int32);
            spParams.Add("ClaimUserType", ((ChatUserTypeEnum)int.Parse(claimUserType)).ToString());

            var channelId = await _dapper
           .GetSingle<ChannelId>("[dbo].[SP_GetChatId]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            return channelId;
        }
        public async Task<GuestDetails> GetGuestChatDetailById(ApplicationDbContext _db, int guestId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("GuestId", guestId, DbType.Int32);

            var guestDetail = await _dapper
           .GetSingle<GuestDetails>("[dbo].[SP_GetGuestDetails]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return guestDetail;
        }



        public async Task<UserDataFromPhoneNumber> GetUserDetailFromPhoneNumber(string phoneNumber , string? UserType , int? userId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("@PhoneNumber", phoneNumber, DbType.Int64);
            spParams.Add("@AnonymousUsersType", UserType != null ? ((ChatUserTypeEnum)int.Parse(UserType)).ToString() : null, DbType.String);
            spParams.Add("@AnonymousUserId", userId.HasValue ? userId : null, DbType.Int32);

            var user = await _dapper.GetSingle<UserDataFromPhoneNumber>("[dbo].[SP_GetUserDetailFromPhoneNumber]", spParams, CancellationToken.None, System.Data.CommandType.StoredProcedure);

            return user;
        }

        public async Task<Channels> GetChannelDataFromUsersDetail(int userIdTo, string userTypeTo, int userIdFrom, string userTypeFrom)
        {
            var spParams = new DynamicParameters();
            spParams.Add("@UserIdTo", userIdTo, DbType.Int32);
            spParams.Add("@UserTypeTo", ((ChatUserTypeEnum)int.Parse(userTypeTo)).ToString());
            spParams.Add("@UserIdFrom", userIdFrom, DbType.Int32);
            spParams.Add("@UserTypeFrom", ((ChatUserTypeEnum)int.Parse(userTypeFrom)).ToString());

            var channels = await _dapper.GetSingle<Channels>("[dbo].[SP_GetChannelDataFromUsersDetail]", spParams, CancellationToken.None, System.Data.CommandType.StoredProcedure);
            return channels;
        }
        public async Task<int> GetTicketTotalUnReadCountByTicketId(int id, int userType, int ticketId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("@Id", id, DbType.Int32);
            spParams.Add("@UserType", userType, DbType.Int32);
            spParams.Add("@TicketId", ticketId, DbType.Int32);

            var totalMessageUnReadCount = await _dapper.GetSingle<int>("[dbo].[SP_GetTicketReplyUnReadMessageCount]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            return totalMessageUnReadCount;
        }
        public async Task<TicketMessageCount> UpdateAllTicketMessageRead(int id, int userType, int ticketId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("@UserId", id, DbType.Int32);
            spParams.Add("@UserType", userType, DbType.Int32);
            spParams.Add("@TicketId", ticketId, DbType.Int32);

            var updateMessageCount = await _dapper.GetSingle<TicketMessageCount>("[dbo].[SP_UpdateAllTicketReadMessage]", spParams, CancellationToken.None, System.Data.CommandType.StoredProcedure);
            return updateMessageCount;
        }

        public async Task<TicketDetail> GetTicketDetailByTicketId(string userId, string userType, int ticketId)
        {
            var spParams = new DynamicParameters();
            spParams.Add("@UserId", userId, DbType.Int32);
            spParams.Add("@UserType", userType, DbType.Int32);
            spParams.Add("@TicketId", ticketId, DbType.Int32);

            var ticketDetail = await _dapper.GetSingle<TicketDetail>("[dbo].[SP_GetTicketDetailByTicketId]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            return ticketDetail;
        }

        public async Task<int> GetTotalUnreadTicketCount(string userId, string userType,string customerId=null)
        {
            var spParams = new DynamicParameters();
            spParams.Add("UserId", userId, DbType.Int32);
            spParams.Add("UserType", userType, DbType.Int32);
            spParams.Add("CustomerId", customerId !=null && customerId != ""? customerId : 0, DbType.Int32);

            var totalCount = await _dapper.GetSingle<int>("[dbo].[SP_GetTotalUnreadTicketCount]", spParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return totalCount;
        }

        public async Task<NotificationCount> UpdateAllNotificationRead(int userId , int UserType)
        {
            var spParamas = new DynamicParameters();
            spParamas.Add("UserId" , userId, DbType.Int32);
            spParamas.Add("UserType", UserType, DbType.Int32);

            var notificationTotalCount = await _dapper.GetSingle<NotificationCount>("[dbo].[SP_UpdateNotificationRead]", spParamas , CancellationToken.None , commandType:CommandType.StoredProcedure);

            return notificationTotalCount;
        }

        public async Task<int> GetTotalUnreadQACount(string userId)
        {
            var spQAParams = new DynamicParameters();
            spQAParams.Add("UserId", userId, DbType.Int32);

            var totalUnreadQACount = await _dapper.GetSingle<int>("[dbo].[SP_GetTotalUnreadQACount]", spQAParams, CancellationToken.None, commandType: CommandType.StoredProcedure);
            
            return totalUnreadQACount;
        }

        public async Task<int> GetTotalUnreadNotificationCount(string UserId , int userType)
        {
            var spNotificationParams = new DynamicParameters();
            spNotificationParams.Add("UserId", UserId != null && UserId != "" ? UserId : 0, DbType.Int32);
            spNotificationParams.Add("UserType", userType, DbType.Int32);

            var totalUnreadNotificationCount = await _dapper.GetSingle<int>("[dbo].[SP_GetTotalUnreadNotificationCount]", spNotificationParams, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return totalUnreadNotificationCount;
        }

        public async Task<List<TotalUnreadCount>> GetTotalUnreadCount(string userId, string userType, string customerId = null)
        {
            List<TotalUnreadCount> totalUnreadCount = new List<TotalUnreadCount>();

            var totalUnreadMessageCount = await GetTotalUnreadMessageCount(userId, int.Parse(userType));

            totalUnreadCount.Add(new TotalUnreadCount()
            {
                Type = "Communication",
                Count = totalUnreadMessageCount
            });
            
            if(int.Parse(userType) != (int)UserTypeEnum.ChatWidgetUser)
            {
                var totalUnreadTicketCount = await GetTotalUnreadTicketCount(userId, userType, customerId);

                totalUnreadCount.Add(new TotalUnreadCount()
                {
                    Type = "Ticket",
                    Count = totalUnreadTicketCount
                });
            }

            if(customerId != null && customerId != "")
            {
                var totalUnreadQACount = await GetTotalUnreadQACount(userId);

                totalUnreadCount.Add(new TotalUnreadCount()
                {
                    Type = "QA",
                    Count = totalUnreadQACount
                });

                var totalUnreadNotificationCount = await GetTotalUnreadNotificationCount(customerId , int.Parse(userType));

                totalUnreadCount.Add(new TotalUnreadCount()
                {
                    Type = "Notification",
                    Count = totalUnreadNotificationCount
                });
            }

            return totalUnreadCount;
        }

        public async Task<QaUnreadCount> UpdateAllQuestionAnswersRead(string userId)
        {
            var spParamas = new DynamicParameters();
            spParamas.Add("UserId", userId, DbType.Int32);

            var questionAnsersTotalCount = await _dapper.GetSingle<QaUnreadCount>("[dbo].[SP_UpdateQustionAnswerRead]", spParamas, CancellationToken.None, commandType: CommandType.StoredProcedure);

            return questionAnsersTotalCount;
        }

        public async Task<GetReceiverUserWP> SendWhatsAppMessage(ApplicationDbContext _db, string message, int chatId, string senderId, string UserType,string SP)
        {
            var spParamas = new DynamicParameters();
            spParamas.Add("UserId", senderId, DbType.Int32);
            spParamas.Add("UserType", ((ChatUserTypeEnum)int.Parse(UserType)).ToString(), DbType.String);
            spParamas.Add("ChatId", chatId, DbType.Int32);
            var receiverDetails = await _dapper.GetSingle<GetReceiverUserWP>(SP, spParamas, CancellationToken.None, commandType: CommandType.StoredProcedure);
            
            return receiverDetails;
        }
        public async Task<List<CustomerOnboardingStatus>> GetCustomerOnboardingStatus(ApplicationDbContext _db ,string customerId)
        {
            List<CustomerOnboardingStatus> customerOnboardingStatuses = new List<CustomerOnboardingStatus>();

            //Get CustomerGuestsCheckInFormBuilders Data for Get OnlineCheckInStatus Data
            var onlineCheckInStatus = await _db.CustomerGuestsCheckInFormBuilders.Where(s => s.CustomerId == int.Parse(customerId) && s.DeletedAt == null).FirstOrDefaultAsync(CancellationToken.None);

            if (onlineCheckInStatus != null)
            {
                if(onlineCheckInStatus.SubmissionMail != null && onlineCheckInStatus.IsOnlineCheckInFormEnable == false)
                {
                    customerOnboardingStatuses.Add(new CustomerOnboardingStatus()
                    {
                        Type = "online-check-in",
                        Status = "Completed"
                    });
                }
                else if (onlineCheckInStatus.SubmissionMail != null && onlineCheckInStatus.IsOnlineCheckInFormEnable == true)
                {
                    customerOnboardingStatuses.Add(new CustomerOnboardingStatus()
                    {
                        Type = "online-check-in",
                        Status = "Activated"
                    });
                }
            }
            if(onlineCheckInStatus == null)
            {
                customerOnboardingStatuses.Add(new CustomerOnboardingStatus()
                {
                    Type = "online-check-in",
                    Status = "In Progress"
                });
            }

            //Get CustomerRoomData For Get GuestPortal Status
            var spParams = new DynamicParameters();
            spParams.Add("CustomerId", int.Parse(customerId), DbType.Int32);

            var customerRoom = await _dapper.GetAll<CustomerAppBuilders>("[dbo].[GetCustomerRoomNames]", spParams, CancellationToken.None, CommandType.StoredProcedure);

            var guestPortalStatus = await GetGuestPortalStatus(customerRoom, _db , customerId);

            if(guestPortalStatus != null)
            {
                customerOnboardingStatuses.Add(guestPortalStatus);
            }

            return customerOnboardingStatuses;
        }

        public async Task<CustomerOnboardingStatus> GetGuestPortalStatus(List<CustomerAppBuilders> customerRoom, ApplicationDbContext _db, string customerId)
        {
            var getTotalRoomNumber = await _db.Customers.Where(s => s.Id == int.Parse(customerId)).FirstOrDefaultAsync();
            CustomerOnboardingStatus customerOnboardingStatuses = new CustomerOnboardingStatus();

            var roomNumber = (getTotalRoomNumber != null && getTotalRoomNumber.NoOfRooms != null) ? getTotalRoomNumber.NoOfRooms : 0;
            if (roomNumber != null)
            {
                customerRoom = customerRoom.Take(roomNumber.GetValueOrDefault()).ToList();
            }

            var completeRoom = customerRoom.Where(s => s.IsWork == 1).ToList();
            var inProgressRoom = customerRoom.Where(s => s.IsWork == 2).ToList();
            var notStartedRoom = customerRoom.Where(s => s.IsWork == 3).ToList();
            if (customerRoom == null || customerRoom.Count == 0 || (customerRoom.Count == notStartedRoom.Count))
            {
                customerOnboardingStatuses = new CustomerOnboardingStatus()
                {
                    Type = "guest-portal",
                    Status = "Not Started"
                };
            }
            else if (customerRoom.Count == completeRoom.Count)
            {
                customerOnboardingStatuses = new CustomerOnboardingStatus()
                {
                    Type = "guest-portal",
                    Status = "Completed"
                };
            }
            else if (inProgressRoom.Count > 0 || (inProgressRoom.Count == 0 && completeRoom.Count != customerRoom.Count && notStartedRoom.Count != customerRoom.Count))
            {
                customerOnboardingStatuses = new CustomerOnboardingStatus()
                {
                    Type = "guest-portal",
                    Status = "In Progress"
                };
            }

            return customerOnboardingStatuses;
        }

    }
}
