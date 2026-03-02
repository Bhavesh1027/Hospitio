using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.RabbitMQ;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Chat.Models.Chat;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
public record GetInBoundWebhookHandlerRequest(GetInboundWebhookIn In) : IRequest<AppHandlerResponse>;
public class GetInBoundWebhookHandler : IRequestHandler<GetInBoundWebhookHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    private readonly IRabbitMQClient _rabbit;

    public GetInBoundWebhookHandler(ApplicationDbContext db, IDapperRepository dapper, IHubContext<ChatHub> hubContext, IChatService chatService, IHandlerResponseFactory response, IRabbitMQClient rabbit)
    {
        _db = db;
        _dapper = dapper;
        _hubContext = hubContext;
        _chatService = chatService;
        _response = response;
        _rabbit = rabbit;
    }
    public async Task<AppHandlerResponse> Handle(GetInBoundWebhookHandlerRequest request, CancellationToken cancellationToken)
    {
        //HttpClient client = new HttpClient();
        //HttpRequest Request = new ();
        //HttpClient client = new HttpClient();
        //HttpRequest request = new HttpRequest();
        //var sms = WebhookParser.ParseWebhook<WhatsAppResponse>(re.Body, "application/json");
        //string requestBody = await new StreamReader(re.request.Body).ReadToEndAsync();
        Console.WriteLine("SMS Received");
        Console.WriteLine($"Message Id: {request.In.message_uuid}");
        Console.WriteLine($"To: {request.In.to}");
        Console.WriteLine($"From: {request.In.from}");
        Console.WriteLine($"Text: {request.In.text}");


        await _rabbit.ReceiveWPMessage(request.In);
        ////var toNumber = "+298562325";
        //var userTo = await _chatService.GetUserDetailFromPhoneNumber(request.In.to);

        //////var fromNumber = "+447888100001";
        //var userFrom = await _chatService.GetUserDetailFromPhoneNumber(request.In.from);

        //if(userFrom.UserId == 0 && userTo.UserId != 0)
        //{
        //    var anonymousUser = new AnonymousUsers();
        //    if (await _db.Users.Where(e => e.Id == userTo.UserId && 1 == Convert.ToInt32(userTo.UserType)).AnyAsync(cancellationToken))
        //    {
        //        anonymousUser.UserType = (int)UserTypeEnum.Customer;
        //    }
        //    else if(await _db.CustomerUsers.Where(e => e.Id == userTo.UserId && 2 == Convert.ToInt32(userTo.UserType)).AnyAsync(cancellationToken))
        //    {
        //        anonymousUser.UserType = (int)UserTypeEnum.Guest;
        //    }
        //    if (await _db.AnonymousUsers.Where(e => e.PhoneNumber == $"+{request.In.from}").FirstOrDefaultAsync(cancellationToken) == null) 
        //    {
        //        anonymousUser.PhoneNumber = $"+{request.In.from}";
        //        _db.AnonymousUsers.Add(anonymousUser);
        //        await _db.SaveChangesAsync(cancellationToken);
        //    }

        //    var objanonymousUsers = new UserDataFromPhoneNumber
        //    {
        //        UserId = anonymousUser.Id,
        //        UserType = ((int)ChatUserTypeEnum.AnonymousUser).ToString()
        //    };

        //    userFrom = objanonymousUsers;
        //}
        ////var source = MessageSourceEnum.;

        ////var source = request.In.channel;
        //var source= (byte)(MessageSourceEnum)Enum.Parse(typeof(MessageSourceEnum), request.In.channel);

        //if (userTo.UserId != 0 && userFrom.UserId != 0)
        //{
        //    var channels = await _chatService.GetChannelDataFromUsersDetail(userTo.UserId, userTo.UserType, userFrom.UserId, userFrom.UserType);
        //    var message = await _db.ChannelMessages.FirstOrDefaultAsync(i => i.MessageUuid == request.In.message_uuid);

        //    if (message == null)
        //    {
        //        if (channels != null)
        //        {
        //            // send message in this channel
        //            var isMessageSend = await SendMessage(channels.Id, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId.ToString(), userTo.UserType, request.In.text, request.In.message_uuid,source, "Text", null, null, null);
        //        }
        //        else
        //        {
        //            // create channel and send message into it.
        //            var chatId = await _chatService.CreateChat(_db, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId, ((ChatUserTypeEnum)int.Parse(userTo.UserType)).ToString());

        //            var isMessageSend = await SendMessage(chatId, userFrom.UserId.ToString(), userFrom.UserType, userTo.UserId.ToString(), userTo.UserType, request.In.text, request.In.message_uuid,source, "Text", null, null, null);
        //        }
        //    }
        //}
        return _response.Success(new GetInBoundWebhookOut("Receive"));
    }

    //public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message,string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    //{
    //    var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url,message_uuid,1,null);

    //    var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
    //    await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
    //    await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

    //    var totalUnreadMessageCountResponse = new { Type="Communication",Id = chatId, Count = totalUnreadMessageCount };

    //    await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
    //    return 1;
    //}
}