using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
public record UpdateCustomerGuestRequest(UpdateCustomerGuestIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerGuestHandler : IRequestHandler<UpdateCustomerGuestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IVonageService _vonage;
    public UpdateCustomerGuestHandler(ApplicationDbContext db, IHandlerResponseFactory response, IChatService chatService, IHubContext<ChatHub> hubContext, IVonageService vonage)
    {
        _db = db;
        _response = response;
        _chatService = chatService;
        _hubContext = hubContext;
        _vonage = vonage;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerGuestRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.CustomerGuests.Where(e => e.CustomerReservationId == request.In.CustomerReservationId && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The customer guest already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}

        var customerGuest = await _db.CustomerGuests.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest == null)
        {
            return _response.Error($"Customer guest could not be found.", AppStatusCodeError.Gone410);
        }
        var customerReservation = await _db.CustomerReservations.Include(x => x.Customer).Where(e => e.Id == customerGuest.CustomerReservationId).FirstOrDefaultAsync(cancellationToken);
        if (customerReservation == null)
        {
            return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
        }

        var reservations = await _db.CustomerReservations
        .Where(r =>
            request.In.CheckinDate < r.CheckoutDate &&
            request.In.CheckoutDate > r.CheckinDate &&
            r.CustomerId == customerReservation.CustomerId &&
            r.Id != request.In.CustomerReservationId)
        .ToListAsync();

        bool isRoomAvailable = !reservations.Any(r =>
            _db.CustomerGuests.Any(g => g.RoomNumber == request.In.RoomNumber && g.CustomerReservationId == r.Id));

        if (!isRoomAvailable && request.In.CheckinDate != customerReservation.CheckinDate)
        {
            return _response.Error($"This Room is not available for the requested dates.", AppStatusCodeError.UnprocessableEntity422);
        }

        customerGuest.CustomerReservationId = request.In.CustomerReservationId;
        customerGuest.ArrivalFlightNumber = request.In.ArrivalFlightNumber;
        customerGuest.BlePinCode = request.In.BlePinCode;
        customerGuest.City = request.In.City;
        customerGuest.Country = request.In.Country;
        customerGuest.DepartureAirline = request.In.DepartureAirline;
        customerGuest.DepartureFlightNumber = request.In.DepartureFlightNumber;
        if (!string.IsNullOrEmpty(request.In.Email))
        {
            customerGuest.Email = request.In.Email;
        }
        customerGuest.FirstJourneyStep = request.In.FirstJourneyStep;
        if (!string.IsNullOrEmpty(request.In.Firstname))
        {
            customerGuest.Firstname = request.In.Firstname;
        }
        customerGuest.IdProof = request.In.IdProof;
        customerGuest.IdProofNumber = request.In.IdProofNumber;
        customerGuest.IdProofType = request.In.IdProofType;
        customerGuest.Language = request.In.Language;
        if (!string.IsNullOrEmpty(request.In.Lastname))
        {
            customerGuest.Lastname = request.In.Lastname;
        }
        customerGuest.PhoneCountry = request.In.PhoneCountry;
        if (!string.IsNullOrEmpty(request.In.PhoneNumber))
        {
            customerGuest.PhoneNumber =request.In.PhoneNumber;
        }
        customerGuest.Picture = request.In.Picture;
        customerGuest.Pin = request.In.Pin;
        customerGuest.Postalcode = request.In.Postalcode;
        customerGuest.Rating = request.In.Rating;
        customerGuest.RoomNumber = request.In.RoomNumber;
        customerGuest.Signature = request.In.Signature;
        customerGuest.Street = request.In.Street;
        customerGuest.StreetNumber = request.In.StreetNumber;
        customerGuest.TermsAccepted = request.In.TermsAccepted;
        customerGuest.DateOfBirth = request.In.DateOfBirth;
        customerGuest.Vat = request.In.Vat;
        customerGuest.IsActive = request.In.IsActive;
        customerGuest.BookingChannel = request.In.BookingChannel;
        customerGuest.DepartingFlightDate = request.In.DepartingFlightDate;
        if (request.In.IsCheckInCompleted != null)
            customerGuest.isCheckInCompleted = request.In.IsCheckInCompleted ?? false;
        if (request.In.IsSkipCheckIn != null)
            customerGuest.isSkipCheckIn = request.In.IsSkipCheckIn ?? false;
        customerReservation.CheckinDate = request.In.CheckinDate;
        customerReservation.CheckoutDate = request.In.CheckoutDate;

        await _db.SaveChangesAsync(cancellationToken);

        //send template message to guest when complete check in
        if (request.In.IsCheckInCompleted != null)
        {
            if ((bool)request.In.IsCheckInCompleted)
            {
                var customerGuestJournies = await _db.CustomerGuestJournies.Where(x => x.JourneyStep == (int)JourneyStepTempleteTypeEnums.OnlineCheckIn && x.CutomerId == customerReservation.CustomerId).ToListAsync(cancellationToken);

                var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == customerReservation.CustomerId).FirstOrDefaultAsync(cancellationToken);

                string bussinesAdress = $"{customerReservation.Customer.City},{customerReservation.Customer.Country},{customerReservation.Customer.Postalcode}";

                var customerUser = await _db.CustomerUsers.Where(x => x.CustomerId == customerReservation.CustomerId && x.CustomerLevelId == 1).FirstOrDefaultAsync(cancellationToken);

                int count = 1;
                foreach (var item in customerGuestJournies)
                {
                    if (count == 1)
                    {
                        CustomerGuestJorneyDetails customerGuestJorneyDetails = new CustomerGuestJorneyDetails();
                        customerGuestJorneyDetails.TempletMessage = item.TempletMessage;
                        customerGuestJorneyDetails.Buttons = item.Buttons;
                        customerGuestJorneyDetails.BussinessName = customerReservation.Customer.BusinessName;
                        customerGuestJorneyDetails.BussinessAddress = bussinesAdress;
                        customerGuestJorneyDetails.BussinessPhoneNumber = customerReservation.Customer.PhoneNumber;
                        customerGuestJorneyDetails.ReservationNumber = customerReservation.ReservationNumber;
                        customerGuestJorneyDetails.GuestName = customerGuest.Firstname;
                        customerGuestJorneyDetails.CheckinDate = (DateTime)customerReservation.CheckinDate;
                        customerGuestJorneyDetails.CheckoutDate = (DateTime)customerReservation.CheckoutDate;
                        customerGuestJorneyDetails.CustomerWhatsAppNumber = customerReservation.Customer.WhatsappNumber;
                        customerGuestJorneyDetails.Phone = customerGuest.PhoneNumber;
                        customerGuestJorneyDetails.APPId = vonageCred.AppId;
                        customerGuestJorneyDetails.PrivateKey = vonageCred.AppPrivatKey;
                        customerGuestJorneyDetails.TemplateName = item.Name;
                        customerGuestJorneyDetails.GuestId = customerGuest.Id;
                        customerGuestJorneyDetails.CustomerUserId = customerUser.Id;


                        await SendWebChatMessage(customerGuestJorneyDetails, _vonage, customerGuest.GuestToken);
                        count++;
                    }
                }
            }
        }

        var updateCustomerGuestAlertsOut = new UpdatedCustomerGuestOut()
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerGuest.CustomerReservationId,
            ArrivalFlightNumber = customerGuest.ArrivalFlightNumber,
            BlePinCode = customerGuest.BlePinCode,
            City = customerGuest.City,
            Country = customerGuest.Country,
            DepartureAirline = customerGuest.DepartureAirline,
            DepartureFlightNumber = customerGuest.DepartureFlightNumber,
            Email = customerGuest.Email,
            FirstJourneyStep = customerGuest.FirstJourneyStep,
            Firstname = customerGuest.Firstname,
            IdProof = customerGuest.IdProof,
            IdProofNumber = customerGuest.IdProofNumber,
            IdProofType = customerGuest.IdProofType,
            Language = customerGuest.Language,
            Lastname = customerGuest.Lastname,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Picture = customerGuest.Picture,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Rating = customerGuest.Rating,
            RoomNumber = customerGuest.RoomNumber,
            Signature = customerGuest.Signature,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            TermsAccepted = customerGuest.TermsAccepted,
            DateOfBirth = customerGuest.DateOfBirth,
            Vat = customerGuest.Vat,
            IsActive = customerGuest.IsActive
        };

        var customerId = customerReservation.CustomerId ?? 0;
        var customerData = await _db.Customers.Where(s => s.Id == customerId).Include(b => b.BusinessType).Include(p => p.Product).FirstOrDefaultAsync();
        var channels = await _chatService.GetChannelDataFromUsersDetail(request.In.Id, ((int)UserTypeEnum.Guest).ToString(), customerId, ((int)UserTypeEnum.Customer).ToString());

        if (channels != null)
        {
            var UserInfo = new
            {
                BusinessName = customerReservation.Customer.BusinessName,
                FirstName = request.In.Firstname,
                lastName = request.In.Lastname,
                Email = request.In.Email,
                ProfilePicture = request.In.Picture,
                PhoneCountry = request.In.PhoneCountry,
                PhoneNumber = request.In.PhoneNumber,
                NoOfRooms = request.In.RoomNumber,
                BizType = customerData.BusinessType.BizType,
                ServicePackageName = customerData.Product.Name,
                CreatedAt = customerGuest.CreatedAt,
                UserType = (ChatUserTypeEnum.CustomerGuest).ToString(),
                UserId = request.In.Id,
                ChatId = channels.Id,
                IsActive = request.In.IsActive
            };

            await _hubContext.Clients.Group($"user-2-{customerId}").SendAsync("updateUserInfo", UserInfo);
        }

        return _response.Success(new UpdateCustomerGuestOut("update customer guest successful.", updateCustomerGuestAlertsOut));
    }
    public async Task<dynamic> SendWebChatMessage(CustomerGuestJorneyDetails message, IVonageService vonageService, string guestToken)
    {
        var realTemplateMessage = message.TempletMessage;
        var templateMessage = realTemplateMessage;

        var bodyplaceholderMatches = System.Text.RegularExpressions.Regex.Matches(realTemplateMessage, @"\{([^}]+)\}").ToList();

        Guid MessgaeUuid = new Guid();
        TimeSpan bookingDays = message.CheckoutDate - message.CheckinDate;

        if (bodyplaceholderMatches.Count > 0)
        {
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        templateMessage = templateMessage.Replace("{hotel_name}", message.BussinessName);
                        break;
                    case "{booking_days}":
                        templateMessage = templateMessage.Replace("{booking_days}", bookingDays.Days.ToString());
                        break;
                    case "{hotel_address}":
                        templateMessage = templateMessage.Replace("{hotel_address}", message.BussinessAddress);
                        break;
                    case "{hotel_phonenumber}":
                        templateMessage = templateMessage.Replace("{hotel_phonenumber}", message.BussinessPhoneNumber);
                        break;
                    case "{guest_name}":
                        templateMessage = templateMessage.Replace("{guest_name}", message.GuestName);
                        break;
                    case "{guest_reservationnumber}":
                        templateMessage = templateMessage.Replace("{guest_reservationnumber}", message.ReservationNumber);
                        break;
                    case "{checkin_date}":
                        templateMessage = templateMessage.Replace("{checkin_date}", message.CheckinDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{checkout_date}":
                        templateMessage = templateMessage.Replace("{checkout_date}", message.CheckoutDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{guest_url}":
                        templateMessage = templateMessage.Replace("{guest_url}", $"[Click Here]({guestToken})");
                        break;
                }
                count++;
            }
        }

        var SenderUserType = ((int)UserTypeEnum.Customer).ToString();
        var receiverUserType = ((int)UserTypeEnum.Guest).ToString();

        var channels = await _chatService.GetChannelDataFromUsersDetail(message.GuestId, receiverUserType, message.CustomerUserId, SenderUserType);

        if (channels != null)
        {
            var isChannelMessageExist = _db.ChannelMessages.Where(x => x.MsgReqType == (byte)MsgReqTypeEnum.welcomeMessage && x.ChannelId == channels.Id).Any();
            if (!isChannelMessageExist)
            {
                var isMessageSend = await SendMessage(channels.Id, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, templateMessage, null, (byte)MessageSourceEnum.WebChat, "Text", null, null, null);
            }
        }
        else
        {
            var chatId = await _chatService.CreateChat(_db, message.CustomerUserId.ToString(), SenderUserType, message.GuestId, ((ChatUserTypeEnum)int.Parse(receiverUserType)).ToString());
            var isChannelMessageExist = _db.ChannelMessages.Where(x => x.MsgReqType == (byte)MsgReqTypeEnum.welcomeMessage && x.ChannelId == chatId).Any();
            if (!isChannelMessageExist)
            {
                var isMessageSend = await SendMessage(chatId, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, templateMessage, null, (byte)MessageSourceEnum.WebChat, "Text", null, null, null);
            }
        }
        return new { MessageId = MessgaeUuid, TemplateMessage = templateMessage };
    }
    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, (int)MsgReqTypeEnum.welcomeMessage, null);
        if (chat != null)
        {
            totalUnreadMessageCount += 1;
        }

        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}
