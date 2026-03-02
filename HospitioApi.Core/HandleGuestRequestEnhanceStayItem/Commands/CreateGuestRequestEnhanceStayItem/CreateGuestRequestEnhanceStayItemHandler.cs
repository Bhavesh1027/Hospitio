using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequestEnhanceStayItem.Commands.CreateGuestRequestEnhanceStayItem;
public record CreateGuestRequestEnhanceStayItemRequest(CreateGuestRequestEnhanceStayItemIn In) : IRequest<AppHandlerResponse>;
public class CreateGuestRequestEnhanceStayItemHandler : IRequestHandler<CreateGuestRequestEnhanceStayItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHandlerResponseFactory _response;
    public CreateGuestRequestEnhanceStayItemHandler(ApplicationDbContext db, IChatService chatService, IHubContext<ChatHub> hubContext, IHandlerResponseFactory response)
    {
        _db = db;
        _chatService = chatService;
        _hubContext = hubContext;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateGuestRequestEnhanceStayItemRequest request, CancellationToken cancellationToken)
    {
        if (request.In == null)
        {
            return _response.Error($"Request cannot be null.", AppStatusCodeError.Forbidden403);
        }
        var checkCustomer = await _db.Customers.Where(e => e.Id == request.In.CustomerId).SingleOrDefaultAsync(cancellationToken);
        if (checkCustomer == null)
        {
            return _response.Error($"Customer not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        var checkGuest = await _db.CustomerGuests.Where(e => e.Id == request.In.GuestId).SingleOrDefaultAsync(cancellationToken);
        if (checkGuest == null)
        {
            return _response.Error($"Customer guest not found.", AppStatusCodeError.UnprocessableEntity422);
        }
        var enhanceStay = new EnhanceStayItemsGuestRequest();
        enhanceStay.CustomerId = request.In.CustomerId;
        enhanceStay.GuestId = request.In.GuestId;
        enhanceStay.CustomerGuestAppEnhanceYourStayItemId = request.In.CustomerGuestAppEnhanceYourStayItemId;
        enhanceStay.Qty = request.In.Qty;
        enhanceStay.GRPaymentId = request.In.PaymentId;
        enhanceStay.GRPaymentDetails = request.In.PaymentDetails;
        enhanceStay.Status = request.In.Status;
        enhanceStay.IsActive = true;

        await _db.EnhanceStayItemsGuestRequests.AddAsync(enhanceStay, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var guestRequests = request.In.enhanceStayItemExtraIns;
        List<EnhanceStayItemExtraGuestRequest> enhanceStayItemExtraGuestRequests = new List<EnhanceStayItemExtraGuestRequest>();

        foreach (var enhanceStayItem in guestRequests)
        {
            EnhanceStayItemExtraGuestRequest enhanceStayItemExtraGuestRequest = new EnhanceStayItemExtraGuestRequest();
            enhanceStayItemExtraGuestRequest.EnhanceStayItemsGuestRequestId = enhanceStay.Id;
            enhanceStayItemExtraGuestRequest.CustomerGuestAppEnhanceYourStayCategoryItemsExtraId = enhanceStayItem.CustomerGuestAppEnhanceYourStayCategoryItemsExtraId;
            enhanceStayItemExtraGuestRequest.Month = enhanceStayItem.Month;
            enhanceStayItemExtraGuestRequest.Day = enhanceStayItem.Day;
            enhanceStayItemExtraGuestRequest.Year = enhanceStayItem.Year;
            enhanceStayItemExtraGuestRequest.Hour = enhanceStayItem.Hour;
            enhanceStayItemExtraGuestRequest.Minute = enhanceStayItem.Minute;
            enhanceStayItemExtraGuestRequest.PickupLocation = enhanceStayItem.PickupLocation;
            enhanceStayItemExtraGuestRequest.Qunatity = enhanceStayItem.Qunatity;
            enhanceStayItemExtraGuestRequest.Destination = enhanceStayItem.Destination;
            enhanceStayItemExtraGuestRequest.Comment = enhanceStayItem.Comment;
            enhanceStayItemExtraGuestRequest.Status = enhanceStayItem.Status;
            enhanceStayItemExtraGuestRequest.IsActive = true;
            enhanceStayItemExtraGuestRequests.Add(enhanceStayItemExtraGuestRequest);
        }

        if (enhanceStayItemExtraGuestRequests.Any())
        {
            await _db.EnhanceStayItemExtraGuestRequests.AddRangeAsync(enhanceStayItemExtraGuestRequests, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        //foreach (var guestRequest in enhanceStayItemExtraGuestRequests)
        //{

        var customerUser = await _db.CustomerUsers.Where(e => e.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();
        if (customerUser.Id != null && request.In.GuestId != null)
        {
            var receiverUserType = ((int)UserTypeEnum.Customer).ToString();
            var SenderUserType = ((int)UserTypeEnum.Guest).ToString();
            var channels = await _chatService.GetChannelDataFromUsersDetail(customerUser.Id, receiverUserType, request.In.GuestId, SenderUserType);
            var enhanceYourStayItem = await _db.CustomerGuestAppEnhanceYourStayItems.Where(e => e.Id == enhanceStay.CustomerGuestAppEnhanceYourStayItemId).FirstOrDefaultAsync(cancellationToken);
            var message = "";
            if (enhanceYourStayItem != null)
            {
                message = enhanceYourStayItem.ShortDescription.Replace("<p>", "").Replace("</p>", "");
            }
            if (channels != null)
            {
                // send message in this channel
                var isMessageSend = await SendMessage(channels.Id, request.In.GuestId.ToString(), SenderUserType, customerUser.Id.ToString(), receiverUserType, message, null, 2, "Text", null, null, null, enhanceStay.Id);
            }
            else
            {
                // create channel and send message into it.
                var chatId = await _chatService.CreateChat(_db, request.In.GuestId.ToString(), SenderUserType, customerUser.Id, receiverUserType);
                var isMessageSend = await SendMessage(chatId, request.In.GuestId.ToString(), SenderUserType, customerUser.Id.ToString(), receiverUserType, message, null, 2, "Text", null, null, null, enhanceStay.Id);
            }
        }
        //}

        return _response.Success(new CreateGuestRequestEnhanceStayItemOut("Create guest request successful."));
    }

    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url, int? enhanceRequestId)
    {
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, 2, 2, enhanceRequestId);

        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}
