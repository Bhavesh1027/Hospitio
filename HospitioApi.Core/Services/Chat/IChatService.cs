using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;
using HospitioApi.Core.Services.Chat.Models;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Chat.Models.QuestionAnswers;
using HospitioApi.Core.Services.Chat.Models.Tickets;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Chat
{
    public interface IChatService
    {
        Task<bool> UpdateUserChatStatus(ApplicationDbContext _db, int userId,int userType, bool status);
        Task<int> CreateChat(ApplicationDbContext _db, string senderId,string userType, int receiverId, string chatType);
        Task<List<Channels>> GetChatList(ApplicationDbContext _db, string userId, int page, int limit, bool isDeleted,string chatType = null, List<string> filters = null);
        Task<List<ChatMessageList>> GetChatMessageList(ApplicationDbContext _db, int chatId, int page, int limit,string userId,string userType);
        Task<ChatMessageList> SendMessage(ApplicationDbContext _db, int chatId, string senderId, string userType, string message, byte source, string type, string attachment, int? requestId,string url,string message_uuid, byte messageRequestType,byte? requestType, int? EnhanceRequestId = null);
        Task<ChatList> MarkChatMessageAsRead(ApplicationDbContext _db, int chatId, int userId, int userType);
        Task<int> DeleteChat(ApplicationDbContext _db, int chatId, int userId);
        Task<int> GetTotalUnreadMessageCount(string userId,int userType);
        Task<int> GetTotalUnreadMessageCountPerChat(ApplicationDbContext _db, string userId,string userType, int chatId);
        Task<ChatListResponse> GetChatListDapper(ApplicationDbContext _db, int userId, int page, int limit, bool isDeleted,int userType);
        Task<ChatListCustomerResponse> GetChatListCustomer(ApplicationDbContext _db, string userId,string userType, int page, int limit, string chatType = null, string filters = null);
        Task UpdateUserDeActivate(ApplicationDbContext _db, int userId, int userType);
        Task<UserDetail> GetUserDetailByChatId(ApplicationDbContext _db, int userId, string userType, int chatId);
        Task<UserChat> GetUserChatListByChatId(ApplicationDbContext _db, int userId,string userType, int chatId);
        Task<bool> UpdateLastMessage(ApplicationDbContext _db, int userId, string userType, int chatId,int lastMessageReadId, DateTime lastMessageReadTime);
        Task<ReceiverData> GetReceiverData(ApplicationDbContext _db, int chatId, int userId, string userType);
        Task<List<ConnectedUsers>> GetConnectedUsers(ApplicationDbContext _db, int userId,int userType);
        Task<CustomerGuestDetail> GetCustomerGuestDetailByChatId(ApplicationDbContext _db, int userId, int chatId);
        Task<ChannelId> GetChatId(ApplicationDbContext _db, int userId, string userType, int claimUserId, string claimUserType);
        Task<GuestDetails> GetGuestChatDetailById(ApplicationDbContext _db, int guestId);
        Task<UserDataFromPhoneNumber> GetUserDetailFromPhoneNumber(string phoneNumber , string? UserType , int? userId);
        Task<Channels> GetChannelDataFromUsersDetail(int userIdTo, string userTypeTo, int userIdFrom, string userTypeFrom);
        Task<int> GetTicketTotalUnReadCountByTicketId(int id, int userType, int ticketId);
        Task<TicketMessageCount> UpdateAllTicketMessageRead(int id, int userType, int ticketId);    
        Task<NotificationCount> UpdateAllNotificationRead(int userId , int UserType);
        Task<TicketDetail> GetTicketDetailByTicketId(string userId, string userType,int ticketId);
        Task<int> GetTotalUnreadTicketCount(string userId, string userType,string customerId=null);
        Task<int> GetTotalUnreadQACount(string userId);
        Task<int> GetTotalUnreadNotificationCount(string customerId , int UserType);
        Task<List<TotalUnreadCount>> GetTotalUnreadCount(string userId, string userType,string customerId=null);
        Task<QaUnreadCount> UpdateAllQuestionAnswersRead(string userId);
        Task<GetReceiverUserWP> SendWhatsAppMessage(ApplicationDbContext _db, string message, int chatId,string senderId, string UserType,string SP);
        Task<List<CustomerOnboardingStatus>> GetCustomerOnboardingStatus(ApplicationDbContext _db ,string customerId);
        Task<CustomerOnboardingStatus> GetGuestPortalStatus(List<CustomerAppBuilders> customerRoom , ApplicationDbContext _db, string customerId);

    }
}
