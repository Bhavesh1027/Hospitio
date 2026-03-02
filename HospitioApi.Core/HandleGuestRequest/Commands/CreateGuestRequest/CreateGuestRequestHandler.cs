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
using System.Linq.Dynamic.Core;
using Vonage.Common;

namespace HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;

public record CreateGuestRequestRequest(CreateGuestRequestIn In) : IRequest<AppHandlerResponse>;

public class CreateGuestRequestHandler : IRequestHandler<CreateGuestRequestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHandlerResponseFactory _response;
    public CreateGuestRequestHandler(ApplicationDbContext db,IChatService chatService , IHubContext<ChatHub> hubContext,IHandlerResponseFactory response)
    {
        _db = db;
        _chatService = chatService;
        _hubContext = hubContext;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateGuestRequestRequest request, CancellationToken cancellationToken)
    {
        var guestRequests = request.In.GuestRequests;
        List<GuestRequest> guestRequestsList = new List<GuestRequest>();

        foreach (var guestRequest in guestRequests)
        {
            var checkCustomer = await _db.Customers.Where(e => e.Id == guestRequest.CustomerId).SingleOrDefaultAsync(cancellationToken);
            if (checkCustomer == null)
            {
                return _response.Error($"Customer not found.", AppStatusCodeError.UnprocessableEntity422);
            }

            var checkGuest = await _db.CustomerGuests.Where(e => e.Id == guestRequest.GuestId).SingleOrDefaultAsync(cancellationToken);
            if (checkGuest == null)
            {
                return _response.Error($"Customer guest not found.", AppStatusCodeError.UnprocessableEntity422);
            }

            switch (guestRequest.RequestType)
            {
                case GuestRequestTypeEnum.Concierge:
                    {
                        if (guestRequest.CustomerGuestAppConciergeItemId != null)
                        {
                            if (!await IsDuplicate(guestRequest, guestRequest.CustomerGuestAppConciergeItemId, cancellationToken))
                            {
                                var obj = GenerateGuestRequestModel(guestRequest, guestRequest.CustomerGuestAppConciergeItemId);
                                guestRequestsList.Add(obj);
                            }
                        }
                    }
                    break;
                case GuestRequestTypeEnum.Housekeeping:
                    {
                        if (guestRequest.CustomerGuestAppHousekeepingItemId != null)
                        {

                            if (!await IsDuplicate(guestRequest, guestRequest.CustomerGuestAppHousekeepingItemId, cancellationToken))
                            {
                                var obj = GenerateGuestRequestModel(guestRequest, guestRequest.CustomerGuestAppHousekeepingItemId);
                                guestRequestsList.Add(obj);
                            }
                        }
                    }
                    break;
                //case GuestRequestTypeEnum.EnhanceYourStay:
                //    {
                //        if (guestRequest.CustomerGuestAppEnhanceYourStayItemId != null)
                //        {

                //            if (!await IsDuplicate(guestRequest, guestRequest.CustomerGuestAppEnhanceYourStayItemId, cancellationToken))
                //            {
                //                var obj = GenerateGuestRequestModel(guestRequest, guestRequest.CustomerGuestAppEnhanceYourStayItemId);
                //                guestRequestsList.Add(obj);
                //            }
                //        }
                //    }
                //    break;
                case GuestRequestTypeEnum.RoomService:
                    {
                        if (guestRequest.CustomerGuestAppRoomServiceItemId != null)
                        {

                            if (!await IsDuplicate(guestRequest, guestRequest.CustomerGuestAppRoomServiceItemId, cancellationToken))
                            {
                                var obj = GenerateGuestRequestModel(guestRequest, guestRequest.CustomerGuestAppRoomServiceItemId);
                                guestRequestsList.Add(obj);
                            }
                        }
                    }
                    break;
                case GuestRequestTypeEnum.Reception:
                    {
                        if (guestRequest.CustomerGuestAppReceptionItemId != null)
                        {

                            if (!await IsDuplicate(guestRequest, guestRequest.CustomerGuestAppReceptionItemId, cancellationToken))
                            {
                                var obj = GenerateGuestRequestModel(guestRequest, guestRequest.CustomerGuestAppReceptionItemId);
                                guestRequestsList.Add(obj);
                            }
                        }
                    }
                    break;
            }
        }

        if (guestRequestsList.Any())
        {
            await _db.GuestRequests.AddRangeAsync(guestRequestsList, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

        }
        
        foreach (var guestRequest in guestRequestsList)
        {
            var customerUser = await _db.CustomerUsers.Where(e => e.CustomerId == guestRequest.CustomerId).FirstOrDefaultAsync();
            if (customerUser.Id != null && guestRequest.GuestId != null)
            {
                var receiverUserType = ((int)UserTypeEnum.Customer).ToString();
                var SenderUserType = ((int)UserTypeEnum.Guest).ToString();
                var channels = await _chatService.GetChannelDataFromUsersDetail(customerUser.Id, receiverUserType,guestRequest.GuestId ?? 0, SenderUserType);
                var message = "";
                if(guestRequest.RequestType == (byte)GuestRequestTypeEnum.Concierge)
                {
                    var concerige = await _db.CustomerGuestAppConciergeItems.Where(e => e.Id == guestRequest.CustomerGuestAppConciergeItemId).FirstOrDefaultAsync();
                    if (concerige != null)
                    {
                        message = concerige.Name;
                    }
                } else if(guestRequest.RequestType == (byte)GuestRequestTypeEnum.Housekeeping)
                {
                    var housekeeping = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.Id == guestRequest.CustomerGuestAppHousekeepingItemId).FirstOrDefaultAsync();
                    if (housekeeping != null)
                    {
                        message = housekeeping.Name;
                    }
                } else if(guestRequest.RequestType == (byte)GuestRequestTypeEnum.RoomService)
                {
                    var roomservice = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.Id == guestRequest.CustomerGuestAppRoomServiceItemId).FirstOrDefaultAsync();
                    if (roomservice != null)
                    {
                        message = roomservice.Name;
                    }
                } else if (guestRequest.RequestType == (byte)GuestRequestTypeEnum.Reception)
                {
                    var reception = await _db.CustomerGuestAppReceptionItems.Where(e => e.Id == guestRequest.CustomerGuestAppReceptionItemId).FirstOrDefaultAsync();
                    if (reception != null)
                    {
                        message = reception.Name;
                    }
                }
                if (channels != null)
                {
                    // send message in this channel
                    var isMessageSend = await SendMessage(channels.Id, guestRequest.GuestId.ToString(), SenderUserType, customerUser.Id.ToString(), receiverUserType, message, null, 2, "Text", null, guestRequest.Id, null);
                }
                else
                {
                    // create channel and send message into it.
                    var receiveUser = ChatUserTypeEnum.CustomerUser.ToString();
                    var chatId = await _chatService.CreateChat(_db, guestRequest.GuestId.ToString(), SenderUserType, customerUser.Id, receiveUser);
                    var isMessageSend = await SendMessage(chatId, guestRequest.GuestId.ToString(), SenderUserType, customerUser.Id.ToString(), receiverUserType, message, null, 2, "Text", null, guestRequest.Id, null);
                }
            }
        }
        return _response.Success(new CreateGuestRequestOut("Create guest request successful."));
    }

    private GuestRequest GenerateGuestRequestModel(GuestRequestIn guestRequest, int? itemId)
    {
        return new GuestRequest
        {
            CustomerId = guestRequest.CustomerId,
            RequestType = (byte)guestRequest.RequestType,
            CustomerGuestAppConciergeItemId = guestRequest.RequestType == GuestRequestTypeEnum.Concierge ? itemId : null,
            CustomerGuestAppEnhanceYourStayItemId = guestRequest.RequestType == GuestRequestTypeEnum.EnhanceYourStay ? itemId : null,
            CustomerGuestAppHousekeepingItemId = guestRequest.RequestType == GuestRequestTypeEnum.Housekeeping ? itemId : null,
            CustomerGuestAppRoomServiceItemId = guestRequest.RequestType == GuestRequestTypeEnum.RoomService ? itemId : null,
            CustomerGuestAppReceptionItemId = guestRequest.RequestType == GuestRequestTypeEnum.Reception ? itemId : null,
            GuestId = guestRequest.GuestId,
            MonthValue = guestRequest.MonthValue,
            DayValue = guestRequest.DayValue,
            YearValue = guestRequest.YearValue,
            HourValue = guestRequest.HourValue,
            MinuteValue = guestRequest.MinuteValue,
            PickupLocation = guestRequest.PickupLocation,
            Destination = guestRequest.Destination,
            Comment = guestRequest.Comment,
            GRPaymentId = guestRequest.PaymentId,
            GRPaymentDetails = guestRequest.PaymentDetails,
            Status = guestRequest.Status != null ? (byte)guestRequest.Status : (byte)GuestRequestStatusEnum.InProgress,
            IsActive = guestRequest.IsActive,
            QuantityBar = guestRequest.QuantityBar
        };
    }

    private async Task<bool> IsDuplicate(GuestRequestIn guestRequest, int? itemId, CancellationToken cancellationToken)
    {
        var check = await _db.GuestRequests.Where(e =>
        e.CustomerId == guestRequest.CustomerId &&
        e.RequestType == (byte)guestRequest.RequestType &&
        e.GuestId == guestRequest.GuestId &&
        e.MonthValue == guestRequest.MonthValue &&
        e.DayValue == guestRequest.DayValue &&
        e.YearValue == guestRequest.YearValue &&
        e.HourValue == guestRequest.HourValue &&
        e.MinuteValue == guestRequest.MinuteValue &&
        e.Status == (byte)guestRequest.Status
        ).FirstOrDefaultAsync(cancellationToken);

        if (check != null)
        {
            if (check.RequestType == (byte)GuestRequestTypeEnum.Reception)
            {
                return check.CustomerGuestAppReceptionItemId == itemId;
            }
            else if (check.RequestType == (byte)GuestRequestTypeEnum.Housekeeping)
            {
                return check.CustomerGuestAppHousekeepingItemId == itemId;
            }
            else if (check.RequestType == (byte)GuestRequestTypeEnum.EnhanceYourStay)
            {
                return check.CustomerGuestAppEnhanceYourStayItemId == itemId;
            }
            else if (check.RequestType == (byte)GuestRequestTypeEnum.Concierge)
            {
                return check.CustomerGuestAppConciergeItemId == itemId;
            }
            else if (check.RequestType == (byte)GuestRequestTypeEnum.RoomService)
            {
                return check.CustomerGuestAppRoomServiceItemId == itemId;
            }
            return false;
        }

        else
            return false;
    }

    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, 2,1);

        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}
