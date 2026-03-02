using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.Services.BackGroundServiceData;
public class BackGroundServiceData : IBackGroundServiceData
{
    private readonly IDapperRepository _dapper;
    private readonly IChatService _chatService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<ChatHub> _hubContext;


    public BackGroundServiceData(IDapperRepository dapper, IChatService chatService, IServiceScopeFactory scopeFactory, IHubContext<ChatHub> hubContext)
    {
        _dapper = dapper;
        _chatService = chatService;
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;


    }
    public async Task<List<CustomerGuestJorneyDetails>> GetGuestMessageByCustomerId(int customerId, DateTime dateTime, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", customerId, DbType.Int32);
        spParams.Add("SendDateTime", dateTime, DbType.DateTime);

        var data = await _dapper.GetAll<CustomerGuestJorneyDetails>("[dbo].[SP_GetCustomerGuestJourneyDetailForSendMessage]", spParams, cancellationToken, CommandType.StoredProcedure);
        return data;
    }
    public async Task<List<Customer>> GetCustomers(CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        return await _dapper.GetAll<Customer>("[dbo].[SP_GetCustomer&CustomerUsers]", spParams, cancellationToken, CommandType.StoredProcedure);
    }

    public async Task AddAnonymousUser(ApplicationDbContext _db, GetInboundWebhookIn message, CancellationToken stoppingToken)
    {
        // Receiver Contact
        var userTo = await _chatService.GetUserDetailFromPhoneNumber(message.to , null , null);

        ////var fromNumber = "+447888100001";
        
        var userFrom = await _chatService.GetUserDetailFromPhoneNumber(message.from , userTo.UserType , userTo.UserId);

        if (userFrom.UserId == 0 && userTo.UserId != 0)
        {
            var anonymousUser = new AnonymousUsers();
            if (await _db.Users.Where(e => e.Id == userTo.UserId && 1 == Convert.ToInt32(userTo.UserType)).AnyAsync(stoppingToken))
            {
                anonymousUser.UserType = (int)UserTypeEnum.Customer;
            }
            else if (await _db.CustomerUsers.Where(e => e.Id == userTo.UserId && 2 == Convert.ToInt32(userTo.UserType)).AnyAsync(stoppingToken))
            {
                anonymousUser.UserType = (int)UserTypeEnum.Guest;
            }
            if (await _db.AnonymousUsers.Where(e => e.PhoneNumber == $"+{message.from}" && e.UserType == anonymousUser.UserType).FirstOrDefaultAsync(stoppingToken) == null)
            {
                anonymousUser.PhoneNumber = $"+{message.from}";
                _db.AnonymousUsers.Add(anonymousUser);
                await _db.SaveChangesAsync(stoppingToken);
            }

            var objanonymousUsers = new UserDataFromPhoneNumber
            {
                UserId = anonymousUser.Id,
                UserType = ((int)ChatUserTypeEnum.AnonymousUser).ToString()
            };

            userFrom = objanonymousUsers;
        }
        //var source = MessageSourceEnum.;

        //var source = request.In.channel;
        var source = (byte)(MessageSourceEnum)Enum.Parse(typeof(MessageSourceEnum), message.channel);

        if (userTo.UserId != 0 && userFrom.UserId != 0)
        {
            var channels = await _chatService.GetChannelDataFromUsersDetail(userTo.UserId, userTo.UserType, userFrom.UserId, userFrom.UserType);
            var message1 = await _db.ChannelMessages.Where(i => i.MessageUuid == message.message_uuid).FirstOrDefaultAsync(stoppingToken);

            if (message1 == null)
            {
                if (channels != null)
                {
                    // send message in this channel
                    var isMessageSend = await SendMessage(channels.Id, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId.ToString(), userTo.UserType, message.text, message.message_uuid, source,message.message_type, message.Attachment, null, message.FileUrl);
                }
                else
                {
                    // create channel and send message into it.
                    var chatId = await _chatService.CreateChat(_db, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId, ((ChatUserTypeEnum)int.Parse(userTo.UserType)).ToString());

                    var isMessageSend = await SendMessage(chatId, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId.ToString(), userTo.UserType, message.text, message.message_uuid, source, message.message_type, message.Attachment, null, message.FileUrl);
                }
            }
        }
    }
    public async Task<List<AlertMessages>> GetAlertMessages(DateTime currentDateTime, CancellationToken stoppingToken)
    {
        var spParams = new DynamicParameters();

        spParams.Add("SendDateTime", currentDateTime, DbType.DateTime);

        var data = await _dapper.GetAll<AlertMessages>("[dbo].[GetAlertMessages]", spParams, stoppingToken, CommandType.StoredProcedure);
        return data;
    }
    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        using var scopedb = _scopeFactory.CreateScope();
        var _db = scopedb.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, 1, null);

        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        //await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}




